using UnityEngine;
using Chromatica.Operators;

namespace Chromatica.Studio.Renderers
{
	public class ChannelSwapperOperatorRenderer : OperatorRenderer
	{
		static Vector4[] m_Channels = new Vector4[] {
			new Vector4(1f, 0f, 0f, 0f),
			new Vector4(0f, 1f, 0f, 0f),
			new Vector4(0f, 0f, 1f, 0f)
		};

		public override int SetParameters(Operator op)
		{
			ChannelSwapperOperator o = (ChannelSwapperOperator)op;
			Material.SetVector("_Red", m_Channels[o.Red]);
			Material.SetVector("_Green", m_Channels[o.Green]);
			Material.SetVector("_Blue", m_Channels[o.Blue]);
			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/ChannelSwapper");
		}
	}
}
