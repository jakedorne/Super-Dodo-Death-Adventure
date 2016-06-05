using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class GradientRampOperator : Operator
	{
		public Gradient GradientRamp;

		public override void Init()
		{
			Title = "Gradient Ramp";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			GradientRampOperator o = ScriptableObject.CreateInstance<GradientRampOperator>();
			o.Init();
			
			GradientColorKey[] colorKeys = (GradientColorKey[])GradientRamp.colorKeys.Clone();
			GradientAlphaKey[] alphaKeys = (GradientAlphaKey[])GradientRamp.alphaKeys.Clone();
			o.GradientRamp.SetKeys(colorKeys, alphaKeys);

			return o;
		}

		public override void ResetToDefault()
		{
			GradientRamp = new Gradient();
			GradientRamp.SetKeys(
				new GradientColorKey[] {
					new GradientColorKey(Color.black, 0f),
					new GradientColorKey(Color.white, 1f),
				},
				new GradientAlphaKey[] {
					new GradientAlphaKey(1f, 0f),
					new GradientAlphaKey(1f, 1f),
				}
			);
		}

		public override string ToXML()
		{
			return string.Format(@"
	<GradientRamp>
		{0}
	</GradientRamp>",

					Serializer.SerializeGradient("Ramp", GradientRamp)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			GradientRamp = Serializer.DeserializeGradient(xmlNode.GetChild("Ramp"), GradientRamp);
		}
	}
}
