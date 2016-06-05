using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class PhotoFilterOperator : Operator
	{
		public int Preset;
		public Color Filter;

		[RangeAttribute(0f, 1f)]
		public float Density;

		public override void Init()
		{
			Title = "Photo Filter";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			PhotoFilterOperator o = ScriptableObject.CreateInstance<PhotoFilterOperator>();
			o.Init();

			o.Preset = Preset;
			o.Filter = Filter;
			o.Density = Density;

			return o;
		}

		public override void ResetToDefault()
		{
			Preset = 0;
			Filter = new Color(1.0f, 0.5f, 0.2f);
			Density = 0.35f;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<PhotoFilter>
		{0}
		{1}
	</PhotoFilter>",

					Serializer.SerializeColor("Filter", Filter),
					Serializer.SerializeFloat("Density", Density)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Filter = Serializer.DeserializeColor(xmlNode.GetChild("Filter"), Filter);
			Density = Serializer.DeserializeFloat(xmlNode.GetChild("Density"), Density);
		}
	}
}
