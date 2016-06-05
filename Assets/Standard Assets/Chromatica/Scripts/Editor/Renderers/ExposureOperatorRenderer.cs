using UnityEngine;
using Chromatica.Operators;

namespace Chromatica.Studio.Renderers
{
	public class ExposureOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			ExposureOperator o = (ExposureOperator)op;

			Material.SetFloat("_Exposure", o.Exposure);
			Material.SetFloat("_Offset", o.Offset);
			Material.SetFloat("_Gamma", 1f / o.Gamma);

			Material.SetFloat("_Brightness", (o.Brightness + 100f) / 100f);

			Vector4 contrast = o.Contrast;
			contrast.w = (contrast.w + 100f) / 100f;
			Material.SetVector("_Contrast", contrast);

			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/Exposure");
		}
	}
}
