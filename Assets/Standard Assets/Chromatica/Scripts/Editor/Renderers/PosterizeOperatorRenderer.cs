using UnityEngine;
using Chromatica.Operators;

namespace Chromatica.Studio.Renderers
{
	public class PosterizeOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			PosterizeOperator o = (PosterizeOperator)op;
			Material.SetFloat("_Levels", o.Levels);
			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/Posterize");
		}
	}
}
