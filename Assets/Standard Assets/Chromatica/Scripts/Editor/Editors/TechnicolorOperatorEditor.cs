using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class TechnicolorOperatorEditor : OperatorEditor
	{
		SerializedProperty p_balance;
		SerializedProperty p_exposure;
		SerializedProperty p_blend;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_balance = target.FindProperty("Balance");
			p_exposure = target.FindProperty("Exposure");
			p_blend = target.FindProperty("Blend");
		}

		public override void DrawContentUI()
		{
			target.Update();

			Color balance = p_balance.colorValue;

			EditorGUILayout.LabelField("Balance", EditorStyles.boldLabel);

			EditorGUI.indentLevel++;
			balance.r = EditorGUILayout.Slider("Red", balance.r, 0f, 1f);
			balance.g = EditorGUILayout.Slider("Green", balance.g, 0f, 1f);
			balance.b = EditorGUILayout.Slider("Blue", balance.b, 0f, 1f);
			EditorGUI.indentLevel--;

			EditorGUILayout.PropertyField(p_exposure);
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(p_blend);

			p_exposure.floatValue = (float)Math.Round(p_exposure.floatValue, Preferences.Decimals);
			balance.r = (float)Math.Round(balance.r, Preferences.Decimals);
			balance.g = (float)Math.Round(balance.g, Preferences.Decimals);
			balance.b = (float)Math.Round(balance.b, Preferences.Decimals);
			p_blend.floatValue = (float)Math.Round(p_blend.floatValue, Preferences.Decimals);

			p_balance.colorValue = balance;

			target.ApplyModifiedProperties();
		}
	}
}
