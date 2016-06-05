using UnityEngine;
using Chromatica.Operators;
using Chromatica.Utils;

namespace Chromatica.Studio.Renderers
{
	public class InvertOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			InvertOperator o = (InvertOperator)op;
			Material.SetFloat("_Blend", o.Blend);
			Material.SetInt("_Linear", InternalColorSpace.IsLinear ? 1 : 0);
			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/Invert");
		}
	}
}
