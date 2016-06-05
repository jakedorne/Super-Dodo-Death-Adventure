using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class InvertOperatorEditor : OperatorEditor
	{
		SerializedProperty p_blend;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_blend = target.FindProperty("Blend");
		}

		public override void DrawContentUI()
		{
			target.Update();

			EditorGUILayout.PropertyField(p_blend);

			p_blend.floatValue = (float)Math.Round(p_blend.floatValue, Preferences.Decimals);

			target.ApplyModifiedProperties();
		}
	}
}
