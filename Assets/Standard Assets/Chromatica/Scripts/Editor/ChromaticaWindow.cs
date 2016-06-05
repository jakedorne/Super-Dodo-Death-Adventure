using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using Chromatica.Operators;
using Chromatica.Studio.Editors;
using Chromatica.Utils;
using Object = UnityEngine.Object;

namespace Chromatica.Studio
{
	[Serializable]
	public class ChromaticaWindow : EditorWindow
	{
		// Global instance
		public static ChromaticaWindow inst { get; private set; }

		public float Width { get { return position.width; } }
		public float Height { get { return position.height; } }

		public ChromaticaRenderer Renderer { get; private set; }

		List<OperatorEditor> m_OperatorEditors;
		ChromaticaStudio m_CurrentContext;
		float m_OldWidth = 0f;
		Texture2D m_TempLUTTexture;
		bool m_IsEditingDepth;
		ChromaticaVolume m_SelectedVolumeContext;
		bool m_VolumeContextDirty = false;

		[SerializeField]
		Vector2 m_ScrollViewPosition;

		[MenuItem("Window/Chromatica Studio %&c")]
		public static void Init()
		{
			EditorWindow window = EditorWindow.GetWindow<ChromaticaWindow>();

#if UNITY_4_5 || UNITY_4_6 || UNITY_5_0
			window.title = "Chromatica";

			// Title icon
			GUIContent windowTitle = GUIUtility.GetTitleContent(window);

			if (windowTitle != null)
			{
				windowTitle.text = " Chromatica";
				windowTitle.image = Styles.LoadPNG("CS_Icon_Small");
			}
#else
			GUIContent windowTitle = new GUIContent("Chromatica", Styles.LoadPNG("CS_Icon_Small"));
			window.titleContent = windowTitle;
#endif

			window.autoRepaintOnSceneChange = true;
			window.wantsMouseMove = true;
			window.minSize = new Vector2(Preferences.LockMinWidth ? 488f : 10f, 200f);
			window.Show();
		}

		void OnEnable()
		{
			inst = this;
			Renderer = new ChromaticaRenderer();

			// Title icon
			GUIContent windowTitle = GUIUtility.GetTitleContent(this);

			if (windowTitle != null)
			{
				windowTitle.text = " Chromatica";
				windowTitle.image = Styles.LoadPNG("CS_Icon_Small");
			}
		}

		void OnDisable()
		{
			GUI.FocusControl(null);
		}

		void OnDestroy()
		{
			if (Renderer != null)
				Renderer.Destroy();
			Styles.inst.Destroy();
		}

		// 10 times per second
		void OnInspectorUpdate()
		{
			// Repaint the editor
			Repaint();

			// Switch volume context if needed
			if (m_VolumeContextDirty)
			{
				m_CurrentContext.VolumeContext = m_SelectedVolumeContext;
				m_VolumeContextDirty = false;
			}
		}

		// 100 times per second
		void Update()
		{
			if (m_CurrentContext != null && Renderer.Update(m_CurrentContext))
				Repaint();
		}

		void CheckSelection()
		{
			GameObject gameObject = Selection.activeGameObject;

			if (gameObject != null)
			{
				ChromaticaStudio chromatica = gameObject.GetComponent<ChromaticaStudio>();

				if (chromatica != null)
				{
					Renderer.Camera = gameObject.GetComponent<Camera>();

					if (chromatica != m_CurrentContext)
					{
						Renderer.SetDirty();
						GUIUtility.RepaintGameView();
					}

					m_CurrentContext = chromatica;
					return;
				}
			}

			m_CurrentContext = null;
		}

		void CheckOperatorEditors(bool forceRebuild = false)
		{
			if (m_OperatorEditors == null)
				m_OperatorEditors = new List<OperatorEditor>();

			if (!forceRebuild && !IsEditorListDirty())
				return;

			m_OperatorEditors.Clear();

			foreach (Operator op in m_CurrentContext.ActiveOperators)
			{
				string typeName = "Chromatica.Studio.Editors." + op.GetType().Name + "Editor";
				Type editorType = Type.GetType(typeName, true, false);
				OperatorEditor editor = (OperatorEditor)Activator.CreateInstance(editorType);
				editor.Init(op);
				m_OperatorEditors.Add(editor);
			}
		}

		bool IsEditorListDirty()
		{
			if (m_OperatorEditors.Count != m_CurrentContext.ActiveOperators.Count)
				return true;

			for (int i = 0; i < m_CurrentContext.ActiveOperators.Count; i++)
			{
				if (m_OperatorEditors[i].Operator != m_CurrentContext.ActiveOperators[i])
					return true;
			}

			return false; // No rebuild needed
		}

		void OnGUI()
		{
			CheckSelection();

			if (m_CurrentContext != null)
			{
				// Filter the operator list to remove all null instances (there shouldn't be any)
				m_CurrentContext.ActiveOperators.RemoveAll(item => item == null);
				CheckOperatorEditors();
			}

			// Blank UI
			if (m_CurrentContext == null)
			{
				DrawSetup();
				return;
			}

			// Header
			DrawHeader();
			GUI.changed = false;

			// Borders for depth edit mode
			if (m_IsEditingDepth)
			{
				Rect lastRect = GUILayoutUtility.GetLastRect();
				GUI.Box(new Rect(0, lastRect.height - 1, Width, Height - lastRect.height + 1), "", Styles.inst.YellowBox);
			}

			// Operators
			m_ScrollViewPosition = EditorGUILayout.BeginScrollView(m_ScrollViewPosition);

			if (m_CurrentContext.ActiveOperators != null)
			{
				for (int i = 0; i < m_CurrentContext.ActiveOperators.Count; i++)
				{
					if (m_CurrentContext.ActiveOperators[i] == null)
						continue;

					DrawOperator(m_CurrentContext.ActiveOperators[i], m_OperatorEditors[i]);
				}
			}

			// Add operator button
			DrawAddButton();

			// End UI
			EditorGUILayout.EndScrollView();

			// Refresh render preview
			if (m_OldWidth != Width || GUI.changed || Event.current.commandName == "UndoRedoPerformed")
				Renderer.SetDirty();

			// Change LUT mode
			if (Event.current.type != EventType.Repaint)
				m_CurrentContext.IsEditingDepthLUT = m_IsEditingDepth;

			m_OldWidth = Width;
		}

		void DrawSetup()
		{
			EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
				GUILayout.FlexibleSpace();

				EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
					GUILayout.FlexibleSpace();
					EditorGUILayout.BeginVertical();

						EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
							GUILayout.FlexibleSpace();
							GUILayout.Label("Welcome to Chromatica Studio!", EditorStyles.boldLabel);
							GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();

						GameObject gameObject = Selection.activeGameObject;

						if (gameObject != null && gameObject.GetComponent<Camera>() != null && gameObject.GetComponent<ChromaticaStudio>() == null)
						{
							GUILayout.Space(4);
							if (GUILayout.Button("Setup Chromatica Studio\non this camera", GUILayout.Width(300), GUILayout.Height(60)))
							{
								gameObject.AddComponent<ChromaticaStudio>();
								Notification("Chromatica Ready");
							}
						}
						else
						{
							EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
								GUILayout.FlexibleSpace();
								GUILayout.Label("Please select a Camera to continue.");
								GUILayout.FlexibleSpace();
							EditorGUILayout.EndHorizontal();
							GUILayout.Space(4);
						}


						if (GUILayout.Button("About & Help", GUILayout.Width(300), GUILayout.Height(30)))
						{
							Chromatica_StartupWindow.Init(true);
						}

					EditorGUILayout.EndVertical();
					GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();

				GUILayout.FlexibleSpace();
			EditorGUILayout.EndVertical();
		}

		void DrawHeader()
		{
			// Buttons
			GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));

			// Help
			if (GUILayout.Button("Help", EditorStyles.toolbarButton, GUILayout.MinWidth(50f)))
			{
				Application.OpenURL("http://www.thomashourdel.com/chromatica/doc/");
			}

			// Project
			if (GUILayout.Button("Lut", EditorStyles.toolbarPopup, GUILayout.MinWidth(50f)))
			{
				GenericMenu menu = new GenericMenu();

				// Clear
				menu.AddItem(new GUIContent("Clear"), false, () =>
				{
					if (m_CurrentContext.ActiveOperators.Count > 0 && EditorUtility.DisplayDialog("Clear", "This will remove all operators. Are you sure ?", "Yes", "No"))
						ClearOperators();
				});

				// Clear Both
				if (m_CurrentContext.Mode == LUTMode.Dual)
				{
					menu.AddItem(new GUIContent("Clear Both"), false, () =>
					{
						if ((m_CurrentContext.ActiveContext.Operators.Count > 0 || m_CurrentContext.ActiveContext.DepthOperators.Count > 0) && EditorUtility.DisplayDialog("Clear Both", "This will remove all operators from both LUT. Are you sure ?", "Yes", "No"))
						{
							ClearOperators(m_CurrentContext.ActiveContext.Operators);
							ClearOperators(m_CurrentContext.ActiveContext.DepthOperators);
						}
					});
				}

				menu.AddSeparator("");

				// Presets
				menu.AddItem(new GUIContent("Save Preset"), false, OnPresetSave);
				menu.AddItem(new GUIContent("Load Preset"), false, OnPresetLoad);

				if (m_CurrentContext.Mode == LUTMode.Dual)
				{
					menu.AddSeparator("");
					menu.AddItem(new GUIContent("Copy Primary -> Depth"), false, OnCopyPrimaryToDepth);
				}

				menu.ShowAsContext();
			}

			// Split
			GUI.enabled = m_CurrentContext.ScreenMask == null;

			if (GUILayout.Button("Split", EditorStyles.toolbarPopup, GUILayout.MinWidth(50f)))
			{
				GenericMenu menu = new GenericMenu();
				menu.AddItem(new GUIContent("None"), m_CurrentContext.Split == LUTSplit.None, () => { m_CurrentContext.Split = LUTSplit.None; GUIUtility.RepaintGameView(); });
				menu.AddItem(new GUIContent("Horizontal"), m_CurrentContext.Split == LUTSplit.Horizontal, () => { m_CurrentContext.Split = LUTSplit.Horizontal; GUIUtility.RepaintGameView(); });
				menu.AddItem(new GUIContent("Vertical"), m_CurrentContext.Split == LUTSplit.Vertical, () => { m_CurrentContext.Split = LUTSplit.Vertical; GUIUtility.RepaintGameView(); });
				menu.ShowAsContext();
			}

			GUI.enabled = true;

			GUILayout.FlexibleSpace();

			// Volume mode
			if (Event.current.commandName == "ObjectSelectorClosed")
			{
				Object obj = EditorGUIUtility.GetObjectPickerObject();
				m_SelectedVolumeContext = (obj == null) ? null : ((GameObject)obj).GetComponent<ChromaticaVolume>();
				m_VolumeContextDirty = true;
			}

			Color oldGuiColor = GUI.color;
			if (m_CurrentContext.VolumeContext != null)
				GUI.color = Color.red;

			if (GUILayout.Button("Context", EditorStyles.toolbarButton))
			{
				int controlID = EditorGUIUtility.GetControlID(FocusType.Passive);
				EditorGUIUtility.ShowObjectPicker<ChromaticaVolume>(m_CurrentContext.VolumeContext, true, "", controlID);
			}

			GUI.color = oldGuiColor;

			// Editing mode
			if (m_CurrentContext.Mode == LUTMode.Dual)
			{
				oldGuiColor = GUI.color;
				if (m_IsEditingDepth)
					GUI.color = Color.yellow;

				m_IsEditingDepth = GUILayout.Toggle(m_CurrentContext.IsEditingDepthLUT, "Edit Depth", EditorStyles.toolbarButton, GUILayout.MinWidth(70f));
				GUI.color = oldGuiColor;
			}
			else
			{
				m_IsEditingDepth = false;
			}

			// Add operator
			if (GUILayout.Button("Operator", EditorStyles.toolbarPopup, GUILayout.MinWidth(70f)))
			{
				OperatorTables.AddOperatorMenu(this);
			}

			// Bake | Bake As
			GUIStyle bakeStyle = Styles.inst.ToolbarSeparatedDropdown;
			Rect rect = GUILayoutUtility.GetRect(new GUIContent("Bake"), bakeStyle, GUILayout.MinWidth(60f));
			Rect position = new Rect(rect.xMax - (float)bakeStyle.border.right, rect.y, (float)bakeStyle.border.right, rect.height);

			if (Event.current.type == EventType.MouseDown && position.Contains(Event.current.mousePosition))
			{
				Event.current.Use();

				GenericMenu menu = new GenericMenu();
				menu.AddItem(new GUIContent("Bake"), false, () => OnBake());
				menu.AddItem(new GUIContent("Bake As"), false, () => OnBakeAs());
				menu.ShowAsContext();
			}

			if (GUI.Button(rect, "Bake", Styles.inst.ToolbarSeparatedDropdown))
			{
				OnBake();
			}

			GUILayout.EndHorizontal();
		}

		void DrawOperator(Operator op, OperatorEditor editor)
		{
			Rect foldRect = GUILayoutUtility.GetRect(new GUIContent(op.Title), Styles.inst.Foldout);
			Rect buttonRect = new Rect(foldRect.x + foldRect.width - 28f, foldRect.y, 28f, foldRect.height);
			GUI.Box(foldRect, new GUIContent(), op.IsEnabled ? Styles.inst.FoldoutBackground : Styles.inst.FoldoutBackgroundDisabled);

			// Context menu
			if (GUIUtility.ClickedOn(foldRect, 1) || GUIUtility.ClickedOn(buttonRect, 0))
			{
				GenericMenu menu = new GenericMenu();

				// Enable/Disable
				menu.AddItem(new GUIContent(op.IsEnabled ? "Disable" : "Enable"), false, () =>
				{
					Undo.RecordObject(op, (op.IsEnabled ? "Disable" : "Enable") + " Operator");
					op.IsEnabled = !op.IsEnabled;
					GUIUtility.RepaintGameView();
				});

				menu.AddSeparator("");

				// Reset to default
				menu.AddItem(new GUIContent("Reset"), false, () =>
				{
					Undo.RecordObject(op, "Reset Operator");
					op.ResetToDefault();
					editor.Reset();
					GUIUtility.RepaintGameView();
				});

				// Remove
				menu.AddItem(new GUIContent("Remove"), false, () =>
				{
					Undo.IncrementCurrentGroup();
					int undoGroup = Undo.GetCurrentGroup();
					Undo.RegisterCompleteObjectUndo(m_CurrentContext.ActiveContext, "Remove Operator");
					RemoveOperator(op);
					Undo.DestroyObjectImmediate(op);
					Undo.CollapseUndoOperations(undoGroup);
					GUIUtility.RepaintGameView();
				});

				menu.AddSeparator("");

				// Move up in the stack
				menu.AddItem(new GUIContent("Move Up"), false, IsOperatorFirst(op) ? null : (GenericMenu.MenuFunction)(() =>
				{
					Undo.RecordObject(m_CurrentContext.ActiveContext, "Move Operator Up");
					MoveOperatorUp(op);
					GUIUtility.RepaintGameView();
				}));

				// Move down the stack
				menu.AddItem(new GUIContent("Move Down"), false, IsOperatorLast(op) ? null : (GenericMenu.MenuFunction)(() =>
				{
					Undo.RecordObject(m_CurrentContext.ActiveContext, "Move Operator Down");
					MoveOperatorDown(op);
					GUIUtility.RepaintGameView();
				}));

				menu.AddSeparator("");

				// Unfold every operator
				menu.AddItem(new GUIContent("Expand All"), false, () =>
				{
					foreach (Operator o in m_CurrentContext.ActiveOperators)
						o.IsUnfolded = true;
				});

				// Fold every operator
				menu.AddItem(new GUIContent("Collapse All"), false, () =>
				{
					foreach (Operator o in m_CurrentContext.ActiveOperators)
						o.IsUnfolded = false;
				});

				menu.ShowAsContext();
				GUI.changed = true;
			}

			EditorGUI.BeginDisabledGroup(!op.IsEnabled);

			op.IsUnfolded = EditorGUI.Foldout(foldRect, op.IsUnfolded, op.Title, true, Styles.inst.Foldout);

			if (op.IsUnfolded)
			{
				EditorGUILayout.BeginVertical(Styles.inst.InspectorMargins);
				editor.DrawContentUI();
				EditorGUILayout.EndVertical();
			}

			EditorGUI.EndDisabledGroup();
		}

		void DrawAddButton()
		{
			GUILayout.Space(8);

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Add Operator", GUILayout.MinWidth(200f), GUILayout.MinHeight(22f)))
				OperatorTables.AddOperatorMenu(this);

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(8);
		}

		Texture2D GetTempLUTTexture()
		{
			if (m_TempLUTTexture == null)
			{
				m_TempLUTTexture = new Texture2D(256, 16, TextureFormat.ARGB32, false, InternalColorSpace.TEXrw);
				m_TempLUTTexture.hideFlags = HideFlags.DontSave;
			}

			return m_TempLUTTexture;
		}

		void OnBake()
		{
			// A texture is already assigned, overwrite it
			if (m_CurrentContext.ActiveContext.LookupTexture != null && m_CurrentContext.ActiveContext.LookupTexture is Texture2D &&
				m_CurrentContext.ActiveContext.LookupTexture.height == (int)(Mathf.Sqrt(m_CurrentContext.ActiveContext.LookupTexture.width)))
			{
				Texture2D lut = (Texture2D)m_CurrentContext.ActiveContext.LookupTexture;
				Texture2D depthLut =
					(m_CurrentContext.ActiveContext.LookupTextureDepth != null && m_CurrentContext.ActiveContext.LookupTextureDepth is Texture2D) ?
					(Texture2D)m_CurrentContext.ActiveContext.LookupTextureDepth :
					null;

				if (m_CurrentContext.Mode == LUTMode.Dual)
				{
					if (depthLut == null)
						depthLut = GetTempLUTTexture();
				}

				string baseLutPath = Application.dataPath + AssetDatabase.GetAssetPath(lut).Substring(6);
				Renderer.Bake(lut, false);
				SaveBakedTexture(baseLutPath, lut, false);

				if (m_CurrentContext.Mode == LUTMode.Dual)
				{
					Renderer.Bake(depthLut, true);
					string depthLutPath = baseLutPath.Substring(0, baseLutPath.Length - 4) + "_Depth.png";
					SaveBakedTexture(depthLutPath, depthLut, true);
				}
			}

			// New texture
			else
			{
				OnBakeAs();
			}
		}

		void OnBakeAs()
		{
			string baseLutPath = EditorUtility.SaveFilePanel("Bake Texture", Application.dataPath + "/Chromatica/LUTs/", m_CurrentContext.ActiveContext.name, "png");

			if (baseLutPath.Length == 0)
				return;

			if (!baseLutPath.StartsWith(Application.dataPath))
			{
				Debug.LogError("You must choose a path inside your project folder !");
				Notification("Invalid path");
				return;
			}

			Renderer.Bake(GetTempLUTTexture(), false);
			SaveBakedTexture(baseLutPath, GetTempLUTTexture(), false);

			if (m_CurrentContext.Mode == LUTMode.Dual)
			{
				Renderer.Bake(GetTempLUTTexture(), true);
				string depthLutPath = baseLutPath.Substring(0, baseLutPath.Length - 4) + "_Depth.png";
				SaveBakedTexture(depthLutPath, GetTempLUTTexture(), true);
			}
		}

		void SaveBakedTexture(string path, Texture2D texture, bool isDepth)
		{
			string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
			byte[] textureBytes = texture.EncodeToPNG();

			try
			{
				File.WriteAllBytes(path, textureBytes);
			}
			catch (Exception e)
			{
				// Something went very wrong
				Debug.LogError(e.StackTrace);
				Notification("Error while writing the file");
				return;
			}

			// Texture import settings
			AssetDatabase.Refresh();
			TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(relativePath);
			importer.isReadable = true;
			importer.wrapMode = TextureWrapMode.Clamp;
			importer.filterMode = FilterMode.Bilinear;
			importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
			importer.anisoLevel = 0;
			importer.maxTextureSize = 512;
			importer.linearTexture = true;
			importer.mipmapEnabled = false;
			AssetDatabase.ImportAsset(relativePath);
			AssetDatabase.Refresh();

			// Set the reference on the scene
			if (isDepth)
				m_CurrentContext.ActiveContext.LookupTextureDepth = (Texture2D)AssetDatabase.LoadAssetAtPath(relativePath, typeof(Texture2D));
			else
				m_CurrentContext.ActiveContext.LookupTexture = (Texture2D)AssetDatabase.LoadAssetAtPath(relativePath, typeof(Texture2D));

			Notification("Bake done");
		}

		void OnPresetSave()
		{
			string depthSuffix = m_CurrentContext.IsEditingDepthLUT ? " Depth" : "";
			string path = EditorUtility.SaveFilePanel("Save Preset", Application.dataPath + "/Chromatica/Presets/", m_CurrentContext.ActiveContext.name + depthSuffix, "chromapreset");

			if (path.Length != 0)
			{
				string xml = "<Operators>";

				foreach (Operator op in m_CurrentContext.ActiveOperators)
				{
					if (op != null)
						xml += op.ToXML() + "\n";
				}

				xml += "</Operators>\n";

				try
				{
					File.WriteAllText(path, xml, System.Text.Encoding.UTF8);
				}
				catch (Exception e)
				{
					// Something went very wrong
					Debug.LogError(e.StackTrace);
					Notification("Error while writing the file");
					return;
				}

				AssetDatabase.Refresh();
				Notification("Preset saved");
			}
		}

		void OnPresetLoad()
		{
			string path = EditorUtility.OpenFilePanel("Load Preset", Application.dataPath + "/Chromatica/Presets/", "chromapreset");

			if (path.Length != 0)
			{
				string xml;

				try
				{
					xml = File.ReadAllText(path);
				}
				catch (Exception ex)
				{
					// Something went horribly wrong
					Debug.LogError(ex.StackTrace);
					Notification("Error while reading the file");
					return;
				}

				if (m_CurrentContext.ActiveOperators.Count > 0)
				{
					if (EditorUtility.DisplayDialog("Load Preset", "This will remove all operators. Are you sure ?", "Yes", "No"))
						ClearOperators();
					else
						return;
				}

				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(xml);
				XmlNode operatorsNode = xmlDoc.FirstChild;

				if (operatorsNode.Name != "Operators")
				{
					Debug.LogError("Invalid preset file");
					Notification("Invalid preset file");
					return;
				}

				foreach (XmlNode node in operatorsNode)
					AddOperator(OperatorTables.GetTypeForXMLName(node.Name), node);

				GUI.changed = true;
				Renderer.SetDirty();
				GUIUtility.RepaintGameView();
				m_ScrollViewPosition.y = 0f;

				Notification("Preset loaded");
			}
		}

		void OnCopyPrimaryToDepth()
		{
			if (m_CurrentContext.DepthOperators.Count > 0)
			{
				if (EditorUtility.DisplayDialog("Copy Primary -> Depth", "This will remove all operators from the depth Lut. Are you sure ?", "Yes", "No"))
					ClearOperators(m_CurrentContext.DepthOperators);
				else
					return;
			}

			foreach (Operator op in m_CurrentContext.Operators)
				AddOperator(op.Clone(), m_CurrentContext.DepthOperators);
		}

		internal void AddOperator(Operator op, List<Operator> context)
		{
			Undo.IncrementCurrentGroup();
			int undoGroup = Undo.GetCurrentGroup();
			Undo.RecordObject(m_CurrentContext.ActiveContext, "Add Operator");

			context.Add(op);

			Undo.RegisterCreatedObjectUndo(op, "Add Operator");
			Undo.CollapseUndoOperations(undoGroup);

			m_ScrollViewPosition.y += 99999999f;
			GUI.changed = true;
			Renderer.SetDirty();
		}

		internal void AddOperator(Type T, XmlNode node)
		{
			Undo.IncrementCurrentGroup();
			int undoGroup = Undo.GetCurrentGroup();
			Undo.RecordObject(m_CurrentContext.ActiveContext, "Add Operator");

			Operator op = (Operator)ScriptableObject.CreateInstance(T);
			op.Init();

			if (node != null)
				op.FromXML(node);

			m_CurrentContext.ActiveOperators.Add(op);

			Undo.RegisterCreatedObjectUndo(op, "Add Operator");
			Undo.CollapseUndoOperations(undoGroup);

			m_ScrollViewPosition.y += 99999999f;
			GUI.changed = true;
			Renderer.SetDirty();
		}

		void ClearOperators()
		{
			ClearOperators(m_CurrentContext.ActiveOperators);
		}

		void ClearOperators(List<Operator> operators)
		{
			Undo.IncrementCurrentGroup();
			int undoGroup = Undo.GetCurrentGroup();
			Undo.RegisterCompleteObjectUndo(m_CurrentContext.ActiveContext, "Clear Operators");

			for (int i = operators.Count - 1; i >= 0; i--)
			{
				if (operators[i] != null)
					Undo.DestroyObjectImmediate(operators[i]);

				operators.RemoveAt(i);
			}

			Undo.CollapseUndoOperations(undoGroup);

			GUI.changed = true;
			Renderer.SetDirty();
			GUIUtility.RepaintGameView();
		}

		public void RemoveOperator(Operator op)
		{
			m_CurrentContext.ActiveOperators.Remove(op);
		}

		public bool IsOperatorFirst(Operator op)
		{
			return m_CurrentContext.ActiveOperators.IndexOf(op) == 0;
		}

		public bool IsOperatorLast(Operator op)
		{
			return m_CurrentContext.ActiveOperators.IndexOf(op) == m_CurrentContext.ActiveOperators.Count - 1;
		}

		public void MoveOperatorUp(Operator op)
		{
			int index = m_CurrentContext.ActiveOperators.IndexOf(op);
			m_CurrentContext.ActiveOperators.RemoveAt(index);
			m_CurrentContext.ActiveOperators.Insert(index - 1, op);
		}

		public void MoveOperatorDown(Operator op)
		{
			int index = m_CurrentContext.ActiveOperators.IndexOf(op);
			m_CurrentContext.ActiveOperators.RemoveAt(index);
			m_CurrentContext.ActiveOperators.Insert(index + 1, op);
		}

		public void Notification(string text)
		{
			if (Preferences.ShowNotifications)
				ShowNotification(new GUIContent(text));
		}
	}
}
