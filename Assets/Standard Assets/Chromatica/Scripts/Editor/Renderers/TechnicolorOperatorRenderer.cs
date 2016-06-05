using UnityEngine;
using Chromatica.Operators;

namespace Chromatica.Studio.Renderers
{
	public class TechnicolorOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			TechnicolorOperator o = (TechnicolorOperator)op;
			Material.SetFloat("_Exposure", 8f - o.Exposure);
			Material.SetColor("_Balance", Color.white - o.Balance);
			Material.SetFloat("_Blend", o.Blend);
			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/Technicolor");
		}
	}
}
