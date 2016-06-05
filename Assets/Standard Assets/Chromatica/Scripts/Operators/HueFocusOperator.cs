using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class HueFocusOperator : Operator
	{
		[Range(0f, 360f)]
		public float Hue = 0f;

		[Range(1f, 180f)]
		public float Range = 30f;

		[Range(0f, 1f)]
		public float Boost = 0.5f;

		[Range(0f, 1f)]
		public float Blend = 1f;

		public override void Init()
		{
			Title = "Hue Focus";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			HueFocusOperator o = ScriptableObject.CreateInstance<HueFocusOperator>();
			o.Init();

			o.Hue = Hue;
			o.Range = Range;
			o.Boost = Boost;
			o.Blend = Blend;

			return o;
		}

		public override void ResetToDefault()
		{
			Hue = 0f;
			Range = 30f;
			Boost = 0.5f;
			Blend = 1f;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<HueFocus>
		{0}
		{1}
		{2}
		{3}
	</HueFocus>",

					Serializer.SerializeFloat("Hue", Hue),
					Serializer.SerializeFloat("Range", Range),
					Serializer.SerializeFloat("Boost", Boost),
					Serializer.SerializeFloat("Blend", Blend)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Hue = Serializer.DeserializeFloat(xmlNode.GetChild("Hue"), Hue);
			Range = Serializer.DeserializeFloat(xmlNode.GetChild("Range"), Range);
			Boost = Serializer.DeserializeFloat(xmlNode.GetChild("Boost"), Boost);
			Blend = Serializer.DeserializeFloat(xmlNode.GetChild("Blend"), Blend);
		}
	}
}
