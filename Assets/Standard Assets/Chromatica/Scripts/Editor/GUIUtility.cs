using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace Chromatica.Studio
{
	public static class GUIUtility
	{
		static MethodInfo m_GradientFieldMethod;
		static Type m_GameViewType;

		static GUIUtility()
		{
			// Gradient
			Type editorGUILayout = typeof(EditorGUILayout);
			m_GradientFieldMethod = editorGUILayout.GetMethod("GradientField", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(string), typeof(Gradient), typeof(GUILayoutOption[]) }, null);

			// GameView
			Assembly assembly = typeof(EditorWindow).Assembly;
			m_GameViewType = assembly.GetType("UnityEditor.GameView");
		}

		public static Gradient GradientField(string label, Gradient gradient, params GUILayoutOption[] options)
		{
			return (Gradient)m_GradientFieldMethod.Invoke(null, new object[] { label, gradient, options }); ;
		}

		public static bool ClickedOn(Rect area, int button = 0)
		{
			Event e = Event.current;

			if (e.type == EventType.MouseDown && area.Contains(e.mousePosition) && e.isMouse && e.button == button)
			{
				e.Use();
				return true;
			}

			return false;
		}

		public static GUIContent GetTitleContent(EditorWindow editor)
		{
			const BindingFlags bFlags = BindingFlags.Instance | BindingFlags.NonPublic;
			PropertyInfo p = typeof(EditorWindow).GetProperty("cachedTitleContent", bFlags);
			if (p == null) return null;
			return p.GetValue(editor, null) as GUIContent;
		}

		public static void RepaintGameView()
		{
			if (!Preferences.AutoSwitchToGameView)
				return;

			EditorWindow gameview = EditorWindow.GetWindow(m_GameViewType);
			gameview.Repaint();
		}
	}
}
