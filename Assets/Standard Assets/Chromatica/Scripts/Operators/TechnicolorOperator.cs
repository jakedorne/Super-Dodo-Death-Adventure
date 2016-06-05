using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class TechnicolorOperator : Operator
	{
		public Color Balance;

		[RangeAttribute(0f, 8f)]
		public float Exposure;

		[RangeAttribute(0f, 1f)]
		public float Blend;

		public override void Init()
		{
			Title = "Technicolor (Three-Strip)";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			TechnicolorOperator o = ScriptableObject.CreateInstance<TechnicolorOperator>();
			o.Init();

			o.Balance = Balance;
			o.Exposure = Exposure;
			o.Blend = Blend;

			return o;
		}

		public override void ResetToDefault()
		{
			Exposure = 4f;
			Balance = new Color(0.25f, 0.25f, 0.25f);
			Blend = 0.5f;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<Technicolor>
		{0}
		{1}
		{2}
	</Technicolor>",

					Serializer.SerializeFloat("Exposure", Exposure),
					Serializer.SerializeColor("Balance", Balance),
					Serializer.SerializeFloat("Blend", Blend)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Exposure = Serializer.DeserializeFloat(xmlNode.GetChild("Exposure"), Exposure);
			Balance = Serializer.DeserializeColor(xmlNode.GetChild("Balance"), Balance);
			Blend = Serializer.DeserializeFloat(xmlNode.GetChild("Blend"), Blend);
		}
	}
}
