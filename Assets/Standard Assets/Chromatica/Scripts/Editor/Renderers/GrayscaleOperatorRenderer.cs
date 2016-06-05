using UnityEngine;
using Chromatica.Operators;

namespace Chromatica.Studio.Renderers
{
	public class GrayscaleOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			GrayscaleOperator o = (GrayscaleOperator)op;
			Material.SetVector("_Data", new Vector4(o.RedLuminance, o.GreenLuminance, o.BlueLuminance, o.Blend));
			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/Grayscale");
		}
	}
}
