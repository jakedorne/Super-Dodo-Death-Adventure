using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class HueFocusOperatorEditor : OperatorEditor
	{
		SerializedProperty p_hue;
		SerializedProperty p_range;
		SerializedProperty p_boost;
		SerializedProperty p_blend;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_hue = target.FindProperty("Hue");
			p_range = target.FindProperty("Range");
			p_boost = target.FindProperty("Boost");
			p_blend = target.FindProperty("Blend");
		}

		public override void DrawContentUI()
		{
			target.Update();

			Rect rect = GUILayoutUtility.GetRect(0, 20);
			GUI.DrawTextureWithTexCoords(rect, Styles.inst.HueRampTexture, new Rect(0.5f + p_hue.floatValue / 360f, 0f, 1f, 1f));

			GUI.enabled = false;
			float min = 180f - p_range.floatValue;
			float max = 180f + p_range.floatValue;
			EditorGUILayout.MinMaxSlider(ref min, ref max, 0f, 360f);
			GUI.enabled = true;

			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(p_hue);
			EditorGUILayout.PropertyField(p_range);
			EditorGUILayout.PropertyField(p_boost);
			EditorGUILayout.PropertyField(p_blend);

			p_hue.floatValue = (float)Math.Round(p_hue.floatValue, Preferences.Decimals);
			p_range.floatValue = (float)Math.Round(p_range.floatValue, Preferences.Decimals);
			p_boost.floatValue = (float)Math.Round(p_boost.floatValue, Preferences.Decimals);
			p_blend.floatValue = (float)Math.Round(p_blend.floatValue, Preferences.Decimals);

			target.ApplyModifiedProperties();
		}
	}
}
