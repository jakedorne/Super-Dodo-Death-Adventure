using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class ChannelMixerOperator : Operator
	{
		public Vector4 Red;
		public Vector4 Green;
		public Vector4 Blue;
		public int CurrentTab;

		public override void Init()
		{
			Title = "Channel Mixer";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			ChannelMixerOperator o = ScriptableObject.CreateInstance<ChannelMixerOperator>();
			o.Init();

			o.Red = Red;
			o.Green = Green;
			o.Blue = Blue;
			o.CurrentTab = CurrentTab;

			return o;
		}

		public override void ResetToDefault()
		{
			Red = new Vector4(100.0f, 0.0f, 0.0f, 0.0f);
			Green = new Vector4(0.0f, 100.0f, 0.0f, 0.0f);
			Blue = new Vector4(0.0f, 0.0f, 100.0f, 0.0f);
		}

		public override string ToXML()
		{
			return string.Format(@"
	<ChannelMixer>
		{0}
		{1}
		{2}
		{3}
	</ChannelMixer>",

					Serializer.SerializeVector4("Red", Red),
					Serializer.SerializeVector4("Green", Green),
					Serializer.SerializeVector4("Blue", Blue),
					Serializer.SerializeInt("CurrentTab", CurrentTab)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Red = Serializer.DeserializeVector4(xmlNode.GetChild("Red"), Red);
			Green = Serializer.DeserializeVector4(xmlNode.GetChild("Green"), Green);
			Blue = Serializer.DeserializeVector4(xmlNode.GetChild("Blue"), Blue);
			CurrentTab = Serializer.DeserializeInt(xmlNode.GetChild("CurrentTab"), CurrentTab);
		}
	}
}
