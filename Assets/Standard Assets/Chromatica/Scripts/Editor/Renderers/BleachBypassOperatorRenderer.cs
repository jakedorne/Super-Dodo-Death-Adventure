using UnityEngine;
using Chromatica.Operators;
using Chromatica.Utils;

namespace Chromatica.Studio.Renderers
{
	public class BleachBypassOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			BleachBypassOperator o = (BleachBypassOperator)op;
			Material.SetVector("_Data", new Vector4(o.RedLuminance, o.GreenLuminance, o.BlueLuminance, o.Blend));
			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/BleachBypass");
		}
	}
}
