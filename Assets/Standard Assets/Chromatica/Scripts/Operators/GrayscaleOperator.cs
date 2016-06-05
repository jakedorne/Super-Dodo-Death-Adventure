using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class GrayscaleOperator : Operator
	{
		[RangeAttribute(0f, 1f)]
		public float RedLuminance;

		[RangeAttribute(0f, 1f)]
		public float GreenLuminance;

		[RangeAttribute(0f, 1f)]
		public float BlueLuminance;

		[RangeAttribute(0f, 1f)]
		public float Blend;

		public override void Init()
		{
			Title = "Black & White";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			GrayscaleOperator o = ScriptableObject.CreateInstance<GrayscaleOperator>();
			o.Init();

			o.RedLuminance = RedLuminance;
			o.GreenLuminance = GreenLuminance;
			o.BlueLuminance = BlueLuminance;
			o.Blend = Blend;

			return o;
		}

		public override void ResetToDefault()
		{
			RedLuminance = 0.299f;
			GreenLuminance = 0.587f;
			BlueLuminance = 0.114f;
			Blend = 1f;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<Grayscale>
		{0}
		{1}
		{2}
		{3}
	</Grayscale>",

					Serializer.SerializeFloat("RedLuminance", RedLuminance),
					Serializer.SerializeFloat("GreenLuminance", GreenLuminance),
					Serializer.SerializeFloat("BlueLuminance", BlueLuminance),
					Serializer.SerializeFloat("Blend", Blend)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			RedLuminance = Serializer.DeserializeFloat(xmlNode.GetChild("RedLuminance"), RedLuminance);
			GreenLuminance = Serializer.DeserializeFloat(xmlNode.GetChild("GreenLuminance"), GreenLuminance);
			BlueLuminance = Serializer.DeserializeFloat(xmlNode.GetChild("BlueLuminance"), BlueLuminance);
			Blend = Serializer.DeserializeFloat(xmlNode.GetChild("Blend"), Blend);
		}
	}
}
