using UnityEngine;
using UnityEditor;

namespace Chromatica.Studio
{
	[InitializeOnLoad]
	public class Preferences
	{
		static bool loaded = false;

		public static bool LockMinWidth = true;
		public static bool ShowNotifications = true;
		public static int Decimals = 3;
		public static bool UseCustomCurveEditor = false;
		public static float ColorWheelSensitivity = 0.5f;
		public static bool AutoSwitchToGameView = true;
		public static Color VolumeColor = new Color(0.15f, 0.69f, 0.93f, 1f);

		static Preferences()
		{
			Load();
		}

		[PreferenceItem("Chromatica")]
		static void PreferenceGUI()
		{
			if (!loaded)
				Load();

			GUILayout.Space(10);
			LockMinWidth = EditorGUILayout.Toggle("Lock Minimum Editor Width", LockMinWidth);
			ShowNotifications = EditorGUILayout.Toggle("Show Notifications", ShowNotifications);
			UseCustomCurveEditor = EditorGUILayout.Toggle("Experimental Curve Editor", UseCustomCurveEditor);
			AutoSwitchToGameView = EditorGUILayout.Toggle("Auto Switch To Game View", AutoSwitchToGameView);
			Decimals = EditorGUILayout.IntSlider("Precision (decimals)", Decimals, 1, 7);
			ColorWheelSensitivity = EditorGUILayout.Slider("Color Wheel Sensitivity", ColorWheelSensitivity, 0.01f, 1f);

			VolumeColor = EditorGUILayout.ColorField("Volume Color", VolumeColor);
			ChromaticaVolume.BoundsColor = VolumeColor;

			if (GUI.changed)
				Save();
		}

		internal static void Load()
		{
			LockMinWidth = EditorPrefs.GetBool("Chroma.LockMinWidth", true);
			ShowNotifications = EditorPrefs.GetBool("Chroma.ShowNotifications", true);
			Decimals = EditorPrefs.GetInt("Chroma.Decimals", 3);
			UseCustomCurveEditor = EditorPrefs.GetBool("Chroma.UseCustomCurveEditor", true);
			ColorWheelSensitivity = EditorPrefs.GetFloat("Chroma.ColorWheelSensitivity", 0.5f);
			AutoSwitchToGameView = EditorPrefs.GetBool("Chroma.AutoSwitchToGameView", true);
			VolumeColor = new Color(
				EditorPrefs.GetFloat("Chroma.VolumeColor.Red", 0.15f),
				EditorPrefs.GetFloat("Chroma.VolumeColor.Green", 0.69f),
				EditorPrefs.GetFloat("Chroma.VolumeColor.Blue", 0.93f),
				EditorPrefs.GetFloat("Chroma.VolumeColor.Alpha", 1f)
			);

			ChromaticaVolume.BoundsColor = VolumeColor;
			loaded = true;
		}

		static void Save()
		{
			EditorPrefs.SetBool("Chroma.LockMinWidth", LockMinWidth);
			EditorPrefs.SetBool("Chroma.ShowNotifications", ShowNotifications);
			EditorPrefs.SetInt("Chroma.Decimals", Decimals);
			EditorPrefs.SetBool("Chroma.UseCustomCurveEditor", UseCustomCurveEditor);
			EditorPrefs.SetFloat("Chroma.ColorWheelSensitivity", ColorWheelSensitivity);
			EditorPrefs.SetBool("Chroma.AutoSwitchToGameView", AutoSwitchToGameView);
			EditorPrefs.SetFloat("Chroma.VolumeColor.Red", VolumeColor.r);
			EditorPrefs.SetFloat("Chroma.VolumeColor.Green", VolumeColor.g);
			EditorPrefs.SetFloat("Chroma.VolumeColor.Blue", VolumeColor.b);
			EditorPrefs.SetFloat("Chroma.VolumeColor.Alpha", VolumeColor.a);
		}
	}
}
