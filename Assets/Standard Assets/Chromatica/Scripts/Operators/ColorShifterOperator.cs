using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class ColorShifterOperator : Operator
	{
		public bool Advanced;
		public int Channel;

		// Master, Reds, Yellows, Greens, Cyans, Blues, Magentas
		public float[] Hue;
		public float[] Saturation;
		public float[] Value;

		public override void Init()
		{
			Title = "Color Shifter";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			ColorShifterOperator o = ScriptableObject.CreateInstance<ColorShifterOperator>();
			o.Init();

			o.Advanced = Advanced;
			o.Channel = Channel;
			Array.Copy(Hue, o.Hue, 7);
			Array.Copy(Saturation, o.Saturation, 7);
			Array.Copy(Value, o.Value, 7);

			return o;
		}

		public override void ResetToDefault()
		{
			Advanced = false;
			Channel = 0;
			Hue = new float[7];
			Saturation = new float[7];
			Value = new float[7];
		}

		public override string ToXML()
		{
			return string.Format(@"
	<ColorShifter>
		{0}
		{1}
		{2}
		{3}
		{4}
		{5}
		{6}
		{7}
		{8}
	</ColorShifter>",

					Serializer.SerializeBool("Advanced", Advanced),
					Serializer.SerializeInt("Channel", Channel),
					Serializer.SerializeVector3("Master", new Vector3(Hue[0], Saturation[0], Value[0])),
					Serializer.SerializeVector3("Reds", new Vector3(Hue[1], Saturation[1], Value[1])),
					Serializer.SerializeVector3("Yellows", new Vector3(Hue[2], Saturation[2], Value[2])),
					Serializer.SerializeVector3("Greens", new Vector3(Hue[3], Saturation[3], Value[3])),
					Serializer.SerializeVector3("Cyans", new Vector3(Hue[4], Saturation[4], Value[4])),
					Serializer.SerializeVector3("Blues", new Vector3(Hue[5], Saturation[5], Value[5])),
					Serializer.SerializeVector3("Magentas", new Vector3(Hue[6], Saturation[6], Value[6]))
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Advanced = Serializer.DeserializeBool(xmlNode.GetChild("Advanced"), Advanced);
			Channel = Serializer.DeserializeInt(xmlNode.GetChild("Channel"), Channel);

			Vector3 Master = Serializer.DeserializeVector3(xmlNode.GetChild("Master"), Vector3.zero);
			Vector3 Reds = Serializer.DeserializeVector3(xmlNode.GetChild("Reds"), Vector3.zero);
			Vector3 Yellows = Serializer.DeserializeVector3(xmlNode.GetChild("Yellows"), Vector3.zero);
			Vector3 Greens = Serializer.DeserializeVector3(xmlNode.GetChild("Greens"), Vector3.zero);
			Vector3 Cyans = Serializer.DeserializeVector3(xmlNode.GetChild("Cyans"), Vector3.zero);
			Vector3 Blues = Serializer.DeserializeVector3(xmlNode.GetChild("Blues"), Vector3.zero);
			Vector3 Magentas = Serializer.DeserializeVector3(xmlNode.GetChild("Magentas"), Vector3.zero);

			Hue[0] = Master.x; Saturation[0] = Master.y; Value[0] = Master.z;
			Hue[1] = Reds.x; Saturation[1] = Reds.y; Value[1] = Reds.z;
			Hue[2] = Yellows.x; Saturation[2] = Yellows.y; Value[2] = Yellows.z;
			Hue[3] = Greens.x; Saturation[3] = Greens.y; Value[3] = Greens.z;
			Hue[4] = Cyans.x; Saturation[4] = Cyans.y; Value[4] = Cyans.z;
			Hue[5] = Blues.x; Saturation[5] = Blues.y; Value[5] = Blues.z;
			Hue[6] = Magentas.x; Saturation[6] = Magentas.y; Value[6] = Magentas.z;
		}
	}
}
