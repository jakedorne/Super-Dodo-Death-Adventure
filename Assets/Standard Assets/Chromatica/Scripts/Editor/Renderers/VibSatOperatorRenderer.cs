using UnityEngine;
using Chromatica.Operators;

namespace Chromatica.Studio.Renderers
{
	public class VibSatOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			VibSatOperator o = (VibSatOperator)op;
			Vector4 vibrance = o.Vibrance;
			vibrance.w /= 100f;
			Material.SetColor("_Vibrance", vibrance);
			Material.SetFloat("_Saturation", o.Saturation / 100f);
			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/VibSat");
		}
	}
}
