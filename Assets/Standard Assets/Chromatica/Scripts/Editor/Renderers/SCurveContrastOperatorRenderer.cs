using UnityEngine;
using Chromatica.Operators;
using Chromatica.Utils;

namespace Chromatica.Studio.Renderers
{
	public class SCurveContrastOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			SCurveContrastOperator o = (SCurveContrastOperator)op;
			Material.SetVector("_Red", new Vector2(o.RedSteepness, o.RedGamma));
			Material.SetVector("_Green", new Vector2(o.GreenSteepness, o.GreenGamma));
			Material.SetVector("_Blue", new Vector2(o.BlueSteepness, o.BlueGamma));
			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/SCurveContrast");
		}
	}
}
