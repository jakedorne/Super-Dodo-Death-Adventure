using UnityEngine;
using Chromatica.Operators;

namespace Chromatica.Studio.Renderers
{
	public class ChannelMixerOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			ChannelMixerOperator o = (ChannelMixerOperator)op;
			Material.SetVector("_Red", o.Red / 100f);
			Material.SetVector("_Green", o.Green / 100f);
			Material.SetVector("_Blue", o.Blue / 100f);
			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/ChannelMixer");
		}
	}
}
