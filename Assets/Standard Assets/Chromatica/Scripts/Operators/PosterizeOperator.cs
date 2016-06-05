using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class PosterizeOperator : Operator
	{
		[RangeAttribute(2, 255)]
		public int Levels;

		public override void Init()
		{
			Title = "Posterize";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			PosterizeOperator o = ScriptableObject.CreateInstance<PosterizeOperator>();
			o.Init();

			o.Levels = Levels;

			return o;
		}

		public override void ResetToDefault()
		{
			Levels = 4;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<Posterize>
		{0}
	</Posterize>",

					Serializer.SerializeInt("Levels", Levels)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Levels = Serializer.DeserializeInt(xmlNode.GetChild("Levels"), Levels);
		}
	}
}
