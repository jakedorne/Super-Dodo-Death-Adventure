using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class ChannelMixerOperatorEditor : OperatorEditor
	{
		SerializedProperty p_tab;
		SerializedProperty p_red;
		SerializedProperty p_green;
		SerializedProperty p_blue;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_tab = target.FindProperty("CurrentTab");
			p_red = target.FindProperty("Red");
			p_green = target.FindProperty("Green");
			p_blue = target.FindProperty("Blue");
		}

		public override void DrawContentUI()
		{
			target.Update();

			int currentTab = p_tab.intValue;

			EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Red Channel", (currentTab == 0) ? Styles.inst.MiniTabLeftActive : Styles.inst.MiniTabLeft)) currentTab = 0;
				if (GUILayout.Button("Green Channel", (currentTab == 1) ? Styles.inst.MiniTabMidActive : Styles.inst.MiniTabMid)) currentTab = 1;
				if (GUILayout.Button("Blue Channel", (currentTab == 2) ? Styles.inst.MiniTabRightActive : Styles.inst.MiniTabRight)) currentTab = 2;
			EditorGUILayout.EndHorizontal();

			if (currentTab == 0) ChannelUI(p_red);
			else if (currentTab == 1) ChannelUI(p_green);
			else if (currentTab == 2) ChannelUI(p_blue);

			p_tab.intValue = currentTab;

			target.ApplyModifiedProperties();
		}

		void ChannelUI(SerializedProperty color)
		{
			Vector4 v = color.vector4Value;

			v.x = EditorGUILayout.Slider("% Red", v.x, -200f, 200f);
			v.y = EditorGUILayout.Slider("% Green", v.y, -200f, 200f);
			v.z = EditorGUILayout.Slider("% Blue", v.z, -200f, 200f);
			v.w = EditorGUILayout.Slider("Constant", v.w, -200f, 200f);

			v.x = (float)Math.Round(v.x, Preferences.Decimals);
			v.y = (float)Math.Round(v.y, Preferences.Decimals);
			v.z = (float)Math.Round(v.z, Preferences.Decimals);
			v.w = (float)Math.Round(v.w, Preferences.Decimals);

			color.vector4Value = v;
		}
	}
}
