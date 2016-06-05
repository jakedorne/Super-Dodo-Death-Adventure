using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class WhiteBalanceOperator : Operator
	{
		public Color White;
		public int Mode;

		public override void Init()
		{
			Title = "White Balance";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			WhiteBalanceOperator o = ScriptableObject.CreateInstance<WhiteBalanceOperator>();
			o.Init();

			o.White = White;
			o.Mode = Mode;

			return o;
		}

		public override void ResetToDefault()
		{
			float c = Chromatica.Utils.InternalColorSpace.IsLinear ? 0.72974005284f : 0.5f;
			White = new Color(c, c, c, 1f);
			Mode = 1;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<WhiteBalance>
		{0}
		{1}
	</WhiteBalance>",

					Serializer.SerializeColor("White", White),
					Serializer.SerializeInt("Mode", Mode)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			White = Serializer.DeserializeColor(xmlNode.GetChild("White"), White);
			Mode = Serializer.DeserializeInt(xmlNode.GetChild("Mode"), Mode);
		}
	}
}
