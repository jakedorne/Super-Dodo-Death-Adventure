using UnityEngine;
using Chromatica.Operators;
using Chromatica.Utils;

namespace Chromatica.Studio.Renderers
{
	public class CurvesOperatorRenderer : OperatorRenderer
	{
		Texture2D m_CurveTexture;

		public override int SetParameters(Operator op)
		{
			CurvesOperator o = (CurvesOperator)op;

			if (m_CurveTexture == null)
			{
				m_CurveTexture = new Texture2D(256, 4, TextureFormat.ARGB32, false, InternalColorSpace.IsLinear);
				m_CurveTexture.wrapMode = TextureWrapMode.Clamp;
				m_CurveTexture.filterMode = FilterMode.Bilinear;
				m_CurveTexture.anisoLevel = 0;
				m_CurveTexture.hideFlags = HideFlags.DontSave;
			}

			UpdateTexture(o);

			Material.SetTexture("_CurveTex", m_CurveTexture);

			return 0;
		}

		void UpdateTexture(CurvesOperator op)
		{
			Color[] pixels = m_CurveTexture.GetPixels();

			// Red, Green, Blue
			for (int i = 0; i < 256; i++)
			{
				float c = (float)i / 255f;

				float r = Mathf.Clamp01(op.RedCurve.Evaluate(c));
				float g = Mathf.Clamp01(op.GreenCurve.Evaluate(c));
				float b = Mathf.Clamp01(op.BlueCurve.Evaluate(c));

				pixels[i] = new Color(r, r, r);
				pixels[i + 256] = new Color(g, g, g);
				pixels[i + 512] = new Color(b, b, b);
			}

			m_CurveTexture.SetPixels(pixels);
			m_CurveTexture.Apply();
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/Curves");
		}
	}
}
