using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class ChannelSwapperOperator : Operator
	{
		public int Red;
		public int Green;
		public int Blue;

		public override void Init()
		{
			Title = "Channel Swapper";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			ChannelSwapperOperator o = ScriptableObject.CreateInstance<ChannelSwapperOperator>();
			o.Init();

			o.Red = Red;
			o.Green = Green;
			o.Blue = Blue;

			return o;
		}

		public override void ResetToDefault()
		{
			Red = 0;
			Green = 1;
			Blue = 2;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<ChannelSwapper>
		{0}
		{1}
		{2}
	</ChannelSwapper>",

					Serializer.SerializeInt("Red", Red),
					Serializer.SerializeInt("Green", Green),
					Serializer.SerializeInt("Blue", Blue)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Red = Serializer.DeserializeInt(xmlNode.GetChild("Red"), Red);
			Green = Serializer.DeserializeInt(xmlNode.GetChild("Green"), Green);
			Blue = Serializer.DeserializeInt(xmlNode.GetChild("Blue"), Blue);
		}
	}
}
