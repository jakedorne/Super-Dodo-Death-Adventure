using UnityEngine;
using Chromatica.Operators;

namespace Chromatica.Studio.Renderers
{
	public class ChannelClamperOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			ChannelClamperOperator o = (ChannelClamperOperator)op;

			Material.SetVector("_RedClamp", o.Red);
			Material.SetVector("_GreenClamp", o.Green);
			Material.SetVector("_BlueClamp", o.Blue);

			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/ChannelClamper");
		}
	}
}
