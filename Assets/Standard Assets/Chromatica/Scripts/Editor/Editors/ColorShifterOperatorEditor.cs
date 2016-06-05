using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class ColorShifterOperatorEditor : OperatorEditor
	{
		static string[] channels = { "Master", "Reds", "Yellows", "Greens", "Cyans", "Blues", "Magentas" };

		SerializedProperty p_advanced;
		SerializedProperty p_channel;
		SerializedProperty p_hue;
		SerializedProperty p_saturation;
		SerializedProperty p_value;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_advanced = target.FindProperty("Advanced");
			p_channel = target.FindProperty("Channel");
			p_hue = target.FindProperty("Hue");
			p_saturation = target.FindProperty("Saturation");
			p_value = target.FindProperty("Value");
		}

		public override void DrawContentUI()
		{
			target.Update();

			bool advanced = p_advanced.boolValue;
			int channel = p_channel.intValue;

			EditorGUILayout.BeginHorizontal();
				if (advanced) channel = EditorGUILayout.Popup(channel, channels);
				else channel = 0;

				advanced = GUILayout.Toggle(advanced, "Advanced Mode", EditorStyles.miniButton);
			EditorGUILayout.EndHorizontal();

			SerializedProperty p_hue2 = p_hue.GetArrayElementAtIndex(channel);
			SerializedProperty p_saturation2 = p_saturation.GetArrayElementAtIndex(channel);
			SerializedProperty p_value2 = p_value.GetArrayElementAtIndex(channel);

			p_hue2.floatValue = EditorGUILayout.Slider("Hue", p_hue2.floatValue, -180f, 180f);
			p_saturation2.floatValue = EditorGUILayout.Slider("Saturation", p_saturation2.floatValue, -100f, 100f);
			p_value2.floatValue = EditorGUILayout.Slider("Value", p_value2.floatValue, -100f, 100f);

			p_hue2.floatValue = (float)Math.Round(p_hue2.floatValue, Preferences.Decimals);
			p_saturation2.floatValue = (float)Math.Round(p_saturation2.floatValue, Preferences.Decimals);
			p_value2.floatValue = (float)Math.Round(p_value2.floatValue, Preferences.Decimals);

			p_advanced.boolValue = advanced;
			p_channel.intValue = channel;

			target.ApplyModifiedProperties();
		}
	}
}
