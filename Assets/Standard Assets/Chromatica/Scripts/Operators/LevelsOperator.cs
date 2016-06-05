using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public enum HistogramQuality
	{
		Low = 140,
		Medium = 220,
		High = 400
	}

	[Serializable]
	public class LevelsOperator : Operator
	{
		public int[] Histogram;
		public int Preset;
		public int Channel;
		public HistogramQuality Quality;
		public bool IsLog;
		public bool IsRGB;

		public float InputMinL;
		public float InputMaxL;
		public float InputGammaL;
		public float InputMinR;
		public float InputMaxR;
		public float InputGammaR;
		public float InputMinG;
		public float InputMaxG;
		public float InputGammaG;
		public float InputMinB;
		public float InputMaxB;
		public float InputGammaB;

		public float OutputMinL;
		public float OutputMaxL;
		public float OutputMinR;
		public float OutputMaxR;
		public float OutputMinG;
		public float OutputMaxG;
		public float OutputMinB;
		public float OutputMaxB;

		public override void Init()
		{
			Title = "Levels";
			ResetToDefault();
		}

		public override Operator Clone()
		{
			LevelsOperator o = ScriptableObject.CreateInstance<LevelsOperator>();
			o.Init();

			o.Preset = Preset;
			o.Channel = Channel;
			o.Quality = Quality;
			o.IsLog = IsLog;
			o.IsRGB = IsRGB;

			o.InputMinL = InputMinL;
			o.InputMaxL = InputMaxL;
			o.InputGammaL = InputGammaL;
			o.InputMinR = InputMinR;
			o.InputMaxR = InputMaxR;
			o.InputGammaR = InputGammaR;
			o.InputMinG = InputMinG;
			o.InputMaxG = InputMaxG;
			o.InputGammaG = InputGammaG;
			o.InputMinB = InputMinB;
			o.InputMaxB = InputMaxB;
			o.InputGammaB = InputGammaB;

			o.OutputMinL = OutputMinL;
			o.OutputMaxL = OutputMaxL;
			o.OutputMinR = OutputMinR;
			o.OutputMaxR = OutputMaxR;
			o.OutputMinG = OutputMinG;
			o.OutputMaxG = OutputMaxG;
			o.OutputMinB = OutputMinB;
			o.OutputMaxB = OutputMaxB;

			return o;
		}

		public override void ResetToDefault()
		{
			Histogram = new int[384];
			Preset = 0;

			Channel = 0;
			Quality = HistogramQuality.Medium;
			IsLog = false;
			IsRGB = false;

			InputMinL = 0;
			InputMaxL = 255;
			InputGammaL = 1;
			InputMinR = 0;
			InputMaxR = 255;
			InputGammaR = 1;
			InputMinG = 0;
			InputMaxG = 255;
			InputGammaG = 1;
			InputMinB = 0;
			InputMaxB = 255;
			InputGammaB = 1;

			OutputMinL = 0;
			OutputMaxL = 255;
			OutputMinR = 0;
			OutputMaxR = 255;
			OutputMinG = 0;
			OutputMaxG = 255;
			OutputMinB = 0;
			OutputMaxB = 255;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<Levels>
		{0}
		{1}
		{2}
		{3}
		{4}
		{5}
		{6}
		{7}
		{8}
	</Levels>",

					Serializer.SerializeInt("Channel", Channel),
					Serializer.SerializeInt("Precision", (int)Quality),
					Serializer.SerializeBool("IsLog", IsLog),
					Serializer.SerializeBool("IsRGB", IsRGB),
					Serializer.SerializeVector4("InputMin", new Vector4(InputMinR, InputMinG, InputMinB, InputMinL)),
					Serializer.SerializeVector4("InputMax", new Vector4(InputMaxR, InputMaxG, InputMaxB, InputMaxL)),
					Serializer.SerializeVector4("InputGamma", new Vector4(InputGammaR, InputGammaG, InputGammaB, InputGammaL)),
					Serializer.SerializeVector4("OutputMin", new Vector4(OutputMinR, OutputMinG, OutputMinB, OutputMinL)),
					Serializer.SerializeVector4("OutputMax", new Vector4(OutputMaxR, OutputMaxG, OutputMaxB, OutputMaxL))
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			Channel = Serializer.DeserializeInt(xmlNode.GetChild("Channel"), Channel);
			Quality = (HistogramQuality)Serializer.DeserializeInt(xmlNode.GetChild("Precision"), (int)Quality);
			IsLog = Serializer.DeserializeBool(xmlNode.GetChild("IsLog"), IsLog);
			IsRGB = Serializer.DeserializeBool(xmlNode.GetChild("IsRGB"), IsRGB);

			Vector4 inputMin = Serializer.DeserializeVector4(xmlNode.GetChild("InputMin"), new Vector4(InputMinR, InputMinG, InputMinB, InputMinL));
			Vector4 inputMax = Serializer.DeserializeVector4(xmlNode.GetChild("InputMax"), new Vector4(InputMaxR, InputMaxG, InputMaxB, InputMaxL));
			Vector4 inputGamma = Serializer.DeserializeVector4(xmlNode.GetChild("InputGamma"), new Vector4(InputGammaR, InputGammaG, InputGammaB, InputGammaL));
			Vector4 outputMin = Serializer.DeserializeVector4(xmlNode.GetChild("OutputMin"), new Vector4(OutputMinR, OutputMinG, OutputMinB, OutputMinL));
			Vector4 outputMax = Serializer.DeserializeVector4(xmlNode.GetChild("OutputMax"), new Vector4(OutputMaxR, OutputMaxG, OutputMaxB, OutputMaxL));

			InputMinL = inputMin.w;
			InputMaxL = inputMax.w;
			InputGammaL = inputGamma.w;
			InputMinR = inputMin.x;
			InputMaxR = inputMax.x;
			InputGammaR = inputGamma.x;
			InputMinG = inputMin.y;
			InputMaxG = inputMax.y;
			InputGammaG = inputGamma.y;
			InputMinB = inputMin.z;
			InputMaxB = inputMax.z;
			InputGammaB = inputGamma.z;

			OutputMinL = outputMin.w;
			OutputMaxL = outputMax.w;
			OutputMinR = outputMin.x;
			OutputMaxR = outputMax.x;
			OutputMinG = outputMin.y;
			OutputMaxG = outputMax.y;
			OutputMinB = outputMin.z;
			OutputMaxB = outputMax.z;
		}
	}
}
