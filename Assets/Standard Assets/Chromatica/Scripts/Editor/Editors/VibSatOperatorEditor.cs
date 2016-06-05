using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class VibSatOperatorEditor : OperatorEditor
	{
		SerializedProperty p_vibrance;
		SerializedProperty p_saturation;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_vibrance = target.FindProperty("Vibrance");
			p_saturation = target.FindProperty("Saturation");
		}

		public override void DrawContentUI()
		{
			target.Update();

			Vector4 vibrance = p_vibrance.vector4Value;

			vibrance.w = EditorGUILayout.Slider("Vibrance", vibrance.w, -100f, 100f);
			EditorGUI.indentLevel++;
			vibrance.x = EditorGUILayout.Slider("Red", vibrance.x, -5f, 5f);
			vibrance.y = EditorGUILayout.Slider("Green", vibrance.y, -5f, 5f);
			vibrance.z = EditorGUILayout.Slider("Blue", vibrance.z, -5f, 5f);
			EditorGUI.indentLevel--;

			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(p_saturation);

			vibrance.w = (float)Math.Round(vibrance.w, Preferences.Decimals);
			vibrance.x = (float)Math.Round(vibrance.x, Preferences.Decimals);
			vibrance.y = (float)Math.Round(vibrance.y, Preferences.Decimals);
			vibrance.z = (float)Math.Round(vibrance.z, Preferences.Decimals);
			p_saturation.floatValue = (float)Math.Round(p_saturation.floatValue, Preferences.Decimals);

			p_vibrance.vector4Value = vibrance;

			target.ApplyModifiedProperties();
		}
	}
}
