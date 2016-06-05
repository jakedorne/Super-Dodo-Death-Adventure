using UnityEngine;
using Chromatica.Operators;

namespace Chromatica.Studio.Renderers
{
	public class WhiteBalanceOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			WhiteBalanceOperator o = (WhiteBalanceOperator)op;
			Material.SetColor("_White", o.White);
			return o.Mode;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/WhiteBalance");
		}
	}
}
