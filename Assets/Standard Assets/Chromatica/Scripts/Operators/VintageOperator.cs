using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class VintageOperator : Operator
	{
		[RangeAttribute(0f, 1f)]
		public float Blend;

		public int Filter;

		public override void Init()
		{
			Title = "Vintage";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			VintageOperator o = ScriptableObject.CreateInstance<VintageOperator>();
			o.Init();

			o.Blend = Blend;
			o.Filter = Filter;

			return o;
		}

		public override void ResetToDefault()
		{
			Blend = 1f;
			Filter = 0;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<Vintage>
		{0}
		{1}
	</Vintage>",

					Serializer.SerializeFloat("Blend", Blend),
					Serializer.SerializeInt("Filter", Filter)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Blend = Serializer.DeserializeFloat(xmlNode.GetChild("Blend"), Blend);
			Filter = Serializer.DeserializeInt(xmlNode.GetChild("Filter"), Filter);
		}
	}
}
