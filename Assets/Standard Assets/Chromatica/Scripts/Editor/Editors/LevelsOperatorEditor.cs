using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;
using Chromatica.Utils;

namespace Chromatica.Studio.Editors
{
	public class LevelsOperatorEditor : OperatorEditor
	{
		static string[] channels = { "Red", "Green", "Blue" };

		static string[] presets = { "Default", "Darker", "Increase Contrast 1", "Increase Contrast 2", "Increase Contrast 3",
								"Lighten Shadows", "Lighter", "Midtones Brighter", "Midtones Darker" };

		static float[,] presetsData = { { 0, 1, 255, 0, 255 }, { 15, 1, 255, 0, 255 }, { 10, 1, 245, 0, 255 },
									 { 20, 1, 235, 0, 255 }, { 30, 1, 225, 0, 255 }, { 0, 1.6f, 255, 0, 255 },
									 { 0, 1, 230, 0, 255 }, { 0, 1.25f, 255, 0, 255 }, { 0, 0.75f, 255, 0, 255 } };

		SerializedProperty p_isRGB;
		SerializedProperty p_isLog;
		SerializedProperty p_channel;
		SerializedProperty p_quality;
		SerializedProperty p_preset;

		SerializedProperty p_inputMinL;
		SerializedProperty p_inputMaxL;
		SerializedProperty p_inputGammaL;
		SerializedProperty p_inputMinR;
		SerializedProperty p_inputMaxR;
		SerializedProperty p_inputGammaR;
		SerializedProperty p_inputMinG;
		SerializedProperty p_inputMaxG;
		SerializedProperty p_inputGammaG;
		SerializedProperty p_inputMinB;
		SerializedProperty p_inputMaxB;
		SerializedProperty p_inputGammaB;

		SerializedProperty p_outputMinL;
		SerializedProperty p_outputMaxL;
		SerializedProperty p_outputMinR;
		SerializedProperty p_outputMaxR;
		SerializedProperty p_outputMinG;
		SerializedProperty p_outputMaxG;
		SerializedProperty p_outputMinB;
		SerializedProperty p_outputMaxB;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_isRGB = target.FindProperty("IsRGB");
			p_isLog = target.FindProperty("IsLog");
			p_channel = target.FindProperty("Channel");
			p_quality = target.FindProperty("Quality");
			p_preset = target.FindProperty("Preset");

			p_inputMinL = target.FindProperty("InputMinL");
			p_inputMaxL = target.FindProperty("InputMaxL");
			p_inputGammaL = target.FindProperty("InputGammaL");
			p_inputMinR = target.FindProperty("InputMinR");
			p_inputMaxR = target.FindProperty("InputMaxR");
			p_inputGammaR = target.FindProperty("InputGammaR");
			p_inputMinG = target.FindProperty("InputMinG");
			p_inputMaxG = target.FindProperty("InputMaxG");
			p_inputGammaG = target.FindProperty("InputGammaG");
			p_inputMinB = target.FindProperty("InputMinB");
			p_inputMaxB = target.FindProperty("InputMaxB");
			p_inputGammaB = target.FindProperty("InputGammaB");

			p_outputMinL = target.FindProperty("OutputMinL");
			p_outputMaxL = target.FindProperty("OutputMaxL");
			p_outputMinR = target.FindProperty("OutputMinR");
			p_outputMaxR = target.FindProperty("OutputMaxR");
			p_outputMinG = target.FindProperty("OutputMinG");
			p_outputMaxG = target.FindProperty("OutputMaxG");
			p_outputMinB = target.FindProperty("OutputMinB");
			p_outputMaxB = target.FindProperty("OutputMaxB");
		}

		public override void DrawContentUI()
		{
			target.Update();

			int[] histogram = ((LevelsOperator)Operator).Histogram;

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.BeginHorizontal();
				if (p_isRGB.boolValue) p_channel.intValue = EditorGUILayout.Popup(p_channel.intValue, channels);
				p_isRGB.boolValue = GUILayout.Toggle(p_isRGB.boolValue, "Multi-channel Mode", EditorStyles.miniButton);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Separator();

			// Top buttons
			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				GUILayout.FlexibleSpace();
				EditorGUILayout.BeginHorizontal(GUILayout.Width(384));

					EditorGUILayout.PropertyField(p_quality, new GUIContent(), GUILayout.Width(100));
					GUILayout.FlexibleSpace();

					if (GUILayout.Button("Auto B&W", EditorStyles.miniButton, GUILayout.Width(100)))
					{
						Undo.RecordObject((LevelsOperator)Operator, "Auto B&W");

						// Find min and max value on the current channel
						int min = 0, max = 383;

						for (int i = 0; i < 384; i++)
						{
							if (histogram[383 - i] > 0)
								min = 383 - i;

							if (histogram[i] > 0)
								max = i;
						}

						float scaledMin = min * (255f / 384f);
						float scaledMax = max * (255f / 384f);

						if (!p_isRGB.boolValue)
						{
							p_inputMinL.floatValue = scaledMin;
							p_inputMaxL.floatValue = scaledMax;
						}
						else
						{
							int c = p_channel.intValue;

							if (c == 0)
							{
								p_inputMinR.floatValue = scaledMin;
								p_inputMaxR.floatValue = scaledMax;
							}
							else if (c == 1)
							{
								p_inputMinG.floatValue = scaledMin;
								p_inputMaxG.floatValue = scaledMax;
							}
							else if (c == 2)
							{
								p_inputMinB.floatValue = scaledMin;
								p_inputMaxB.floatValue = scaledMax;
							}
						}
					}

					GUILayout.FlexibleSpace();
					p_isLog.boolValue = GUILayout.Toggle(p_isLog.boolValue, "Logarithmic", EditorStyles.miniButton, GUILayout.Width(100));

				EditorGUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			if (EditorGUI.EndChangeCheck())
				ChromaticaWindow.inst.Renderer.SetDirty();

			// Histogram
			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				GUILayout.FlexibleSpace();

				Rect histogramRect = GUILayoutUtility.GetRect(384, 128);

				GUI.Box(histogramRect, "");

				if (!p_isRGB.boolValue) Handles.color = Styles.inst.HistogramLuminanceColor;
				else if (p_channel.intValue == 0) Handles.color = Styles.inst.HistogramRedColor;
				else if (p_channel.intValue == 1) Handles.color = Styles.inst.HistogramGreenColor;
				else if (p_channel.intValue == 2) Handles.color = Styles.inst.HistogramBlueColor;

				for (int i = 0; i < 384; i++)
				{
					Handles.DrawLine(
							new Vector2(histogramRect.x + i + 1f, histogramRect.yMax - 1f),
							new Vector2(histogramRect.x + i + 1f, histogramRect.yMin - 1f + (histogramRect.height - histogram[i]))
						);
				}

				GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			// Bottom buttons
			EditorGUILayout.Separator();
			if (!p_isRGB.boolValue)
			{
				ChannelUI(p_inputMinL, p_inputMaxL, p_inputGammaL, p_outputMinL, p_outputMaxL);
			}
			else
			{
				int c = p_channel.intValue;
				if (c == 0) ChannelUI(p_inputMinR, p_inputMaxR, p_inputGammaR, p_outputMinR, p_outputMaxR);
				else if (c == 1) ChannelUI(p_inputMinG, p_inputMaxG, p_inputGammaG, p_outputMinG, p_outputMaxG);
				else if (c == 2) ChannelUI(p_inputMinB, p_inputMaxB, p_inputGammaB, p_outputMinB, p_outputMaxB);
			}

			// Presets
			if (!p_isRGB.boolValue)
			{
				EditorGUILayout.Separator();
				EditorGUI.BeginChangeCheck();
				p_preset.intValue = EditorGUILayout.Popup("Preset", p_preset.intValue, presets);
				if (EditorGUI.EndChangeCheck())
				{
					int p = p_preset.intValue;
					p_inputMinL.floatValue = presetsData[p, 0];
					p_inputGammaL.floatValue = presetsData[p, 1];
					p_inputMaxL.floatValue = presetsData[p, 2];
					p_outputMinL.floatValue = presetsData[p, 3];
					p_outputMaxL.floatValue = presetsData[p, 4];
				}
			}

			target.ApplyModifiedProperties();
		}

		void ChannelUI(SerializedProperty p_inputMin, SerializedProperty p_inputMax, SerializedProperty p_inputGamma, SerializedProperty p_outputMin, SerializedProperty p_outputMax)
		{
			float inputMin = p_inputMin.floatValue;
			float inputGamma = p_inputGamma.floatValue;
			float inputMax = p_inputMax.floatValue;
			float outputMin = p_outputMin.floatValue;
			float outputMax = p_outputMax.floatValue;

			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				GUILayout.FlexibleSpace();
				EditorGUILayout.BeginHorizontal(GUILayout.Width(384));

					EditorGUILayout.MinMaxSlider(ref inputMin, ref inputMax, 0, 255);

				EditorGUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				GUILayout.FlexibleSpace();
				EditorGUILayout.BeginHorizontal(GUILayout.Width(384));

					inputMin = EditorGUILayout.FloatField(inputMin, GUILayout.Width(50));
					GUILayout.FlexibleSpace();
					inputGamma = EditorGUILayout.FloatField(inputGamma, GUILayout.Width(50));
					GUILayout.FlexibleSpace();
					inputMax = EditorGUILayout.FloatField(inputMax, GUILayout.Width(50));

				EditorGUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(8);

			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				GUILayout.FlexibleSpace();
				EditorGUILayout.BeginHorizontal(GUILayout.Width(384));

					Rect rampRect = GUILayoutUtility.GetRect(384, 20);
					EditorGUI.DrawPreviewTexture(rampRect, InternalColorSpace.IsLinear ? Styles.inst.GrayscaleLinearTexture : Styles.inst.GrayscaleTexture);

				EditorGUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				GUILayout.FlexibleSpace();
				EditorGUILayout.BeginHorizontal(GUILayout.Width(384));

					EditorGUILayout.MinMaxSlider(ref outputMin, ref outputMax, 0, 255);

				EditorGUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				GUILayout.FlexibleSpace();
				EditorGUILayout.BeginHorizontal(GUILayout.Width(384));

					outputMin = EditorGUILayout.FloatField(outputMin, GUILayout.Width(50));
					GUILayout.FlexibleSpace();
					outputMax = EditorGUILayout.FloatField(outputMax, GUILayout.Width(50));

				EditorGUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			inputMin = Mathf.Clamp((float)Math.Round(inputMin, Preferences.Decimals), 0, 254);
			inputGamma = (float)Math.Round(inputGamma, Preferences.Decimals);
			inputMax = Mathf.Clamp((float)Math.Round(inputMax, Preferences.Decimals), 1, 255);
			outputMin = Mathf.Clamp((float)Math.Round(outputMin, Preferences.Decimals), 0, 254);
			outputMax = Mathf.Clamp((float)Math.Round(outputMax, Preferences.Decimals), 1, 255);

			p_inputMin.floatValue = inputMin;
			p_inputMax.floatValue = inputMax;
			p_inputGamma.floatValue = inputGamma;
			p_outputMin.floatValue = outputMin;
			p_outputMax.floatValue = outputMax;
		}
	}
}
