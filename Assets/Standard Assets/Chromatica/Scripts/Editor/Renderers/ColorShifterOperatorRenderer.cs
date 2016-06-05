using UnityEngine;
using Chromatica.Operators;

namespace Chromatica.Studio.Renderers
{
	public class ColorShifterOperatorRenderer : OperatorRenderer
	{
		public override int SetParameters(Operator op)
		{
			ColorShifterOperator o = (ColorShifterOperator)op;
			Material.SetVector("_Master", new Vector4(o.Hue[0] / 360f, o.Saturation[0] / 100f, o.Value[0] / 100f, 0f));

			if (o.Advanced)
			{
				Material.SetVector("_Reds", new Vector4(o.Hue[1] / 360f, o.Saturation[1] / 100f, o.Value[1] / 100f, 0f));
				Material.SetVector("_Yellows", new Vector4(o.Hue[2] / 360f, o.Saturation[2] / 100f, o.Value[2] / 100f, 0f));
				Material.SetVector("_Greens", new Vector4(o.Hue[3] / 360f, o.Saturation[3] / 100f, o.Value[3] / 100f, 0f));
				Material.SetVector("_Cyans", new Vector4(o.Hue[4] / 360f, o.Saturation[4] / 100f, o.Value[4] / 100f, 0f));
				Material.SetVector("_Blues", new Vector4(o.Hue[5] / 360f, o.Saturation[5] / 100f, o.Value[5] / 100f, 0f));
				Material.SetVector("_Magentas", new Vector4(o.Hue[6] / 360f, o.Saturation[6] / 100f, o.Value[6] / 100f, 0f));
				return 1;
			}

			return 0;
		}

		public override Shader GetShader()
		{
			return Shader.Find("Hidden/Chroma/ColorShifter");
		}
	}
}
