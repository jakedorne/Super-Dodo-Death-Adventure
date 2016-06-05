using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using Chromatica.Operators;
using Chromatica.Studio.Renderers;
using Chromatica.Utils;

namespace Chromatica.Studio
{
	[Serializable]
	public class RendererDictionary : SerializedDictionary<Type, OperatorRenderer> { }

	[Serializable]
	public class ChromaticaRenderer
	{
		public Camera Camera;

		[SerializeField]
		RenderTexture m_ActiveLutRT;

		[SerializeField]
		RenderTexture m_LutRT;

		[SerializeField]
		RenderTexture m_LutDepthRT;

		[SerializeField]
		Texture2D m_LookupTexture;

		[SerializeField]
		RendererDictionary m_Renderers;

		RenderTexture m_TempRT;
		Material m_LutMaterial;
		bool m_IsDirty;
		ChromaticaBase m_LastContext;

		public ChromaticaRenderer()
		{
		}

		public void SetDirty()
		{
			m_IsDirty = true;
		}

		void CreateRenderers()
		{
			m_Renderers = new RendererDictionary();
			OperatorRendererItem[] items = OperatorTables.GetRenderers();

			foreach (OperatorRendererItem item in items)
				m_Renderers.Set(item.Type, item.Renderer);
		}

		RenderTexture CreateRenderTexture(string name, int width, int height, int depth, RenderTextureReadWrite readWrite, TextureWrapMode wrap, RenderTexture oldRT)
		{
			if (oldRT != null)
			{
				oldRT.Release();
				UnityEngine.Object.DestroyImmediate(oldRT);
			}

			RenderTexture rt = new RenderTexture(width, height, depth, RenderTextureFormat.ARGB32, readWrite);
			rt.wrapMode = wrap;
			rt.generateMips = false;
			rt.anisoLevel = 0;
			rt.hideFlags = HideFlags.HideAndDontSave;
			rt.name = name;
			rt.Create();
			return rt;
		}

		bool CheckRenderTexture(RenderTexture rt, int width, int height)
		{
			return (rt == null || !rt.IsCreated() || rt.width != (int)width || rt.height != (int)height);
		}

		public void Destroy()
		{
			if (m_LutRT != null)
			{
				m_LutRT.Release();
				UnityEngine.Object.DestroyImmediate(m_LutRT);
			}

			if (m_LutDepthRT != null)
			{
				m_LutDepthRT.Release();
				UnityEngine.Object.DestroyImmediate(m_LutDepthRT);
			}

			if (Camera != null)
			{
				ChromaticaStudio studio = Camera.GetComponent<ChromaticaStudio>();

				if (studio != null)
				{
					studio.EditModeLoookupTexture = null;
					studio.EditModeLoookupTextureDepth = null;
				}
			}
		}

		public bool Update(ChromaticaStudio chromatica, bool forceRender = false)
		{
			ChromaticaBase context = chromatica.ActiveContext;

			if (m_LastContext != context)
			{
				forceRender = true;
				m_LastContext = context;
			}

			if (!m_IsDirty && !forceRender)
				return false;

			if (m_Renderers == null)
				CreateRenderers();

			if (m_LookupTexture == null)
				m_LookupTexture = Resources.Load<Texture2D>("CS_NeutralLUT");

			// Near LUT
			if (context.Operators != null)
			{
				if (CheckRenderTexture(m_LutRT, 256, 16))
					m_LutRT = CreateRenderTexture("Edit mode RT", 256, 16, 0, InternalColorSpace.RTrw, TextureWrapMode.Clamp, m_LutRT);

				m_ActiveLutRT = m_LutRT;

				// Starts rendering
				Begin();

				// Render each operator one by one
				foreach (Operator op in context.Operators)
				{
					// Skip null & disabled operators
					if (op == null || !op.IsEnabled)
						continue;

					// Render
					Render(op);
				}

				// Done
				End();

				Camera.GetComponent<ChromaticaStudio>().EditModeLoookupTexture = m_LutRT;
			}

			// Far LUT
			if (chromatica.Mode == LUTMode.Dual && context.DepthOperators != null)
			{
				if (CheckRenderTexture(m_LutDepthRT, 256, 16))
					m_LutDepthRT = CreateRenderTexture("Edit mode Depth RT", 256, 16, 0, InternalColorSpace.RTrw, TextureWrapMode.Clamp, m_LutDepthRT);

				m_ActiveLutRT = m_LutDepthRT;

				// Starts rendering
				Begin();

				// Render each operator one by one
				foreach (Operator op in context.DepthOperators)
				{
					// Skip null & disabled operators
					if (op == null || !op.IsEnabled)
						continue;

					// Render
					Render(op);
				}

				// Done
				End();

				Camera.GetComponent<ChromaticaStudio>().EditModeLoookupTextureDepth = m_LutDepthRT;
			}

			m_IsDirty = false;
			m_ActiveLutRT = null;

			return true;
		}

		public void Begin()
		{
			Graphics.Blit(m_LookupTexture, m_ActiveLutRT);
			RenderTexture.active = null;
			m_TempRT = RenderTexture.GetTemporary(256, 16, 0, RenderTextureFormat.ARGB32, InternalColorSpace.RTrw);
		}

		public void Render(Operator op)
		{
			OperatorRenderer opRenderer = m_Renderers.Get(op.GetType());
			int pass = opRenderer.SetParameters(op);

			opRenderer.PreRender(this, op);
			Graphics.Blit(m_ActiveLutRT, m_TempRT, opRenderer.Material, pass);
			Graphics.Blit(m_TempRT, m_ActiveLutRT);
			RenderTexture.active = null;
		}

		public void End()
		{
			RenderTexture.active = null;
			RenderTexture.ReleaseTemporary(m_TempRT);
		}

		public void RequestRenderPass(RenderTexture rt)
		{
			// Disable Chromatica before rendering the camera to make sure the LUT isn't applied
			// Also make sure the background color has alpha = 1
			ChromaticaStudio chromatica = Camera.GetComponent<ChromaticaStudio>();
			bool wasChromaEnabled = chromatica.enabled;

			chromatica.enabled = false;
			Color oldBackgroundColor = Camera.backgroundColor;
			Camera.backgroundColor = new Color(oldBackgroundColor.r, oldBackgroundColor.g, oldBackgroundColor.b, 1f);

			RenderTexture oldTargetRT = Camera.targetTexture;
			Camera.targetTexture = rt;
			Camera.Render();
			Camera.targetTexture = oldTargetRT;

			Camera.backgroundColor = oldBackgroundColor;
			chromatica.enabled = wasChromaEnabled;

			// Apply the current LUT
			if (m_LutMaterial == null)
			{
				m_LutMaterial = new Material(Shader.Find("Hidden/Chroma/LUT"));
				m_LutMaterial.hideFlags = HideFlags.HideAndDontSave;
			}

			m_LutMaterial.SetTexture("_LookupTex", m_LutRT);
			RenderTexture tempRT = RenderTexture.GetTemporary(rt.width, rt.height, 0, RenderTextureFormat.ARGB32, InternalColorSpace.RTrw);
			Graphics.Blit(rt, tempRT, m_LutMaterial, 0);
			Graphics.Blit(tempRT, rt);
			RenderTexture.active = null;
			RenderTexture.ReleaseTemporary(tempRT);
		}

		public void Bake(Texture2D target, bool isDepth)
		{
			RenderTexture.active = isDepth ? m_LutDepthRT : m_LutRT;
			target.ReadPixels(new Rect(0, 0, 256, 16), 0, 0, false);
			target.Apply(false);
			RenderTexture.active = null;
		}
	}
}
