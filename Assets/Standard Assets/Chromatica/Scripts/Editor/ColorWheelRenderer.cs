using UnityEngine;
using UnityEditor;
using System;

namespace Chromatica.Studio
{
	[Serializable]
	public class ColorWheelRenderer
	{
		public int Size { get; private set; }
		public int HalfSize { get; private set; }

		public Texture2D Texture
		{
			get
			{
				if (m_Texture == null)
					Fill(Enabled ? 1f : 0.5f);

				return m_Texture;
			}
		}

		public bool Enabled
		{
			get
			{
				return m_Enabled;
			}
			set
			{
				if (m_Enabled == value)
					return;

				m_Enabled = value;
				Fill(m_Enabled ? 1f : 0.5f);
			}
		}

		[SerializeField]
		Texture2D m_Texture;

		[SerializeField]
		bool m_Enabled;

		public ColorWheelRenderer(int size)
		{
			Size = size;
			HalfSize = size / 2;
			m_Enabled = true;
		}

		void Fill(float alphaMultiplier)
		{
			if (m_Texture == null)
			{
				Texture2D texture = new Texture2D(Size, Size, TextureFormat.ARGB32, false, true);
				texture.filterMode = FilterMode.Point;
				texture.wrapMode = TextureWrapMode.Clamp;
				texture.hideFlags = HideFlags.DontSave;
				texture.alphaIsTransparency = true;
				m_Texture = texture;
			}

			Color[] pixels = m_Texture.GetPixels();

			for (int y = 0; y < Size; y++)
			{
				for (int x = 0; x < Size; x++)
				{
					int index = y * Size + x;
					float hue = 0f, saturation = 0f;
					float dx = (float)(x - HalfSize) / HalfSize;
					float dy = (float)(y - HalfSize) / HalfSize;
					float d = Mathf.Sqrt(dx * dx + dy * dy);

					// Out of the wheel, early exit
					if (d >= 1)
					{
						pixels[index] = new Color(0f, 0f, 0f, 0f);
						continue;
					}

					saturation = d;
					hue = Mathf.Acos(dx / d) / 3.14159265359f / 2f;

					if (dy > 0)
						hue = 1f - hue;

					Color color = Color.HSVToRGB(hue, saturation, 1f);
					color.a = (saturation > 0.99) ? (1f - saturation) * 100f : 1f; // Antialiasing
					color.a *= alphaMultiplier;

					pixels[index] = color;
				}
			}

			m_Texture.SetPixels(pixels);
			m_Texture.Apply(false);
		}
	}
}
