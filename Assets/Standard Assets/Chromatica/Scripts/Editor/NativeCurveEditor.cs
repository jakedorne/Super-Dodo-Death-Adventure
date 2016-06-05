using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace Chromatica.Studio
{
	public class NativeCurveEditor
	{
		Assembly a_Assembly;
		Type t_NormalCurveRenderer;
		Type t_CurveWrapper;
		Type t_CurveEditor;
		Type t_CurveEditorSettings;

		object o_CurveEditor;

		AnimationCurve[] m_Curves;

		public NativeCurveEditor(AnimationCurve[] curves, Color[] colors)
		{
			m_Curves = (AnimationCurve[])curves.Clone();

			a_Assembly = typeof(EditorWindow).Assembly;
			t_NormalCurveRenderer = a_Assembly.GetType("UnityEditor.NormalCurveRenderer");
			t_CurveWrapper = a_Assembly.GetType("UnityEditor.CurveWrapper");
			t_CurveEditor = a_Assembly.GetType("UnityEditor.CurveEditor");
			t_CurveEditorSettings = a_Assembly.GetType("UnityEditor.CurveEditorSettings");

			object[] o_CurveWrappers = new object[curves.Length];

			for (int i = 0; i < curves.Length; i++)
			{
				// Curve renderer
				object o_NormalCurveRenderer = Activator.CreateInstance(t_NormalCurveRenderer, new object[] { curves[i] });
				InvokeMethod(t_NormalCurveRenderer, o_NormalCurveRenderer, "SetWrap", new Type[] { typeof(WrapMode) }, new object[] { WrapMode.Clamp });

				// Curve Wrapper
				object o_CurveWrapper = Activator.CreateInstance(t_CurveWrapper);
				SetField(t_CurveWrapper, o_CurveWrapper, "id", ("Curve " + i.ToString()).GetHashCode());
				SetField(t_CurveWrapper, o_CurveWrapper, "color", colors[i]);
				SetField(t_CurveWrapper, o_CurveWrapper, "groupId", -1);
				SetField(t_CurveWrapper, o_CurveWrapper, "hidden", false);
				SetField(t_CurveWrapper, o_CurveWrapper, "readOnly", false);
				SetProperty(t_CurveWrapper, o_CurveWrapper, "renderer", o_NormalCurveRenderer);

				o_CurveWrappers[i] = o_CurveWrapper;
			}

			// Curve Wrapper array
			Array wrapperArray = Array.CreateInstance(t_CurveWrapper, o_CurveWrappers.Length);

			for (int i = 0; i < o_CurveWrappers.Length; i++)
				wrapperArray.SetValue(o_CurveWrappers[i], i);

			// Curve Editor Settings
			object o_CurveEditorSettings = Activator.CreateInstance(t_CurveEditorSettings);
			SetProperty(t_CurveEditorSettings, o_CurveEditorSettings, "hRangeMin", 0f);
			SetProperty(t_CurveEditorSettings, o_CurveEditorSettings, "hRangeMax", 1f);
			SetProperty(t_CurveEditorSettings, o_CurveEditorSettings, "vRangeMin", 0f);
			SetProperty(t_CurveEditorSettings, o_CurveEditorSettings, "vRangeMax", 1f);
			SetProperty(t_CurveEditorSettings, o_CurveEditorSettings, "hSlider", false);
			SetProperty(t_CurveEditorSettings, o_CurveEditorSettings, "vSlider", false);
			SetField(t_CurveEditorSettings, o_CurveEditorSettings, "allowDeleteLastKeyInCurve", false);
			SetField(t_CurveEditorSettings, o_CurveEditorSettings, "hTickLabelOffset", 10f);

			// Curve Editor
			o_CurveEditor = Activator.CreateInstance(t_CurveEditor, new object[] { new Rect(0, 0, 400, 400), wrapperArray, true });

			SetProperty(t_CurveEditor, o_CurveEditor, "settings", o_CurveEditorSettings);
			SetField(t_CurveEditor.BaseType.BaseType, o_CurveEditor, "m_HScaleMin", 1f);
			SetField(t_CurveEditor.BaseType.BaseType, o_CurveEditor, "m_HScaleMax", 1f);
			SetField(t_CurveEditor.BaseType.BaseType, o_CurveEditor, "m_VScaleMin", 1f);
			SetField(t_CurveEditor.BaseType.BaseType, o_CurveEditor, "m_VScaleMax", 1f);
			SetField(t_CurveEditor.BaseType.BaseType, o_CurveEditor, "m_MarginBottom", 20f);
			SetField(t_CurveEditor.BaseType.BaseType, o_CurveEditor, "m_MarginTop", 35f);
			SetField(t_CurveEditor.BaseType.BaseType, o_CurveEditor, "m_MarginLeft", 35f);
			SetField(t_CurveEditor.BaseType.BaseType, o_CurveEditor, "m_MarginRight", 25f);

			// Curve Update delegate (doesn't seem to be working ?)
			MethodInfo mi_OnCurveUpdate = GetType().GetMethod("OnCurveUpdate", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo fi_curvesUpdated = t_CurveEditor.GetField("curvesUpdated", BindingFlags.Public | BindingFlags.Instance);
			object dlg_OnCurveUpdate = Delegate.CreateDelegate(fi_curvesUpdated.FieldType, this, mi_OnCurveUpdate);
			fi_curvesUpdated.SetValue(o_CurveEditor, dlg_OnCurveUpdate);

			// Frame [0;1]
			InvokeMethod(t_CurveEditor, o_CurveEditor, "SetShownHRangeInsideMargins", new Type[] { typeof(float), typeof(float) }, new object[] { 0f, 1f });
			InvokeMethod(t_CurveEditor, o_CurveEditor, "SetShownVRangeInsideMargins", new Type[] { typeof(float), typeof(float) }, new object[] { 0f, 1f });

			// Ready
			InvokeMethod(t_CurveEditor, o_CurveEditor, "OnEnable");
		}

		public AnimationCurve[] OnGUI(Rect rect)
		{
			bool flag = Event.current.type == EventType.MouseUp;

			GUI.Box(rect, "", Styles.inst.CurveBackground);

			SetProperty(t_CurveEditor, o_CurveEditor, "rect", rect);
			SetProperty(t_CurveEditor, o_CurveEditor, "hRangeLocked", Event.current.shift);
			SetProperty(t_CurveEditor, o_CurveEditor, "vRangeLocked", EditorGUI.actionKey);

			InvokeMethod(t_CurveEditor, o_CurveEditor, "OnGUI");

			if (Event.current.type == EventType.Used && flag)
				OnCurveUpdate();
			else if (Event.current.type != EventType.Layout && Event.current.type != EventType.Repaint)
				OnCurveUpdate();

			return m_Curves;
		}

		void OnCurveUpdate()
		{
			GUI.changed = true;
		}

		#region Reflection utils
		void SetField(Type type, object instance, string name, object value)
		{
			type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(instance, value);
		}

		void SetProperty(Type type, object instance, string name, object value)
		{
			type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(instance, value, null);
		}

		void InvokeMethod(Type type, object instance, string name)
		{
			type.GetMethod(name).Invoke(instance, null);
		}

		void InvokeMethod(Type type, object instance, string name, Type[] types, object[] parameters)
		{
			type.GetMethod(name, types).Invoke(instance, parameters);
		}
		#endregion
	}
}
