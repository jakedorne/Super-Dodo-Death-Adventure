using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class WhiteBalanceOperatorEditor : OperatorEditor
	{
		static string[] modes = { "Simple", "Complex" };

		SerializedProperty p_white;
		SerializedProperty p_mode;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_white = target.FindProperty("White");
			p_mode = target.FindProperty("Mode");
		}

		public override void DrawContentUI()
		{
			target.Update();

			p_mode.intValue = EditorGUILayout.Popup("Mode", p_mode.intValue, modes);
			EditorGUILayout.PropertyField(p_white, new GUIContent("White Point"));

			target.ApplyModifiedProperties();
		}
	}
}
