using UnityEngine;
using Chromatica.Operators;
using Chromatica.Utils;

namespace Chromatica.Studio.Renderers
{
	public class GradientRampOperatorRenderer : OperatorRenderer
	{
		Texture2D m_GradientTexture;

		public override int SetParameters(Operator op)
		{
			GradientRampOperator o = (GradientRampOperator)op;

			if (m_GradientTexture == null)
			{
				m_GradientTexture = new Texture2D(256, 1, TextureFormat.ARGB32, false, InternalColorSpace.IsLinear);
				m_GradientTexture.wrapMode = TextureWrapMode.Clamp;
				m_GradientTexture.filterMode = FilterMode.Bilinear;
				m_GradientTexture.anisoLevel = 0;
				m_GradientTexture.hideFlags = HideFlags.DontSave;
			}

			UpdateTexture(o);

			Material.SetTexture("_RampTex", m_GradientTexture);

			return 0;
		}

		public void UpdateTexture(GradientRampOperator op)
		{
			Color[] pixels = new Color[256];
			Gradient g = op.GradientRamp;

			for (int i = 0; i < 256; i++)
				pixels[i] = g.Evaluate(i / 255f);

			m_GradientTexture.SetPixels(pixels);
			m_GradientTexture.Apply();
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/GradientRamp");
		}
	}
}
