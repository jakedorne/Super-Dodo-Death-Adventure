using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class BleachBypassOperatorEditor : OperatorEditor
	{
		SerializedProperty p_redLuminance;
		SerializedProperty p_greenLuminance;
		SerializedProperty p_blueLuminance;
		SerializedProperty p_blend;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_redLuminance = target.FindProperty("RedLuminance");
			p_greenLuminance = target.FindProperty("GreenLuminance");
			p_blueLuminance = target.FindProperty("BlueLuminance");
			p_blend = target.FindProperty("Blend");
		}

		public override void DrawContentUI()
		{
			target.Update();

			EditorGUILayout.LabelField("Brightness", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(p_redLuminance, new GUIContent("Red"));
			EditorGUILayout.PropertyField(p_greenLuminance, new GUIContent("Green"));
			EditorGUILayout.PropertyField(p_blueLuminance, new GUIContent("Blue"));
			EditorGUI.indentLevel--;
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(p_blend);

			p_redLuminance.floatValue = (float)Math.Round(p_redLuminance.floatValue, Preferences.Decimals);
			p_greenLuminance.floatValue = (float)Math.Round(p_greenLuminance.floatValue, Preferences.Decimals);
			p_blueLuminance.floatValue = (float)Math.Round(p_blueLuminance.floatValue, Preferences.Decimals);
			p_blend.floatValue = (float)Math.Round(p_blend.floatValue, Preferences.Decimals);

			target.ApplyModifiedProperties();
		}
	}
}
