using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class ExposureOperator : Operator
	{
		[RangeAttribute(0f, 10f)]
		public float Exposure;

		[RangeAttribute(0.1f, 10f)]
		public float Gamma;

		[RangeAttribute(-1f, 1f)]
		public float Offset;

		[RangeAttribute(-100f, 100f)]
		public float Brightness;

		public Vector4 Contrast;

		public override void Init()
		{
			Title = "Exposure";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			ExposureOperator o = ScriptableObject.CreateInstance<ExposureOperator>();
			o.Init();

			o.Exposure = Exposure;
			o.Gamma = Gamma;
			o.Offset = Offset;
			o.Brightness = Brightness;
			o.Contrast = Contrast;

			return o;
		}

		public override void ResetToDefault()
		{
			Exposure = 1f;
			Gamma = 1f;
			Offset = 0f;
			Brightness = 0f;
			Contrast = new Vector4(0.5f, 0.5f, 0.5f, 0.0f);
		}

		public override string ToXML()
		{
			return string.Format(@"
	<Exposure>
		{0}
		{1}
		{2}
		{3}
		{4}
	</Exposure>",

					Serializer.SerializeFloat("Exposure", Exposure),
					Serializer.SerializeFloat("Gamma", Gamma),
					Serializer.SerializeFloat("Offset", Offset),
					Serializer.SerializeFloat("Brightness", Brightness),
					Serializer.SerializeVector4("Contrast", Contrast)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Exposure = Serializer.DeserializeFloat(xmlNode.GetChild("Exposure"), Exposure);
			Gamma = Serializer.DeserializeFloat(xmlNode.GetChild("Gamma"), Gamma);
			Offset = Serializer.DeserializeFloat(xmlNode.GetChild("Offset"), Offset);
			Brightness = Serializer.DeserializeFloat(xmlNode.GetChild("Brightness"), Brightness);
			Contrast = Serializer.DeserializeVector4(xmlNode.GetChild("Contrast"), Contrast);
		}
	}
}
