using UnityEngine;
using Chromatica.Operators;

namespace Chromatica.Studio.Renderers
{
	public class ThreeWayOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			ThreeWayOperator o = (ThreeWayOperator)op;

			Material.SetVector("_Shadows", o.Shadows * o.Shadows.w);
			float multiplier = 1f + (1f - (o.Midtones.x * 0.299f + o.Midtones.y * 0.587f + o.Midtones.z * 0.114f));
			Material.SetVector("_Midtones", (o.Midtones * multiplier) * o.Midtones.w);
			multiplier = 1f + (1f - (o.Highlights.x * 0.299f + o.Highlights.y * 0.587f + o.Highlights.z * 0.114f));
			Material.SetVector("_Highlights", (o.Highlights * multiplier) * o.Highlights.w);
			Material.SetFloat("_Blend", o.Blend);

			return o.IsCDL ? 1 : 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/ThreeWay");
		}
	}
}
