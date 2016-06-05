using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class ChannelSwapperOperatorEditor : OperatorEditor
	{
		static string[] channels = { "Red Channel", "Green Channel", "Blue Channel" };

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

			p_red.intValue = EditorGUILayout.Popup("Red Source", p_red.intValue, channels);
			p_green.intValue = EditorGUILayout.Popup("Green Source", p_green.intValue, channels);
			p_blue.intValue = EditorGUILayout.Popup("Blue Source", p_blue.intValue, channels);

			target.ApplyModifiedProperties();
		}
	}
}
