using UnityEngine;
using Chromatica.Operators;
using Chromatica.Utils;

namespace Chromatica.Studio.Renderers
{
	public class HueFocusOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			HueFocusOperator o = (HueFocusOperator)op;
			float h = o.Hue / 360f;
			float r = o.Range / 180f;
			Material.SetVector("_Range", new Vector2(h - r, h + r));
			Material.SetVector("_Params", new Vector3(h, o.Boost + 1f, o.Blend));
			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/HueFocus");
		}
	}
}
