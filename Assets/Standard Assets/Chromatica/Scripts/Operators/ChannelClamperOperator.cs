using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class ChannelClamperOperator : Operator
	{
		public Vector2 Red;
		public Vector2 Green;
		public Vector2 Blue;

		public override void Init()
		{
			Title = "Channel Clamper";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			ChannelClamperOperator o = ScriptableObject.CreateInstance<ChannelClamperOperator>();
			o.Init();

			o.Red = Red;
			o.Green = Green;
			o.Blue = Blue;

			return o;
		}

		public override void ResetToDefault()
		{
			Red = new Vector2(0f, 1f);
			Green = new Vector2(0f, 1f);
			Blue = new Vector2(0f, 1f);
		}

		public override string ToXML()
		{
			return string.Format(@"
	<ChannelClamper>
		{0}
		{1}
		{2}
	</ChannelClamper>",

					Serializer.SerializeVector2("Red", Red),
					Serializer.SerializeVector2("Green", Red),
					Serializer.SerializeVector2("Blue", Red)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Red = Serializer.DeserializeVector2(xmlNode.GetChild("Red"), Red);
			Green = Serializer.DeserializeVector2(xmlNode.GetChild("Green"), Green);
			Blue = Serializer.DeserializeVector2(xmlNode.GetChild("Blue"), Blue);
		}
	}
}
