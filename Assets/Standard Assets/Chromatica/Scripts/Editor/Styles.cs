using UnityEngine;
using UnityEditor;

namespace Chromatica.Studio
{
	public class Styles
	{
		static Styles m_Instance;
		public static Styles inst
		{
			get
			{
				if (m_Instance == null)
					m_Instance = new Styles();

				return m_Instance;
			}
		}

		public Texture2D FoldoutBackgroundTexture;
		public Texture2D FoldoutBackgroundTextureDisabled;
		public Texture2D FoldoutClosedTexture;
		public Texture2D FoldoutOpenedTexture;
		public Texture2D GrayscaleTexture;
		public Texture2D GrayscaleLinearTexture;
		public Texture2D WheelThumbTexture;
		public Texture2D YellowBoxTexture;
		public Texture2D HueRampTexture;

		public Color FoldoutForegroundColor;
		public Color HistogramLuminanceColor;
		public Color HistogramRedColor;
		public Color HistogramGreenColor;
		public Color HistogramBlueColor;
		public Color CurveEditorBackgroundColor;

		public GUIStyle Foldout;
		public GUIStyle FoldoutBackground;
		public GUIStyle FoldoutBackgroundDisabled;
		public GUIStyle InspectorMargins;
		public GUIStyle MiniTabLeft;
		public GUIStyle MiniTabMid;
		public GUIStyle MiniTabRight;
		public GUIStyle MiniTabLeftActive;
		public GUIStyle MiniTabMidActive;
		public GUIStyle MiniTabRightActive;
		public GUIStyle WheelThumb;
		public GUIStyle CurveBackground;
		public GUIStyle ToolbarSeparatedDropdown;
		public GUIStyle YellowBox;

		static string m_RelativeDataPath = "Assets/Chromatica/Internal/Textures/";

		public Styles()
		{
			// Get the relative data path
			string[] results = AssetDatabase.FindAssets("ChromaticaStudio t:Script", null);
			if (results.Length > 0)
			{
				string p = AssetDatabase.GUIDToAssetPath(results[0]);
				p = System.IO.Path.GetDirectoryName(p);
				p = p.Substring(0, p.LastIndexOf('/'));
				m_RelativeDataPath = p + "/Internal/Textures/";
			}

			// Colors & textures
			if (EditorGUIUtility.isProSkin)
			{
				FoldoutForegroundColor = new Color(180f / 255f, 180f / 255f, 180f / 255f, 1f);
				HistogramLuminanceColor = Color.white;
				HistogramRedColor = new Color(215f / 255f, 0f, 0f, 1f);
				HistogramGreenColor = new Color(0f, 215f / 255f, 0f, 1f);
				HistogramBlueColor = new Color(0f, 110f / 255f, 205f / 255f, 1f);

				FoldoutBackgroundTexture = LoadPNG("CS_DGUI_FoldoutBackground");
				FoldoutBackgroundTextureDisabled = LoadPNG("CS_DGUI_FoldoutBackgroundDisabled");
				FoldoutClosedTexture = LoadPNG("CS_DGUI_FoldoutClosed");
				FoldoutOpenedTexture = LoadPNG("CS_DGUI_FoldoutOpened");

				// Curves
				CurveBackground = new GUIStyle();
				CurveBackground.normal.background = CreateTexture(1, 1, new Color(38f / 255f, 38f / 255f, 38f / 255f));
			}
			else
			{
				FoldoutForegroundColor = new Color(220f / 255f, 220f / 255f, 220f / 255f, 1f);
				HistogramLuminanceColor = new Color(20f / 255f, 20f / 255f, 20f / 255f, 1f);
				HistogramRedColor = new Color(215f / 255f, 0f, 0f, 1f);
				HistogramGreenColor = new Color(0f, 180f / 255f, 0f, 1f);
				HistogramBlueColor = new Color(0f, 110f / 255f, 205f / 255f, 1f);

				FoldoutBackgroundTexture = LoadPNG("CS_LGUI_FoldoutBackground");
				FoldoutBackgroundTextureDisabled = LoadPNG("CS_LGUI_FoldoutBackgroundDisabled");
				FoldoutClosedTexture = LoadPNG("CS_LGUI_FoldoutClosed");
				FoldoutOpenedTexture = LoadPNG("CS_LGUI_FoldoutOpened");

				// Curves
				CurveBackground = new GUIStyle();
				CurveBackground.normal.background = CreateTexture(1, 1, new Color(100f / 255f, 100f / 255f, 100f / 255f));
			}

			GrayscaleTexture = LoadPNG("CS_Grayscale");
			GrayscaleLinearTexture = LoadPNG("CS_GrayscaleLinear");
			WheelThumbTexture = LoadPNG("CS_WheelThumb");
			YellowBoxTexture = LoadPNG("CS_YellowBox");
			HueRampTexture = LoadPNG("CS_HueRamp");

			FoldoutBackgroundTexture.hideFlags = HideFlags.HideAndDontSave;
			FoldoutBackgroundTextureDisabled.hideFlags = HideFlags.HideAndDontSave;
			FoldoutClosedTexture.hideFlags = HideFlags.HideAndDontSave;
			FoldoutOpenedTexture.hideFlags = HideFlags.HideAndDontSave;
			GrayscaleTexture.hideFlags = HideFlags.HideAndDontSave;
			GrayscaleLinearTexture.hideFlags = HideFlags.HideAndDontSave;
			WheelThumbTexture.hideFlags = HideFlags.HideAndDontSave;
			YellowBoxTexture.hideFlags = HideFlags.HideAndDontSave;
			HueRampTexture.hideFlags = HideFlags.HideAndDontSave;

			// Foldout
			Foldout = new GUIStyle(EditorStyles.foldout);
			Foldout.normal.textColor = FoldoutForegroundColor;
			Foldout.onNormal.textColor = FoldoutForegroundColor;
			Foldout.hover.textColor = FoldoutForegroundColor;
			Foldout.onHover.textColor = FoldoutForegroundColor;
			Foldout.active.textColor = FoldoutForegroundColor;
			Foldout.onActive.textColor = FoldoutForegroundColor;
			Foldout.focused.textColor = FoldoutForegroundColor;
			Foldout.onFocused.textColor = FoldoutForegroundColor;
			Foldout.normal.background = FoldoutClosedTexture;
			Foldout.onNormal.background = FoldoutOpenedTexture;
			Foldout.hover.background = FoldoutClosedTexture;
			Foldout.onHover.background = FoldoutOpenedTexture;
			Foldout.active.background = FoldoutClosedTexture;
			Foldout.onActive.background = FoldoutOpenedTexture;
			Foldout.focused.background = FoldoutClosedTexture;
			Foldout.onFocused.background = FoldoutOpenedTexture;
			Foldout.fixedHeight = 22f;
			Foldout.padding = new RectOffset(24, 10, 4, 0);
			Foldout.margin = new RectOffset(8, 8, 10, 6);
			Foldout.overflow.top = -4;
			Foldout.overflow.left = -7;
			Foldout.fontStyle = FontStyle.Bold;

			FoldoutBackground = new GUIStyle(GUI.skin.box);
			FoldoutBackground.normal.background = FoldoutBackgroundTexture;
			FoldoutBackground.border = new RectOffset(10, 19, 10, 10);

			FoldoutBackgroundDisabled = new GUIStyle(FoldoutBackground);
			FoldoutBackgroundDisabled.normal.background = FoldoutBackgroundTextureDisabled;

			// Inspector Margins
			InspectorMargins = new GUIStyle(EditorStyles.inspectorDefaultMargins);
			InspectorMargins.padding.right = InspectorMargins.padding.left + 3;

			// Tabs
			MiniTabLeft = new GUIStyle(EditorStyles.miniButtonLeft);
			MiniTabMid = new GUIStyle(EditorStyles.miniButtonMid);
			MiniTabRight = new GUIStyle(EditorStyles.miniButtonRight);

			MiniTabLeftActive = new GUIStyle(MiniTabLeft);
			MiniTabLeftActive.active = MiniTabLeft.onActive;
			MiniTabLeftActive.normal = MiniTabLeft.onNormal;
			MiniTabLeftActive.hover = MiniTabLeft.onHover;

			MiniTabMidActive = new GUIStyle(MiniTabMid);
			MiniTabMidActive.active = MiniTabMid.onActive;
			MiniTabMidActive.normal = MiniTabMid.onNormal;
			MiniTabMidActive.hover = MiniTabMid.onHover;

			MiniTabRightActive = new GUIStyle(MiniTabRight);
			MiniTabRightActive.active = MiniTabRight.onActive;
			MiniTabRightActive.normal = MiniTabRight.onNormal;
			MiniTabRightActive.hover = MiniTabRight.onHover;

			// Wheel thumb
			WheelThumb = new GUIStyle("ColorPicker2DThumb");
			WheelThumb.normal.background = WheelThumbTexture;

			// Toolbar
			ToolbarSeparatedDropdown = new GUIStyle("GV Gizmo DropDown");

			// Yellow border box
			YellowBox = new GUIStyle("Box");
			YellowBox.normal.background = YellowBoxTexture;
			YellowBox.border = new RectOffset(3, 3, 3, 3);
		}

		public void Destroy()
		{
			m_Instance = null;
		}

		Texture2D CreateTexture(int width, int height, Color fillColor)
		{
			Color[] colors = new Color[width * height];

			for (int i = 0; i < colors.Length; i++)
				colors[i] = fillColor;

			Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
			texture.SetPixels(colors);
			texture.Apply();
			texture.hideFlags = HideFlags.DontSave;

			return texture;
		}

		public static Texture2D LoadPNG(string name)
		{
			return (Texture2D)AssetDatabase.LoadAssetAtPath(m_RelativeDataPath + name + ".png", typeof(Texture2D));
		}

		public static Texture2D LoadTGA(string name)
		{
			return (Texture2D)AssetDatabase.LoadAssetAtPath(m_RelativeDataPath + name + ".tga", typeof(Texture2D));
		}
	}
}
