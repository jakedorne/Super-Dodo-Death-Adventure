using UnityEngine;
using UnityEditor;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class ThreeWayOperatorEditor : OperatorEditor
	{
		[SerializeField]
		ColorWheel m_ShadowsWheel;

		[SerializeField]
		ColorWheel m_MidtonesWheel;

		[SerializeField]
		ColorWheel m_HighlightsWheel;

		[SerializeField]
		ColorWheelRenderer m_WheelRenderer;

		SerializedProperty p_shadows;
		SerializedProperty p_midtones;
		SerializedProperty p_highlights;
		SerializedProperty p_blend;
		SerializedProperty p_iscdl;
		SerializedProperty p_showdetails;

		[SerializeField]
		float m_CurrentWidth = 0;

		[SerializeField]
		float m_WheelSize;

		[SerializeField]
		float m_SpaceAfterMidtones;

		[SerializeField]
		float m_SpaceBetweenShadowsAndHighlights;

		[SerializeField]
		float m_SpaceModes;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_shadows = target.FindProperty("Shadows");
			p_midtones = target.FindProperty("Midtones");
			p_highlights = target.FindProperty("Highlights");
			p_blend = target.FindProperty("Blend");
			p_iscdl = target.FindProperty("IsCDL");
			p_showdetails = target.FindProperty("ShowDetails");
		}

		public override void DrawContentUI()
		{
			target.Update();

			float w = ChromaticaWindow.inst.Width;

			if (w != m_CurrentWidth)
			{
				float minW = ChromaticaWindow.inst.minSize.x;
				float factor = 1f;

				if (w > minW)
					factor = Mathf.Min(w, 620f) / minW;

				m_WheelSize = 154f * factor;
				m_SpaceAfterMidtones = -150f * factor;
				m_SpaceBetweenShadowsAndHighlights = 126f * factor;
				m_SpaceModes = -60f;

				m_WheelRenderer = new ColorWheelRenderer((int)m_WheelSize);

				m_ShadowsWheel = new ColorWheel(m_WheelRenderer);
				m_MidtonesWheel = new ColorWheel(m_WheelRenderer);
				m_HighlightsWheel = new ColorWheel(m_WheelRenderer);

				m_CurrentWidth = w;
			}

			Vector4 shadows = p_shadows.vector4Value;
			Vector4 midtones = p_midtones.vector4Value;
			Vector4 highlights = p_highlights.vector4Value;

			GUILayout.Space(-4);

			EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				midtones = m_MidtonesWheel.Draw("Midtones", midtones);
				GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(m_SpaceAfterMidtones);

			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				GUILayout.FlexibleSpace();
				EditorGUILayout.BeginHorizontal();
					shadows = m_ShadowsWheel.Draw("Shadows", shadows);
					GUILayout.Space(m_SpaceBetweenShadowsAndHighlights);
					highlights = m_HighlightsWheel.Draw("Highlights", highlights);
				EditorGUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(m_SpaceModes);

			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				GUILayout.FlexibleSpace();
				GUILayout.Label("Mode");
				GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				GUILayout.FlexibleSpace();
				p_iscdl.boolValue = !GUILayout.Toggle(!p_iscdl.boolValue, "LGG", EditorStyles.miniButtonLeft, GUILayout.Width(50));
				p_iscdl.boolValue = GUILayout.Toggle(p_iscdl.boolValue, "CDL", EditorStyles.miniButtonRight, GUILayout.Width(50));
				GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(5);

			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				GUILayout.FlexibleSpace();
				p_showdetails.boolValue = GUILayout.Toggle(p_showdetails.boolValue, "Show Details", EditorStyles.miniButton, GUILayout.Width(100));
				GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(1);

			if (p_showdetails.boolValue)
			{
				GUILayout.Space(6);

				EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
					GUILayout.FlexibleSpace();
					EditorGUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();

						GUILayout.BeginVertical();
							GUILayout.Label(string.Format("R: {0:F3}", shadows.x * shadows.w));
							GUILayout.Label(string.Format("G: {0:F3}", shadows.y * shadows.w));
							GUILayout.Label(string.Format("B: {0:F3}", shadows.z * shadows.w));
						GUILayout.EndVertical();

						GUILayout.FlexibleSpace();

						GUILayout.BeginVertical();
							float multiplier = 1f + (1f - (midtones.x * 0.299f + midtones.y * 0.587f + midtones.z * 0.114f));
							GUILayout.Label(string.Format("R: {0:F3}", (midtones.x * multiplier) * midtones.w));
							GUILayout.Label(string.Format("G: {0:F3}", (midtones.y * multiplier) * midtones.w));
							GUILayout.Label(string.Format("B: {0:F3}", (midtones.z * multiplier) * midtones.w));
						GUILayout.EndVertical();

						GUILayout.FlexibleSpace();

						GUILayout.BeginVertical();
							multiplier = 1f + (1f - (highlights.x * 0.299f + highlights.y * 0.587f + highlights.z * 0.114f));
							GUILayout.Label(string.Format("R: {0:F3}", (highlights.x * multiplier) * highlights.w));
							GUILayout.Label(string.Format("G: {0:F3}", (highlights.y * multiplier) * highlights.w));
							GUILayout.Label(string.Format("B: {0:F3}", (highlights.z * multiplier) * highlights.w));
						GUILayout.EndVertical();

						GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.PropertyField(p_blend);

			p_shadows.vector4Value = shadows;
			p_midtones.vector4Value = midtones;
			p_highlights.vector4Value = highlights;

			target.ApplyModifiedProperties();
		}
	}
}
