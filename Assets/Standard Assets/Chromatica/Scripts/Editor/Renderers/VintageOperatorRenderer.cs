using UnityEngine;
using Chromatica.Operators;
using Chromatica.Studio.Editors;
using Chromatica.Utils;

namespace Chromatica.Studio.Renderers
{
	public class VintageOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			VintageOperator o = (VintageOperator)op;
			Material.SetFloat("_Blend", o.Blend);
			Texture2D texture = Styles.LoadPNG("Instagram/" + VintageOperatorEditor.filters[o.Filter]);
			Material.SetTexture("_LookupTex", texture);
			return InternalColorSpace.IsLinear ? 1 : 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/Vintage");
		}
	}
}