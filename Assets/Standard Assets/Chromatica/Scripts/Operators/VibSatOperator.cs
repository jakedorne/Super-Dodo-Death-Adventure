using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class VibSatOperator : Operator
	{
		public Vector4 Vibrance;

		[RangeAttribute(-100f, 100f)]
		public float Saturation;

		public override void Init()
		{
			Title = "Vibrance & Saturation";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			VibSatOperator o = ScriptableObject.CreateInstance<VibSatOperator>();
			o.Init();

			o.Vibrance = Vibrance;
			o.Saturation = Saturation;

			return o;
		}

		public override void ResetToDefault()
		{
			Vibrance = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
			Saturation = 0f;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<VibSat>
		{0}
		{1}
	</VibSat>",

					Serializer.SerializeVector4("Vibrance", Vibrance),
					Serializer.SerializeFloat("Saturation", Saturation)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Vibrance = Serializer.DeserializeVector4(xmlNode.GetChild("Vibrance"), Vibrance);
			Saturation = Serializer.DeserializeFloat(xmlNode.GetChild("Saturation"), Saturation);
		}
	}
}
