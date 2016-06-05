using UnityEngine;
using UnityEditor;
using System;

namespace Chromatica.Studio
{
	[Serializable]
	public class ColorWheel
	{
		public static ColorWheel Dragging = null;

		[SerializeField]
		bool m_WasEnabled = true;

		[SerializeField]
		Vector2 m_Cursor = Vector2.zero;

		[SerializeField]
		ColorWheelRenderer m_Renderer;

		public ColorWheel(ColorWheelRenderer renderer)
		{
			m_Renderer = renderer;
		}

		public Vector4 Draw(string title, Vector4 values)
		{
			if (m_WasEnabled != GUI.enabled)
			{
				m_WasEnabled = GUI.enabled;
				m_Renderer.Enabled = m_WasEnabled;
			}

			Vector4 color = new Vector4();
			Color.RGBToHSV(values, out color.x, out color.y, out color.z);

			EditorGUILayout.BeginVertical();

				// Title
				EditorGUILayout.BeginHorizontal(GUILayout.Width(m_Renderer.Size));
					GUILayout.FlexibleSpace();
					GUILayout.Label(title, EditorStyles.boldLabel);
					GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();

				// Wheel
				EditorGUILayout.BeginHorizontal(GUILayout.Width(m_Renderer.Size));
					GUILayout.FlexibleSpace();
					Rect wheelRect = GUILayoutUtility.GetRect(m_Renderer.Size, m_Renderer.Size);
					GUI.DrawTexture(new Rect(wheelRect.x, wheelRect.y, m_Renderer.Size, m_Renderer.Size), m_Renderer.Texture);
					GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();

				color = GetInput(wheelRect, color);
				color.w = values.w;

				// Wheel thumb
				Vector2 thumbPos = Vector2.zero;
				Vector2 thumbBorder = Vector2.zero;
				float theta = color.x * 6.28318530718f;
				float len = color.y * m_Renderer.HalfSize;
				thumbPos.x = Mathf.Cos(theta);
				thumbPos.y = Mathf.Sin(theta);
				thumbBorder = thumbPos * (len - 4);
				thumbPos *= len;
				GUI.Box(new Rect(wheelRect.x + m_Renderer.HalfSize + thumbPos.x - 4, wheelRect.y + m_Renderer.HalfSize + thumbPos.y - 4, 8, 8), "", Styles.inst.WheelThumb);

				Handles.color = new Color(0f, 0f, 0f, 0.75f);
				Handles.DrawAAPolyLine(new Vector2(wheelRect.x + m_Renderer.HalfSize, wheelRect.y + m_Renderer.HalfSize), new Vector2(wheelRect.x + m_Renderer.HalfSize + thumbBorder.x, wheelRect.y + m_Renderer.HalfSize + thumbBorder.y));

				GUILayout.Space(4);

				// Slider
				EditorGUILayout.BeginHorizontal(GUILayout.Width(m_Renderer.Size));
					GUILayout.FlexibleSpace();
					color.w = GUILayout.HorizontalSlider(color.w, 0f, 2f, GUILayout.Width(110));
					GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();

				// Slider float field & Reset button
				EditorGUILayout.BeginHorizontal(GUILayout.Width(m_Renderer.Size));
					GUILayout.FlexibleSpace();
					color.w = EditorGUILayout.FloatField(color.w, GUILayout.Width(40));

					if (GUILayout.Button("Reset", EditorStyles.miniButton))
						color.Set(0f, 0f, 1f, 1f);

					GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();

				color.w = Mathf.Clamp((float)Math.Round(color.w, Preferences.Decimals), 0f, 2f);

			EditorGUILayout.EndVertical();

			Vector4 newColor = Color.HSVToRGB(color.x, color.y, color.z);
			newColor.w = color.w;
			return newColor;
		}

		Vector3 GetInput(Rect rect, Vector4 color)
		{
			Event e = Event.current;

			if (e.type == EventType.MouseDown && e.button == 0)
			{
				Vector2 mousePos = e.mousePosition;

				if (rect.Contains(mousePos))
				{
					Vector2 center = new Vector2(rect.x + m_Renderer.HalfSize, rect.y + m_Renderer.HalfSize);
					float dist = Vector2.Distance(center, mousePos);

					if (dist <= m_Renderer.HalfSize)
					{
						e.Use();

						Vector2 relativePos = mousePos - new Vector2(rect.x, rect.y);
						m_Cursor = relativePos;
						GetWheelHueSaturation((int)relativePos.x, (int)relativePos.y, ref color.x, ref color.y);
						Dragging = this;
						GUI.changed = true;
					}
				}
			}
			else if (Dragging == this && e.type == EventType.MouseDrag && e.button == 0)
			{
				e.Use();

				m_Cursor += e.delta * Preferences.ColorWheelSensitivity;
				GetWheelHueSaturation((int)Mathf.Clamp(m_Cursor.x, 0f, m_Renderer.Size), (int)Mathf.Clamp(m_Cursor.y, 0f, m_Renderer.Size), ref color.x, ref color.y);
				GUI.changed = true;
			}
			else if (Dragging == this && e.type == EventType.MouseUp && e.button == 0)
			{
				e.Use();
				Dragging = null;
				GUI.changed = true;
			}

			return color;
		}

		void GetWheelHueSaturation(int x, int y, ref float hue, ref float saturation)
		{
			float dx = (float)(x - m_Renderer.HalfSize) / m_Renderer.HalfSize;
			float dy = (float)(y - m_Renderer.HalfSize) / m_Renderer.HalfSize;
			float d = Mathf.Sqrt((dx * dx + dy * dy));
			saturation = d;
			hue = Mathf.Acos(dx / d) / 3.14159265359f / 2f;

			if (dy < 0)
				hue = 1f - hue;
		}
	}
}
