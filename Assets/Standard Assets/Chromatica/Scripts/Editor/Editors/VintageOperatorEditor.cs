using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class VintageOperatorEditor : OperatorEditor
	{
		internal static string[] filters = {
			"None",
			"F1977",
			"Aden",
			"Amaro",
			"Brannan",
			"Crema",
			"Earlybird",
			"Hefe",
			"Hudson",
			"Inkwell",
			"Juno",
			"Kelvin",
			"Lark",
			"LoFi",
			"Ludwig",
			"Mayfair",
			"Nashville",
			"Perpetua",
			"Reyes",
			"Rise",
			"Sierra",
			"Slumber",
			"Sutro",
			"Toaster",
			"Valencia",
			"Walden",
			"Willow",
			"XProII"
		};

		SerializedProperty p_blend;
		SerializedProperty p_filter;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_blend = target.FindProperty("Blend");
			p_filter = target.FindProperty("Filter");
		}

		public override void DrawContentUI()
		{
			target.Update();

			p_filter.intValue = EditorGUILayout.Popup("Filter", p_filter.intValue, filters);
			EditorGUILayout.PropertyField(p_blend);

			p_blend.floatValue = (float)Math.Round(p_blend.floatValue, Preferences.Decimals);

			target.ApplyModifiedProperties();
		}
	}
}
