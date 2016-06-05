using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace Chromatica.Studio
{
	public class LUTConverter : ScriptableWizard
	{
		public Texture2D[] OldLUTs;

		Material m_Mat;
		Material Mat
		{
			get
			{
				if (m_Mat == null)
				{
					m_Mat = new Material(Shader.Find("Hidden/Chroma/Convert LUT"));
					m_Mat.hideFlags = HideFlags.HideAndDontSave;
				}

				return m_Mat;
			}
		}

		[MenuItem("Window/Chromatica LUT Converter")]
		static void CreateWizard()
		{
			var wizard = ScriptableWizard.DisplayWizard<LUTConverter>("Chromatica LUT Converter", "Convert");
			wizard.minSize = new Vector2(300, 150);
			wizard.helpString = "Converts old LUTs (512x512) to the new, faster\nLUT format (256x16).";
		}

		void OnWizardUpdate()
		{
			if (OldLUTs == null || OldLUTs.Length == 0)
			{
				errorString = "";
				isValid = false;
				return;
			}

			for (int i = 0; i < OldLUTs.Length; i++)
			{
				Texture lut = OldLUTs[i];

				if (lut == null)
					continue;

				if (lut.width != 512 || lut.height != 512)
				{
					errorString = "Invalid LUT format. Expected 512x512 textures.";
					isValid = false;
					return;
				}
			}

			errorString = "";
			isValid = true;
		}

		void OnWizardCreate()
		{
			Texture2D neutralLut = Resources.Load<Texture2D>("CS_NeutralLUT");

			if (neutralLut == null)
			{
				Debug.LogError("CS_NeutralLUT is missing ! Make sure Chromatica has been installed correctly.");
				return;
			}

			for (int i = 0; i < OldLUTs.Length; i++)
			{
				Texture2D lut = OldLUTs[i];

				if (lut == null)
					continue;

				RenderTexture rt = RenderTexture.GetTemporary(256, 16, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);

				Mat.SetTexture("_LutTex", lut);
				Graphics.Blit(neutralLut, rt, Mat);

				Texture2D t = new Texture2D(256, 16, TextureFormat.RGB24, false, true);
				t.anisoLevel = 0;
				t.wrapMode = TextureWrapMode.Clamp;
				t.filterMode = FilterMode.Bilinear;

				RenderTexture oldActive = RenderTexture.active;
				RenderTexture.active = rt;
				t.ReadPixels(new Rect(0, 0, 256, 16), 0, 0);
				t.Apply();
				RenderTexture.active = oldActive;

				RenderTexture.ReleaseTemporary(rt);

				string srcPath = AssetDatabase.GetAssetPath(lut);
				string dstPath = srcPath.Substring(0, srcPath.LastIndexOf(".")) + "_Converted.png";
				string fullPath = Application.dataPath.Remove(Application.dataPath.Length - 6) + dstPath;
				byte[] bytes = t.EncodeToPNG();

				try
				{
					File.WriteAllBytes(fullPath, bytes);
				}
				catch (Exception e)
				{
					Debug.LogError("Error while saving converted LUT: " + e.StackTrace);
					return;
				}

				AssetDatabase.Refresh();
				TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(dstPath);
				importer.textureType = TextureImporterType.Advanced;
				importer.spriteImportMode = SpriteImportMode.None;
				importer.textureFormat = TextureImporterFormat.RGB24;
				importer.isReadable = false;
				importer.mipmapEnabled = false;
				importer.anisoLevel = 0;
				importer.maxTextureSize = 256;
				importer.linearTexture = true;
				importer.wrapMode = TextureWrapMode.Repeat;
				importer.filterMode = FilterMode.Bilinear;
				AssetDatabase.ImportAsset(dstPath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				EditorUtility.DisplayProgressBar("Chromatica LUT Converter", "srcPath", (float)i / (float)OldLUTs.Length);
				Debug.Log("Converted LUT to: " + dstPath);
			}

			EditorUtility.ClearProgressBar();
		}
	}
}