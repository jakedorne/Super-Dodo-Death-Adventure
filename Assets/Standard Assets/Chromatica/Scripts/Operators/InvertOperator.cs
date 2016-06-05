using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class InvertOperator : Operator
	{
		[RangeAttribute(0f, 1f)]
		public float Blend;

		public override void Init()
		{
			Title = "Invert";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			InvertOperator o = ScriptableObject.CreateInstance<InvertOperator>();
			o.Init();

			o.Blend = Blend;

			return o;
		}

		public override void ResetToDefault()
		{
			Blend = 1f;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<Invert>
		{0}
	</Invert>",

					Serializer.SerializeFloat("Blend", Blend)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Blend = Serializer.DeserializeFloat(xmlNode.GetChild("Blend"), Blend);
		}
	}
}
