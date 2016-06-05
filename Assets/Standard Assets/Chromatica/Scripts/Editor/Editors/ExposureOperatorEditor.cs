using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class ExposureOperatorEditor : OperatorEditor
	{
		SerializedProperty p_exposure;
		SerializedProperty p_offset;
		SerializedProperty p_gamma;
		SerializedProperty p_brightness;
		SerializedProperty p_contrast;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_exposure = target.FindProperty("Exposure");
			p_offset = target.FindProperty("Offset");
			p_gamma = target.FindProperty("Gamma");
			p_brightness = target.FindProperty("Brightness");
			p_contrast = target.FindProperty("Contrast");
		}

		public override void DrawContentUI()
		{
			target.Update();

			Vector4 contrast = p_contrast.vector4Value;

			EditorGUILayout.PropertyField(p_exposure);
			EditorGUILayout.PropertyField(p_offset);
			EditorGUILayout.PropertyField(p_gamma);
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(p_brightness);
			contrast.w = EditorGUILayout.Slider("Contrast", contrast.w, -100f, 100f);

			EditorGUI.indentLevel++;
			contrast.x = EditorGUILayout.Slider("Red", contrast.x, 0.0f, 1.0f);
			contrast.y = EditorGUILayout.Slider("Green", contrast.y, 0.0f, 1.0f);
			contrast.z = EditorGUILayout.Slider("Blue", contrast.z, 0.0f, 1.0f);
			EditorGUI.indentLevel--;

			p_exposure.floatValue = (float)Math.Round(p_exposure.floatValue, Preferences.Decimals);
			p_offset.floatValue = (float)Math.Round(p_offset.floatValue, Preferences.Decimals);
			p_gamma.floatValue = (float)Math.Round(p_gamma.floatValue, Preferences.Decimals);
			contrast.w = (float)Math.Round(contrast.w, Preferences.Decimals);
			contrast.x = (float)Math.Round(contrast.x, Preferences.Decimals);
			contrast.y = (float)Math.Round(contrast.y, Preferences.Decimals);
			contrast.z = (float)Math.Round(contrast.z, Preferences.Decimals);

			p_contrast.vector4Value = contrast;

			target.ApplyModifiedProperties();
		}
	}
}
