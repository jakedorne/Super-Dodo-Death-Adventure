using UnityEngine;
using System;
using Chromatica.Operators;
using Chromatica.Utils;

namespace Chromatica.Studio.Renderers
{
	public class LevelsOperatorRenderer : OperatorRenderer
	{
		Texture2D m_HistogramTexture;
		Vector2 m_LastHistogramTextureSize;

		public override int SetParameters(Operator op)
		{
			LevelsOperator o = (LevelsOperator)op;

			if (!o.IsRGB)
			{
				Material.SetVector("_InputMin", new Vector4(o.InputMinL / 255f, o.InputMinL / 255f, o.InputMinL / 255f, 1f));
				Material.SetVector("_InputMax", new Vector4(o.InputMaxL / 255f, o.InputMaxL / 255f, o.InputMaxL / 255f, 1f));
				Material.SetVector("_InputGamma", new Vector4(o.InputGammaL, o.InputGammaL, o.InputGammaL, 1f));
				Material.SetVector("_OutputMin", new Vector4(o.OutputMinL / 255f, o.OutputMinL / 255f, o.OutputMinL / 255f, 1f));
				Material.SetVector("_OutputMax", new Vector4(o.OutputMaxL / 255f, o.OutputMaxL / 255f, o.OutputMaxL / 255f, 1f));
			}
			else
			{
				Material.SetVector("_InputMin", new Vector4(o.InputMinR / 255f, o.InputMinG / 255f, o.InputMinB / 255f, 1f));
				Material.SetVector("_InputMax", new Vector4(o.InputMaxR / 255f, o.InputMaxG / 255f, o.InputMaxB / 255f, 1f));
				Material.SetVector("_InputGamma", new Vector4(o.InputGammaR, o.InputGammaG, o.InputGammaB, 1f));
				Material.SetVector("_OutputMin", new Vector4(o.OutputMinR / 255f, o.OutputMinG / 255f, o.OutputMinB / 255f, 1f));
				Material.SetVector("_OutputMax", new Vector4(o.OutputMaxR / 255f, o.OutputMaxG / 255f, o.OutputMaxB / 255f, 1f));
			}

			return 0;
		}

		public override void PreRender(ChromaticaRenderer renderer, Operator op)
		{
			LevelsOperator o = (LevelsOperator)op;

			float aspectRatio = renderer.Camera.aspect;
			Vector2 size = new Vector2((float)o.Quality, (float)o.Quality / aspectRatio);

			// Grab the texture data
			if (m_HistogramTexture == null || size != m_LastHistogramTextureSize)
			{
				if (m_HistogramTexture == null)
					UnityEngine.Object.DestroyImmediate(m_HistogramTexture);

				m_HistogramTexture = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGB24, false, InternalColorSpace.IsLinear);
				m_HistogramTexture.hideFlags = HideFlags.DontSave;
				m_HistogramTexture.filterMode = FilterMode.Point;

				m_LastHistogramTextureSize = size;
			}

			RenderTexture tempRT = RenderTexture.GetTemporary((int)size.x, (int)size.y, 16, RenderTextureFormat.ARGB32);
			renderer.RequestRenderPass(tempRT);
			RenderTexture.active = tempRT;

			m_HistogramTexture.ReadPixels(new Rect(0f, 0f, size.x, size.y), 0, 0, false);
			m_HistogramTexture.Apply(false);

			RenderTexture.ReleaseTemporary(tempRT);
			RenderTexture.active = null;

			// Get the channel histogram
			Color[] pixels = m_HistogramTexture.GetPixels();
			int[] histogram = o.Histogram;
			Array.Clear(histogram, 0, 384);

			if (!o.IsRGB)
			{
				foreach (Color color in pixels)
					histogram[(int)((color.r * 0.3f + color.g * 0.59f + color.b * 0.11f) * 383)]++;
			}
			else if (o.Channel == 0)
			{
				foreach (Color color in pixels)
					histogram[(int)(color.r * 383)]++;
			}
			else if (o.Channel == 1)
			{
				foreach (Color color in pixels)
					histogram[(int)(color.g * 383)]++;
			}
			else if (o.Channel == 2)
			{
				foreach (Color color in pixels)
					histogram[(int)(color.b * 383)]++;
			}

			// Find max histogram value
			float max = 0f;

			for (int i = 0; i < 384; i++)
				max = (max < histogram[i]) ? histogram[i] : max;

			// Scale the histogram values
			if (!o.IsLog)
			{
				float factor = 126f / max;

				for (int i = 0; i < 384; i++)
					histogram[i] = (int)Mathf.Max(Mathf.Round(histogram[i] * factor), 1f);
			}
			else
			{
				float factor = 126f / Mathf.Log10(max);

				for (int i = 0; i < 384; i++)
					histogram[i] = (histogram[i] == 0) ? 0 : (int)Mathf.Max(Mathf.Round(Mathf.Log10(histogram[i]) * factor), 1f);
			}

			// Interpolate the holes (due to 255 -> 384)
			for (int i = 1; i < 383; i++)
			{
				if (histogram[i] == 0)
					histogram[i] = (histogram[i - 1] + histogram[i + 1]) / 2;
			}
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/Levels");
		}
	}
}
