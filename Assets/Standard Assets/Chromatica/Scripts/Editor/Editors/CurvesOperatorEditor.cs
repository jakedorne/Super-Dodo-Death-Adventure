using UnityEngine;
using UnityEditor;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class CurvesOperatorEditor : OperatorEditor
	{
		SerializedProperty p_tab;

		SerializedProperty p_redCurve;
		SerializedProperty p_greenCurve;
		SerializedProperty p_blueCurve;

		NativeCurveEditor m_CurveEditor;

		int previousTab = -1;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_tab = target.FindProperty("CurrentTab");
			p_redCurve = target.FindProperty("RedCurve");
			p_greenCurve = target.FindProperty("GreenCurve");
			p_blueCurve = target.FindProperty("BlueCurve");

			Reset();
		}

		public override void Reset()
		{
			target.Update();

			if (Preferences.UseCustomCurveEditor)
				CheckCurveEditor(true);
		}

		public override void DrawContentUI()
		{
			target.Update();

			if (!Preferences.UseCustomCurveEditor || m_CurveEditor == null)
			{
				Rect ranges = new Rect(0f, 0f, 1f, 1f);

				p_redCurve.animationCurveValue = EditorGUILayout.CurveField("Red", p_redCurve.animationCurveValue, Styles.inst.HistogramRedColor, ranges);
				p_greenCurve.animationCurveValue = EditorGUILayout.CurveField("Green", p_greenCurve.animationCurveValue, Styles.inst.HistogramGreenColor, ranges);
				p_blueCurve.animationCurveValue = EditorGUILayout.CurveField("Blue", p_blueCurve.animationCurveValue, Styles.inst.HistogramBlueColor, ranges);
			}
			else
			{
				CheckCurveEditor(false);
				int currentTab = p_tab.intValue;

				EditorGUILayout.BeginHorizontal();
					if (GUILayout.Button("Red Channel", (currentTab == 0) ? Styles.inst.MiniTabLeftActive : Styles.inst.MiniTabLeft)) currentTab = 0;
					if (GUILayout.Button("Green Channel", (currentTab == 1) ? Styles.inst.MiniTabMidActive : Styles.inst.MiniTabMid)) currentTab = 1;
					if (GUILayout.Button("Blue Channel", (currentTab == 2) ? Styles.inst.MiniTabMidActive : Styles.inst.MiniTabMid)) currentTab = 2;
					if (GUILayout.Button("All Channels", (currentTab == 3) ? Styles.inst.MiniTabRightActive : Styles.inst.MiniTabRight)) currentTab = 3;
				EditorGUILayout.EndHorizontal();

				GUILayout.Space(8);

				if (p_tab.intValue == 0) p_redCurve.animationCurveValue = m_CurveEditor.OnGUI(GUILayoutUtility.GetRect(400, 300))[0];
				else if (p_tab.intValue == 1) p_greenCurve.animationCurveValue = m_CurveEditor.OnGUI(GUILayoutUtility.GetRect(400, 300))[0];
				else if (p_tab.intValue == 2) p_blueCurve.animationCurveValue = m_CurveEditor.OnGUI(GUILayoutUtility.GetRect(400, 300))[0];
				else
				{
					AnimationCurve[] curves = m_CurveEditor.OnGUI(GUILayoutUtility.GetRect(400, 300));
					p_redCurve.animationCurveValue = curves[0];
					p_greenCurve.animationCurveValue = curves[1];
					p_blueCurve.animationCurveValue = curves[2];
				}

				p_tab.intValue = currentTab;
			}

			target.ApplyModifiedProperties();
		}

		void CurveField(SerializedProperty property, NativeCurveEditor editor)
		{
			property.animationCurveValue = editor.OnGUI(GUILayoutUtility.GetRect(400, 300))[0];
		}

		void CheckCurveEditor(bool force)
		{
			if (!force && p_tab.intValue == previousTab)
				return;

			try
			{
				switch (p_tab.intValue)
				{
					case 0: m_CurveEditor = new NativeCurveEditor(new AnimationCurve[] { p_redCurve.animationCurveValue }, new Color[] { Styles.inst.HistogramRedColor });
						break;
					case 1: m_CurveEditor = new NativeCurveEditor(new AnimationCurve[] { p_greenCurve.animationCurveValue }, new Color[] { Styles.inst.HistogramGreenColor });
						break;
					case 2: m_CurveEditor = new NativeCurveEditor(new AnimationCurve[] { p_blueCurve.animationCurveValue }, new Color[] { Styles.inst.HistogramBlueColor });
						break;
					case 3: m_CurveEditor = new NativeCurveEditor(new AnimationCurve[] { p_redCurve.animationCurveValue, p_greenCurve.animationCurveValue, p_blueCurve.animationCurveValue }, new Color[] { Styles.inst.HistogramRedColor, Styles.inst.HistogramGreenColor, Styles.inst.HistogramBlueColor });
						break;
					default: m_CurveEditor = null;
						break;
				}
			}
			catch
			{
				m_CurveEditor = null;
			}

			previousTab = p_tab.intValue;
		}
	}
}
