using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class CurvesOperator : Operator
	{
		public AnimationCurve RedCurve;
		public AnimationCurve GreenCurve;
		public AnimationCurve BlueCurve;
		public int CurrentTab;

		public override void Init()
		{
			Title = "Curves";
			RedCurve = new AnimationCurve();
			GreenCurve = new AnimationCurve();
			BlueCurve = new AnimationCurve();
			ResetToDefault();
		}

		public override Operator Clone()
		{
			CurvesOperator o = ScriptableObject.CreateInstance<CurvesOperator>();
			o.Init();

			o.RedCurve.keys = RedCurve.keys; o.RedCurve.preWrapMode = RedCurve.preWrapMode; o.RedCurve.postWrapMode = RedCurve.postWrapMode;
			o.GreenCurve.keys = GreenCurve.keys; o.GreenCurve.preWrapMode = GreenCurve.preWrapMode; o.GreenCurve.postWrapMode = GreenCurve.postWrapMode;
			o.BlueCurve.keys = BlueCurve.keys; o.BlueCurve.preWrapMode = BlueCurve.preWrapMode; o.BlueCurve.postWrapMode = BlueCurve.postWrapMode;
			o.CurrentTab = CurrentTab;

			return o;
		}

		public override void ResetToDefault()
		{
			Keyframe[] keyframes = new Keyframe[] {
				new Keyframe(0f, 0f, 1f, 1f),
				new Keyframe(1f, 1f, 1f, 1f)
			};

			RedCurve.keys = keyframes;
			GreenCurve.keys = keyframes;
			BlueCurve.keys = keyframes;
			CurrentTab = 0;
		}

		public override string ToXML()
		{
			return string.Format(@"
	<Curves>
		{0}
		{1}
		{2}
		{3}
	</Curves>",

					Serializer.SerializeCurve("Red", RedCurve),
					Serializer.SerializeCurve("Green", GreenCurve),
					Serializer.SerializeCurve("Blue", BlueCurve),
					Serializer.SerializeInt("CurrentTab", CurrentTab)
				);
		}

		public override void FromXML(XmlNode xmlNode)
		{
			RedCurve = Serializer.DeserializeCurve(xmlNode.GetChild("Red"), RedCurve);
			GreenCurve = Serializer.DeserializeCurve(xmlNode.GetChild("Green"), GreenCurve);
			BlueCurve = Serializer.DeserializeCurve(xmlNode.GetChild("Blue"), BlueCurve);
			CurrentTab = Serializer.DeserializeInt(xmlNode.GetChild("CurrentTab"), CurrentTab);
		}
	}
}
