using Chromatica.Utils;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml;

namespace Chromatica.Studio
{
	[CustomEditor(typeof(ChromaticaStudio))]
	public class ChromaticaEditor : Editor
	{
		SerializedProperty p_mode;
		SerializedProperty p_split;
		SerializedProperty p_contribution;
		SerializedProperty p_screenMask;
		SerializedProperty p_screenMaskInvert;
		SerializedProperty p_screenMaskTilingOffset;
		SerializedProperty p_screenMaskOpacity;
		SerializedProperty p_disableAllBlending;
		SerializedProperty p_volumeTrigger;
		SerializedProperty p_volumeMask;
		SerializedProperty p_texture;
		SerializedProperty p_textureDepth;
		SerializedProperty p_depthCurve;
		SerializedProperty p_isDepthCurveEditorExpanded;
		SerializedProperty p_volumeContext;
		SerializedProperty p_useTonemapping;
		SerializedProperty p_exposure;

		SerializedProperty p_hasBeenReset;
		NativeCurveEditor m_CurveEditor;
		bool m_ForceCurveEditorRebuild = false;
		int indentSize = 14;

		void OnEnable()
		{
			p_split = serializedObject.FindProperty("Split");
			p_mode = serializedObject.FindProperty("Mode");
			p_contribution = serializedObject.FindProperty("Contribution");
			p_screenMask = serializedObject.FindProperty("ScreenMask");
			p_screenMaskInvert = serializedObject.FindProperty("ScreenMaskInvert");
			p_screenMaskOpacity = serializedObject.FindProperty("ScreenMaskOpacity");
			p_screenMaskTilingOffset = serializedObject.FindProperty("ScreenMaskTilingOffset");
			p_disableAllBlending = serializedObject.FindProperty("DisableAllBlending");
			p_volumeTrigger = serializedObject.FindProperty("VolumeTrigger");
			p_volumeMask = serializedObject.FindProperty("VolumeMask");
			p_texture = serializedObject.FindProperty("LookupTexture");
			p_textureDepth = serializedObject.FindProperty("LookupTextureDepth");
			p_depthCurve = serializedObject.FindProperty("DepthCurve");
			p_isDepthCurveEditorExpanded = serializedObject.FindProperty("IsDepthCurveEditorExpanded");
			p_volumeContext = serializedObject.FindProperty("VolumeContext");
			p_hasBeenReset = serializedObject.FindProperty("HasBeenReset");
			p_useTonemapping = serializedObject.FindProperty("UseTonemapping");
			p_exposure = serializedObject.FindProperty("Exposure");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			ChromaticaStudio chromatica = (ChromaticaStudio)serializedObject.targetObject;

			// Settings
			Header("Global Settings");

			EditorGUI.indentLevel++;
			int m = EditorGUILayout.Popup("Mode", p_mode.enumValueIndex, p_mode.enumNames);
			bool modeChanged = m != p_mode.enumValueIndex;
			p_mode.enumValueIndex = m;

			Transform volumeTrigger = (Transform)p_volumeTrigger.objectReferenceValue;
			string defaultText = (volumeTrigger == null) ? "" : " (Default)";

			GUI.enabled = p_screenMask.objectReferenceValue == null;
			EditorGUILayout.PropertyField(p_split, new GUIContent("Split", "Before / After preview in horizontal or vertical mode. Will not work if a Mask is set !"));
			GUI.enabled = true;

			EditorGUILayout.PropertyField(p_contribution, new GUIContent("Contribution", "Global contribution of the effect to the camera."));
			EditorGUILayout.PropertyField(p_disableAllBlending, new GUIContent("Disable All Blending", "Tick this to save memory if you don't need the Volume feature and don't intend to blend LUTs from scripts."));
			EditorGUI.indentLevel--;

			// Tonemapping
			Header("Tonemapping");

			EditorGUI.indentLevel++;

			EditorGUILayout.PropertyField(p_useTonemapping, new GUIContent("Enable"));

			if (p_useTonemapping.boolValue)
			{
				EditorGUILayout.PropertyField(p_exposure);

				if (!chromatica.GetComponent<Camera>().hdr)
					EditorGUILayout.HelpBox("The input is not in HDR. Make sure that the camera has HDR enabled and all effects prior to Chromatica are executed in HDR.", MessageType.Warning);
			}

			EditorGUI.indentLevel--;

			// Lookup textures & depth curve
			Header("Lookup Textures");

			EditorGUI.indentLevel++;
			GUI.changed = false;
			EditorGUILayout.PropertyField(p_texture, new GUIContent("LUT" + defaultText, "Base / Near Lookup Texture."));
			CheckLutFormat(p_texture);

			if (p_mode.intValue == (int)LUTMode.Dual)
			{
				EditorGUILayout.PropertyField(p_textureDepth, new GUIContent("Depth LUT" + defaultText, "Far Lookup Texture."));
				CheckLutFormat(p_textureDepth);
				CheckCurveEditor();
				p_isDepthCurveEditorExpanded.boolValue = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), p_isDepthCurveEditorExpanded.boolValue, "Depth", true);

				if (p_isDepthCurveEditorExpanded.boolValue)
				{
					if (m_CurveEditor == null)
					{
						p_depthCurve.animationCurveValue = EditorGUILayout.CurveField(new GUIContent("Curve", "Depth interpolation curve."), p_depthCurve.animationCurveValue, Color.white, new Rect(0f, 0f, 1f, 1f));
						CurvePresets();
					}
					else
					{
						Rect rect = GUILayoutUtility.GetRect(10, 300);
						rect = EditorGUI.IndentedRect(rect);
						p_depthCurve.animationCurveValue = m_CurveEditor.OnGUI(rect)[0];
						CurvePresets();
					}
				}

				if (GUI.changed)
					chromatica.UpdateCurve();
			}
			EditorGUI.indentLevel--;

			// Mask
			Header("Screen Mask");

			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(p_screenMaskOpacity, new GUIContent("Opacity", "Global mask opacity."));
			EditorGUILayout.PropertyField(p_screenMask, new GUIContent("Mask", "A black & white or colored Texture to use as a screen mask. Setting a mask will override the Split feature."));

			Vector4 tilingOffset = p_screenMaskTilingOffset.vector4Value;
			Vector2 tiling = new Vector2(tilingOffset.x, tilingOffset.y);
			Vector2 offset = new Vector2(tilingOffset.z, tilingOffset.w);
			tiling = EditorGUILayout.Vector2Field(new GUIContent("Tiling", "Placement scale of the mask texture."), tiling);
			offset = EditorGUILayout.Vector2Field(new GUIContent("Offset", "Placement offset of the mask texture."), offset);
			p_screenMaskTilingOffset.vector4Value = new Vector4(tiling.x, tiling.y, offset.x, offset.y);

			EditorGUILayout.PropertyField(p_screenMaskInvert, new GUIContent("Invert", "Invert the mask texture."));
			EditorGUI.indentLevel--;

			// Blending
			if (!p_disableAllBlending.boolValue)
			{
				Header("Blending");

				EditorGUI.indentLevel++;
				EditorGUILayout.BeginHorizontal();
					Prefix("Volume Trigger", "Select an object to test against volumes or keep empty to disable this feature.");
					EditorGUILayout.BeginHorizontal();
						volumeTrigger = (Transform)EditorGUILayout.ObjectField(volumeTrigger, typeof(Transform), true);
						if (GUILayout.Button(new GUIContent("Self", "Use this GameObject as Volume Trigger."), EditorStyles.miniButtonLeft)) volumeTrigger = chromatica.transform;
						if (GUILayout.Button(new GUIContent("None", "Disable the volume feature."), EditorStyles.miniButtonRight)) volumeTrigger = null;
						p_volumeTrigger.objectReferenceValue = volumeTrigger;
					EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.PropertyField(p_volumeMask, new GUIContent("Layer Mask", "Collision layer used by volumes. Use a custom one for better performances !"));
				int mask = p_volumeMask.intValue;

				if (mask == 0)
					EditorGUILayout.HelpBox("No Layer Mask selected, the volume feature won't work !", MessageType.Warning);
				else if (mask == -1)
					EditorGUILayout.HelpBox("Layer Mask is set to \"Everything\" ! Put your volumes in a custom layer for better performances.", MessageType.Warning);
				EditorGUI.indentLevel--;
			}

			// Edit
			Header("Edit Mode");

			if (chromatica.EditModeLoookupTexture != null)
			{
				EditorGUI.indentLevel++;
				if (ChromaticaWindow.inst != null)
				{
					string mode = chromatica.IsEditingDepthLUT ? " (depth)" : "";
					EditorGUILayout.HelpBox("Chromatica is currently in edit mode" + mode + ".", MessageType.Info);

					GUILayout.Space(-32);
					EditorGUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("Exit Edit Mode", GUILayout.Width(120)))
						{
							ChromaticaWindow.inst.Close();
							chromatica.EditModeLoookupTexture = null;
							chromatica.EditModeLoookupTextureDepth = null;
						}
						GUILayout.Space(8);
					EditorGUILayout.EndHorizontal();
					GUILayout.Space(11);

					GUILayout.BeginHorizontal();
						Prefix("Volume Context", "Select a volume to edit or set to null to edit the camera LUTs.");
						EditorGUILayout.BeginHorizontal();
							p_volumeContext.objectReferenceValue = (ChromaticaVolume)EditorGUILayout.ObjectField(p_volumeContext.objectReferenceValue, typeof(ChromaticaVolume), true);
							if (GUILayout.Button(new GUIContent("None", "Empty the field to edit the camera LUTs."), EditorStyles.miniButton)) p_volumeContext.objectReferenceValue = null;
						GUILayout.EndHorizontal();
					GUILayout.EndHorizontal();
				}
				else
				{
					chromatica.EditModeLoookupTexture = null;
					chromatica.EditModeLoookupTextureDepth = null;
				}
				EditorGUI.indentLevel--;
			}
			else if (GUILayout.Button("Open Chromatica Studio", GUILayout.Height(58)))
			{
				ChromaticaWindow.Init();
			}
			
			if (GUILayout.Button("About - Documentation - Changelog", EditorStyles.miniButton))
				Chromatica_StartupWindow.Init(true);

			serializedObject.ApplyModifiedProperties();

			// Mode was changed, re-render if needed
			if (modeChanged && ChromaticaWindow.inst != null && ChromaticaWindow.inst.Renderer != null)
				ChromaticaWindow.inst.Renderer.Update(chromatica, true);
		}

		void Header(string title)
		{
			GUILayout.Space(6);
			EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
		}

		void Prefix(string label, string tooltip = "")
		{
			EditorGUIUtility.labelWidth = EditorGUIUtility.labelWidth - EditorGUI.indentLevel * indentSize;
			EditorGUILayout.PrefixLabel(new GUIContent(label, tooltip));
			EditorGUIUtility.labelWidth = EditorGUIUtility.labelWidth + EditorGUI.indentLevel * indentSize;
		}

		void CheckCurveEditor()
		{
			if (p_hasBeenReset.boolValue)
			{
				p_hasBeenReset.boolValue = false;
				m_ForceCurveEditorRebuild = true;
			}

			if (!m_ForceCurveEditorRebuild)
				if (!Preferences.UseCustomCurveEditor || m_CurveEditor != null)
					return;

			try
			{
				m_CurveEditor = new NativeCurveEditor(new AnimationCurve[] { p_depthCurve.animationCurveValue }, new Color[] { Color.white });
			}
			catch
			{
				m_CurveEditor = null;
			}

			m_ForceCurveEditorRebuild = false;
		}

		void CurvePresets()
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(EditorGUI.indentLevel * indentSize);

			if (GUILayout.Button(new GUIContent("Save Curve Preset", "Save the curve to a file."), EditorStyles.miniButton))
			{
				string path = EditorUtility.SaveFilePanel("Save Curve Preset", Application.dataPath + "/Chromatica/Presets/", "curve", "curvepreset");

				if (path.Length != 0)
				{
					string data = Serializer.SerializeCurve("Preset", p_depthCurve.animationCurveValue);
					File.WriteAllText(path, data);
					AssetDatabase.Refresh();
				}
			}

			if (GUILayout.Button(new GUIContent("Load Curve Preset", "Load the curve from a file."), EditorStyles.miniButton))
			{
				string path = EditorUtility.OpenFilePanel("Load Curve Preset", Application.dataPath + "/Chromatica/Presets/", "curvepreset");

				if (path.Length != 0)
				{
					string xml;

					try
					{
						xml = File.ReadAllText(path);
					}
					catch (Exception ex)
					{
						// Something went wrong, do not continue
						Debug.LogError(ex.StackTrace);
						return;
					}

					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(xml);
					XmlNode node = xmlDoc.FirstChild;
					p_depthCurve.animationCurveValue = Serializer.DeserializeCurve(node, new AnimationCurve());
					GUI.changed = true;
					m_ForceCurveEditorRebuild = true;
					serializedObject.ApplyModifiedProperties();
				}
			}

			GUILayout.EndHorizontal();
		}

		void CheckLutFormat(SerializedProperty prop)
		{
			Texture nt = prop.objectReferenceValue as Texture;

			if (nt != null && nt.height != (int)Mathf.Sqrt(nt.width))
				EditorGUILayout.HelpBox("Invalid LUT format. Please re-bake the LUT or use the LUT converter. Read the upgrade guide for more information.", MessageType.Error);
		}
	}
}
