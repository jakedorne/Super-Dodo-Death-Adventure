using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class ThreeWayOperator : Operator
	{
		public Vector4 Shadows;
		public Vector4 Midtones;
		public Vector4 Highlights;
		public bool IsCDL;
		public bool ShowDetails;

		[RangeAttribute(0f, 1f)]
		public float Blend;

		public override void Init()
		{
			Title = "Three Way Color Corrector";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			ThreeWayOperator o = ScriptableObject.CreateInstance<ThreeWayOperator>();
			o.Init();

			o.Shadows = Shadows;
			o.Midtones = Midtones;
			o.Highlights = Highlights;
			o.IsCDL = IsCDL;
			o.ShowDetails = ShowDetails;
			o.Blend = Blend;

			return o;
		}

		public override void ResetToDefault()
		{
			Shadows = Vector4.one;
			Midtones = Vector4.one;
			Highlights = Vector4.one;
			Blend = 1f;
			IsCDL = false;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<ThreeWay>
		{0}
		{1}
		{2}
		{3}
		{4}
		{5}
	</ThreeWay>",

					Serializer.SerializeVector4("Shadows", Shadows),
					Serializer.SerializeVector4("Midtones", Midtones),
					Serializer.SerializeVector4("Highlights", Highlights),
					Serializer.SerializeFloat("Blend", Blend),
					Serializer.SerializeBool("IsCDL", IsCDL),
					Serializer.SerializeBool("ShowDetails", ShowDetails)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Shadows = Serializer.DeserializeVector4(xmlNode.GetChild("Shadows"), Shadows);
			Midtones = Serializer.DeserializeVector4(xmlNode.GetChild("Midtones"), Midtones);
			Highlights = Serializer.DeserializeVector4(xmlNode.GetChild("Highlights"), Highlights);
			Blend = Serializer.DeserializeFloat(xmlNode.GetChild("Blend"), Blend);
			IsCDL = Serializer.DeserializeBool(xmlNode.GetChild("IsCDL"), IsCDL);
			ShowDetails = Serializer.DeserializeBool(xmlNode.GetChild("ShowDetails"), ShowDetails);
		}
	}
}
