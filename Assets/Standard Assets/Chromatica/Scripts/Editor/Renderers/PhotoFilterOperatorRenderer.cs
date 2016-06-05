using UnityEngine;
using Chromatica.Operators;

namespace Chromatica.Studio.Renderers
{
	public class PhotoFilterOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			PhotoFilterOperator o = (PhotoFilterOperator)op;
			Color filter = new Color(o.Filter.r, o.Filter.g, o.Filter.b, o.Density);
			Material.SetColor("_Filter", filter);
			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/PhotoFilter");
		}
	}
}
