using Chromatica.Utils;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml;

namespace Chromatica.Studio
{
	[CustomEditor(typeof(ChromaticaVolume))]
	public class ChromaticaVolumeEditor : Editor
	{
		SerializedProperty p_transitionTime;
		SerializedProperty p_exitTransitionTime;
		SerializedProperty p_texture;
		SerializedProperty p_textureDepth;
		SerializedProperty p_depthCurve;
		SerializedProperty p_isDepthCurveEditorExpanded;

		SerializedProperty p_hasBeenReset;
		NativeCurveEditor m_CurveEditor;
		bool m_ForceCurveEditorRebuild = false;
		int indentSize = 14;

		void OnEnable()
		{
			p_transitionTime = serializedObject.FindProperty("TransitionTime");
			p_exitTransitionTime = serializedObject.FindProperty("ExitTransitionTime");
			p_texture = serializedObject.FindProperty("LookupTexture");
			p_textureDepth = serializedObject.FindProperty("LookupTextureDepth");
			p_depthCurve = serializedObject.FindProperty("DepthCurve");
			p_isDepthCurveEditorExpanded = serializedObject.FindProperty("IsDepthCurveEditorExpanded");
			p_hasBeenReset = serializedObject.FindProperty("HasBeenReset");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_transitionTime, new GUIContent("Enter Transition Time"));
			p_transitionTime.floatValue = Mathf.Max(p_transitionTime.floatValue, 0f);
			EditorGUILayout.PropertyField(p_exitTransitionTime, new GUIContent("Exit Transition Time"));
			p_exitTransitionTime.floatValue = Mathf.Max(p_exitTransitionTime.floatValue, 0f);

			EditorGUILayout.PropertyField(p_texture, new GUIContent("LUT", "Base / Near Lookup Texture."));
			CheckLutFormat(p_texture);
			EditorGUILayout.PropertyField(p_textureDepth, new GUIContent("Depth LUT", "Far Lookup Texture."));
			CheckLutFormat(p_textureDepth);

			CheckCurveEditor();
			p_isDepthCurveEditorExpanded.boolValue = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), p_isDepthCurveEditorExpanded.boolValue, "Depth", true);
			GUI.changed = false;

			if (p_isDepthCurveEditorExpanded.boolValue)
			{
				EditorGUI.indentLevel++;

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

				EditorGUI.indentLevel--;
			}

			if (GUI.changed)
				((ChromaticaVolume)target).UpdateCurve();

			serializedObject.ApplyModifiedProperties();
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

		[MenuItem("GameObject/Chromatica Volume", false, 20)]
		static void Create()
		{
			GameObject gameObject = new GameObject("Chromatica Volume");
			gameObject.AddComponent<ChromaticaVolume>();
			Selection.objects = new GameObject[1] { gameObject };
			EditorApplication.ExecuteMenuItem("GameObject/Move To View");
			Undo.RegisterCreatedObjectUndo(gameObject, "Created New Chromatica Volume");
		}

		void CheckLutFormat(SerializedProperty prop)
		{
			Texture nt = prop.objectReferenceValue as Texture;

			if (nt != null && nt.height != (int)Mathf.Sqrt(nt.width))
				EditorGUILayout.HelpBox("Invalid LUT format. Please re-bake the LUT or use the LUT converter. Read the upgrade guide for more information.", MessageType.Error);
		}
	}
}
