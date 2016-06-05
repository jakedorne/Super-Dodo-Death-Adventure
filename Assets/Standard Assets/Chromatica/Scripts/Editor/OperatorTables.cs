using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;
using Chromatica.Studio.Renderers;

namespace Chromatica.Studio
{
	public struct OperatorRendererItem
	{
		public Type Type;
		public OperatorRenderer Renderer;

		public OperatorRendererItem(Type type, OperatorRenderer renderer)
		{
			Type = type;
			Renderer = renderer;
		}
	}

	public class OperatorTables
	{
		public static void AddOperatorMenu(ChromaticaWindow chroma)
		{
			GenericMenu menu = new GenericMenu();
			menu.AddItem(new GUIContent("Black and White"), false, () => chroma.AddOperator(typeof(GrayscaleOperator), null));
			menu.AddItem(new GUIContent("Bleach Bypass"), false, () => chroma.AddOperator(typeof(BleachBypassOperator), null));
			menu.AddItem(new GUIContent("Channel Clamper"), false, () => chroma.AddOperator(typeof(ChannelClamperOperator), null));
			menu.AddItem(new GUIContent("Channel Mixer"), false, () => chroma.AddOperator(typeof(ChannelMixerOperator), null));
			menu.AddItem(new GUIContent("Channel Swapper"), false, () => chroma.AddOperator(typeof(ChannelSwapperOperator), null));
			menu.AddItem(new GUIContent("Color Shifter"), false, () => chroma.AddOperator(typeof(ColorShifterOperator), null));
			menu.AddItem(new GUIContent("Curves"), false, () => chroma.AddOperator(typeof(CurvesOperator), null));
			menu.AddItem(new GUIContent("Exposure"), false, () => chroma.AddOperator(typeof(ExposureOperator), null));
			menu.AddItem(new GUIContent("Gradient Ramp"), false, () => chroma.AddOperator(typeof(GradientRampOperator), null));
			menu.AddItem(new GUIContent("Hue Focus"), false, () => chroma.AddOperator(typeof(HueFocusOperator), null));
			menu.AddItem(new GUIContent("Invert"), false, () => chroma.AddOperator(typeof(InvertOperator), null));
			menu.AddItem(new GUIContent("Levels"), false, () => chroma.AddOperator(typeof(LevelsOperator), null));
			menu.AddItem(new GUIContent("Photo Filter"), false, () => chroma.AddOperator(typeof(PhotoFilterOperator), null));
			menu.AddItem(new GUIContent("Posterize"), false, () => chroma.AddOperator(typeof(PosterizeOperator), null));
			menu.AddItem(new GUIContent("S-Curve Contrast"), false, () => chroma.AddOperator(typeof(SCurveContrastOperator), null));
			menu.AddItem(new GUIContent("Technicolor (Three-Strip)"), false, () => chroma.AddOperator(typeof(TechnicolorOperator), null));
			menu.AddItem(new GUIContent("Three-way Color Corrector"), false, () => chroma.AddOperator(typeof(ThreeWayOperator), null));
			menu.AddItem(new GUIContent("Vibrance and Saturation"), false, () => chroma.AddOperator(typeof(VibSatOperator), null));
			menu.AddItem(new GUIContent("Vintage"), false, () => chroma.AddOperator(typeof(VintageOperator), null));
			menu.AddItem(new GUIContent("White Balance"), false, () => chroma.AddOperator(typeof(WhiteBalanceOperator), null));
			menu.ShowAsContext();
		}

		public static Type GetTypeForXMLName(string name)
		{
			switch (name)
			{
				case "ChannelClamper": return typeof(ChannelClamperOperator);
				case "BleachBypass": return typeof(BleachBypassOperator);
				case "ChannelMixer": return typeof(ChannelMixerOperator);
				case "ChannelSwapper": return typeof(ChannelSwapperOperator);
				case "ColorShifter": return typeof(ColorShifterOperator);
				case "Curves": return typeof(CurvesOperator);
				case "Exposure": return typeof(ExposureOperator);
				case "GradientRamp": return typeof(GradientRampOperator);
				case "Grayscale": return typeof(GrayscaleOperator);
				case "HueFocus": return typeof(HueFocusOperator);
				case "Invert": return typeof(InvertOperator);
				case "Levels": return typeof(LevelsOperator);
				case "PhotoFilter": return typeof(PhotoFilterOperator);
				case "Posterize": return typeof(PosterizeOperator);
				case "SCurveContrast": return typeof(SCurveContrastOperator);
				case "Technicolor": return typeof(TechnicolorOperator);
				case "ThreeWay": return typeof(ThreeWayOperator);
				case "VibSat": return typeof(VibSatOperator);
				case "Vintage": return typeof(VintageOperator);
				case "WhiteBalance": return typeof(WhiteBalanceOperator);
				default: Debug.LogError("Unknown operator: " + name);
					break;
			}

			return null;
		}

		public static OperatorRendererItem[] GetRenderers()
		{
			return new OperatorRendererItem[] {
				new OperatorRendererItem(typeof(BleachBypassOperator), new BleachBypassOperatorRenderer()),
				new OperatorRendererItem(typeof(ChannelClamperOperator), new ChannelClamperOperatorRenderer()),
				new OperatorRendererItem(typeof(ChannelMixerOperator), new ChannelMixerOperatorRenderer()),
				new OperatorRendererItem(typeof(ChannelSwapperOperator), new ChannelSwapperOperatorRenderer()),
				new OperatorRendererItem(typeof(ColorShifterOperator), new ColorShifterOperatorRenderer()),
				new OperatorRendererItem(typeof(CurvesOperator), new CurvesOperatorRenderer()),
				new OperatorRendererItem(typeof(ExposureOperator), new ExposureOperatorRenderer()),
				new OperatorRendererItem(typeof(GradientRampOperator), new GradientRampOperatorRenderer()),
				new OperatorRendererItem(typeof(GrayscaleOperator), new GrayscaleOperatorRenderer()),
				new OperatorRendererItem(typeof(HueFocusOperator), new HueFocusOperatorRenderer()),
				new OperatorRendererItem(typeof(InvertOperator), new InvertOperatorRenderer()),
				new OperatorRendererItem(typeof(LevelsOperator), new LevelsOperatorRenderer()),
				new OperatorRendererItem(typeof(PhotoFilterOperator), new PhotoFilterOperatorRenderer()),
				new OperatorRendererItem(typeof(PosterizeOperator), new PosterizeOperatorRenderer()),
				new OperatorRendererItem(typeof(SCurveContrastOperator), new SCurveContrastOperatorRenderer()),
				new OperatorRendererItem(typeof(TechnicolorOperator), new TechnicolorOperatorRenderer()),
				new OperatorRendererItem(typeof(ThreeWayOperator), new ThreeWayOperatorRenderer()),
				new OperatorRendererItem(typeof(VibSatOperator), new VibSatOperatorRenderer()),
				new OperatorRendererItem(typeof(VintageOperator), new VintageOperatorRenderer()),
				new OperatorRendererItem(typeof(WhiteBalanceOperator), new WhiteBalanceOperatorRenderer())
			};
		}
	}
}
