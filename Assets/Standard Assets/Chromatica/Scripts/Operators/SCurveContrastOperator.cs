using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class SCurveContrastOperator : Operator
	{
		public float RedSteepness = 1f;
		public float RedGamma = 1f;
		public float GreenSteepness = 1f;
		public float GreenGamma = 1f;
		public float BlueSteepness = 1f;
		public float BlueGamma = 1f;
		public bool DrawCurve = false;

		public override void Init()
		{
			Title = "S-Curve Contrast";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			SCurveContrastOperator o = ScriptableObject.CreateInstance<SCurveContrastOperator>();
			o.Init();

			o.RedSteepness = RedSteepness;
			o.RedGamma = RedGamma;
			o.GreenSteepness = GreenSteepness;
			o.GreenGamma = GreenGamma;
			o.BlueSteepness = BlueSteepness;
			o.BlueGamma = BlueGamma;
			o.DrawCurve = DrawCurve;

			return o;
		}

		public override void ResetToDefault()
		{
			RedSteepness = 1f;
			RedGamma = 1f;
			GreenSteepness = 1f;
			GreenGamma = 1f;
			BlueSteepness = 1f;
			BlueGamma = 1f;
			DrawCurve = false;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<SCurveContrast>
		{0}
		{1}
		{2}
		{3}
		{4}
		{5}
		{6}
	</SCurveContrast>",
					  
					Serializer.SerializeFloat("RedSteepness", RedSteepness),
					Serializer.SerializeFloat("RedGamma", RedGamma),
					Serializer.SerializeFloat("GreenSteepness", GreenSteepness),
					Serializer.SerializeFloat("GreenGamma", GreenGamma),
					Serializer.SerializeFloat("BlueSteepness", BlueSteepness),
					Serializer.SerializeFloat("BlueGamma", BlueGamma),
					Serializer.SerializeBool("DrawCurve", DrawCurve)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			RedSteepness = Serializer.DeserializeFloat(xmlNode.GetChild("RedSteepness"), RedSteepness);
			RedGamma = Serializer.DeserializeFloat(xmlNode.GetChild("RedGamma"), RedGamma);
			GreenSteepness = Serializer.DeserializeFloat(xmlNode.GetChild("GreenSteepness"), GreenSteepness);
			GreenGamma = Serializer.DeserializeFloat(xmlNode.GetChild("GreenGamma"), GreenGamma);
			BlueSteepness = Serializer.DeserializeFloat(xmlNode.GetChild("BlueSteepness"), BlueSteepness);
			BlueGamma = Serializer.DeserializeFloat(xmlNode.GetChild("BlueGamma"), BlueGamma);
			DrawCurve = Serializer.DeserializeBool(xmlNode.GetChild("DrawCurve"), DrawCurve);
		}
	}
}
