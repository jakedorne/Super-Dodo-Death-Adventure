using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class ChannelClamperOperatorEditor : OperatorEditor
	{
		SerializedProperty p_red;
		SerializedProperty p_green;
		SerializedProperty p_blue;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_red = target.FindProperty("Red");
			p_green = target.FindProperty("Green");
			p_blue = target.FindProperty("Blue");
		}

		public override void DrawContentUI()
		{
			target.Update();

			Vector2 red = p_red.vector2Value;
			Vector2 green = p_green.vector2Value;
			Vector2 blue = p_blue.vector2Value;

			EditorGUILayout.LabelField("Red Channel", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			EditorGUILayout.MinMaxSlider(ref red.x, ref red.y, 0f, 1f);
			EditorGUILayout.BeginHorizontal();
			red.x = EditorGUILayout.FloatField(red.x, GUILayout.Width(80));
			GUILayout.FlexibleSpace();
			red.y = EditorGUILayout.FloatField(red.y, GUILayout.Width(80));
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel--;

			EditorGUILayout.LabelField("Green Channel", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			EditorGUILayout.MinMaxSlider(ref green.x, ref green.y, 0f, 1f);
			EditorGUILayout.BeginHorizontal();
			green.x = EditorGUILayout.FloatField(green.x, GUILayout.Width(80));
			GUILayout.FlexibleSpace();
			green.y = EditorGUILayout.FloatField(green.y, GUILayout.Width(80));
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel--;

			EditorGUILayout.LabelField("Blue Channel", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			EditorGUILayout.MinMaxSlider(ref blue.x, ref blue.y, 0f, 1f);
			EditorGUILayout.BeginHorizontal();
			blue.x = EditorGUILayout.FloatField(blue.x, GUILayout.Width(80));
			GUILayout.FlexibleSpace();
			blue.y = EditorGUILayout.FloatField(blue.y, GUILayout.Width(80));
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel--;

			red.x = (float)Math.Round(red.x, Preferences.Decimals);
			red.y = (float)Math.Round(red.y, Preferences.Decimals);
			green.x = (float)Math.Round(green.x, Preferences.Decimals);
			green.y = (float)Math.Round(green.y, Preferences.Decimals);
			blue.x = (float)Math.Round(blue.x, Preferences.Decimals);
			blue.y = (float)Math.Round(blue.y, Preferences.Decimals);

			p_red.vector2Value = red;
			p_green.vector2Value = green;
			p_blue.vector2Value = blue;

			target.ApplyModifiedProperties();
		}
	}
}
