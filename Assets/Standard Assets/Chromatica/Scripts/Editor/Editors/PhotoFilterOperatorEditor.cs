using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class PhotoFilterOperatorEditor : OperatorEditor
	{
		static string[] presets = { "Warming Filter (85)", "Warming Filter (LBA)", "Warming Filter (81)",
								"Cooling Filter (80)", "Cooling Filter (LBB)", "Cooling Filter (82)",
								"Red", "Orange", "Yellow", "Green", "Cyan", "Blue", "Violet", "Magenta",
								"Sepia", "Deep Red", "Deep Blue", "Deep Emerald", "Deep Yellow", "Underwater" };

		static float[,] presetsData = { { 0.925f, 0.541f, 0.0f }, { 0.98f, 0.541f, 0.0f }, { 0.922f, 0.694f, 0.075f },
									 { 0.0f, 0.427f, 1.0f }, { 0.0f, 0.365f, 1.0f }, { 0.0f, 0.71f, 1.0f },
									 { 0.918f, 0.102f, 0.102f }, { 0.956f, 0.518f, 0.09f }, { 0.976f, 0.89f, 0.11f },
									 { 0.098f, 0.788f, 0.098f }, { 0.114f, 0.796f, 0.918f }, { 0.114f, 0.209f, 0.918f },
									 { 0.608f, 0.114f, 0.918f }, { 0.89f, 0.094f, 0.89f }, { 0.675f, 0.478f, 0.2f },
									 { 1.0f, 0.0f, 0.0f }, { 0.0f, 0.133f, 0.804f }, { 0.0f, 0.553f, 0.0f },
									 { 1.0f, 0.835f, 0.0f }, { 0.0f, 0.761f, 0.694f } };

		SerializedProperty p_preset;
		SerializedProperty p_filter;
		SerializedProperty p_density;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_preset = target.FindProperty("Preset");
			p_filter = target.FindProperty("Filter");
			p_density = target.FindProperty("Density");
		}

		public override void DrawContentUI()
		{
			target.Update();

			EditorGUI.BeginChangeCheck();
			p_preset.intValue = EditorGUILayout.Popup("Preset", p_preset.intValue, presets);
			if (EditorGUI.EndChangeCheck())
			{
				p_filter.colorValue = new Color(
						presetsData[p_preset.intValue, 0],
						presetsData[p_preset.intValue, 1],
						presetsData[p_preset.intValue, 2]
					);
			}

			EditorGUILayout.PropertyField(p_filter);
			EditorGUILayout.PropertyField(p_density);

			p_density.floatValue = (float)Math.Round(p_density.floatValue, Preferences.Decimals);

			target.ApplyModifiedProperties();
		}
	}
}
