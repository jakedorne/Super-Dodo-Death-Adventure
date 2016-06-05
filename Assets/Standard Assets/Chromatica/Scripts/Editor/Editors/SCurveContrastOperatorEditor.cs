using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class SCurveContrastOperatorEditor : OperatorEditor
	{
		SerializedProperty p_redSteepness;
		SerializedProperty p_redGamma;
		SerializedProperty p_greenSteepness;
		SerializedProperty p_greenGamma;
		SerializedProperty p_blueSteepness;
		SerializedProperty p_blueGamma;
		SerializedProperty p_drawCurve;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_redSteepness = target.FindProperty("RedSteepness");
			p_redGamma = target.FindProperty("RedGamma");
			p_greenSteepness = target.FindProperty("GreenSteepness");
			p_greenGamma = target.FindProperty("GreenGamma");
			p_blueSteepness = target.FindProperty("BlueSteepness");
			p_blueGamma = target.FindProperty("BlueGamma");
			p_drawCurve = target.FindProperty("DrawCurve");
		}

		public override void DrawContentUI()
		{
			target.Update();

			EditorGUILayout.LabelField(new GUIContent("Red"), EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_redSteepness, new GUIContent("Steepness"));
				EditorGUILayout.PropertyField(p_redGamma, new GUIContent("Gamma"));
			}
			EditorGUI.indentLevel--;
			
			EditorGUILayout.LabelField(new GUIContent("Green"), EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_greenSteepness, new GUIContent("Steepness"));
				EditorGUILayout.PropertyField(p_greenGamma, new GUIContent("Gamma"));
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.LabelField(new GUIContent("Blue"), EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_blueSteepness, new GUIContent("Steepness"));
				EditorGUILayout.PropertyField(p_blueGamma, new GUIContent("Gamma"));
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(p_drawCurve, new GUIContent("Show Curves"));

			if (p_drawCurve.boolValue)
				DrawCurve();

			p_redSteepness.floatValue = (float)Math.Round(p_redSteepness.floatValue, Preferences.Decimals);
			p_redGamma.floatValue = (float)Math.Round(p_redGamma.floatValue, Preferences.Decimals);
			p_greenSteepness.floatValue = (float)Math.Round(p_greenSteepness.floatValue, Preferences.Decimals);
			p_greenGamma.floatValue = (float)Math.Round(p_greenGamma.floatValue, Preferences.Decimals);
			p_blueSteepness.floatValue = (float)Math.Round(p_blueSteepness.floatValue, Preferences.Decimals);
			p_blueGamma.floatValue = (float)Math.Round(p_blueGamma.floatValue, Preferences.Decimals);

			target.ApplyModifiedProperties();
		}

		void DrawCurve()
		{
			int h = 200;
			int h_1 = h - 1;
			Rect r = GUILayoutUtility.GetRect(256f, h);
			GUI.Box(r, GUIContent.none);

			float rs = p_redSteepness.floatValue;
			float rg = p_redGamma.floatValue;
			float gs = p_greenSteepness.floatValue;
			float gg = p_greenGamma.floatValue;
			float bs = p_blueSteepness.floatValue;
			float bg = p_blueGamma.floatValue;

			int w = Mathf.FloorToInt(r.width);
			Vector3[] red = new Vector3[w];
			Vector3[] green = new Vector3[w];
			Vector3[] blue = new Vector3[w];

			for (int i = 0; i < w; i++)
			{
				float v = (float)i / (w - 1);
				red[i] = new Vector3(r.x + i, r.y + (h - curve(v, rs, rg) * h_1), 0f);
				green[i] = new Vector3(r.x + i, r.y + (h - curve(v, gs, gg) * h_1), 0f);
				blue[i] = new Vector3(r.x + i, r.y + (h - curve(v, bs, bg) * h_1), 0f);
			}

			Handles.color = EditorGUIUtility.isProSkin ? new Color(0f, 1f, 1f, 2f) : new Color(0f, 0f, 1f, 2f);
			Handles.DrawAAPolyLine(1f, blue);
			Handles.color = EditorGUIUtility.isProSkin ? new Color(0f, 1f, 0f, 2f) : new Color(0.2f, 0.8f, 0.2f, 2f);
			Handles.DrawAAPolyLine(1f, green);
			Handles.color = new Color(1f, 0f, 0f, 2f);
			Handles.DrawAAPolyLine(1f, red);
			Handles.color = Color.white;
		}

		float curve(float o, float steepness, float gamma)
		{
			float g = Mathf.Pow(2f, steepness) * 0.5f;
			float c = (o < 0.5f) ? Mathf.Pow(o, steepness) * g : 1f - Mathf.Pow(1f - o, steepness) * g;
			return Mathf.Clamp01(Mathf.Pow(c, gamma));
		}
	}
}
