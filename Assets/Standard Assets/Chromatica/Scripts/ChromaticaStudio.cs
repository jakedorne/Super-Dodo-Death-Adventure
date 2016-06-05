using UnityEngine;
using System;
using System.Collections.Generic;
using Chromatica.Utils;
using Chromatica.Operators;

[Serializable]
public enum LUTSplit
{
	None,
	Horizontal,
	Vertical
}

[Serializable]
public enum LUTMode
{
	Single,
	Dual
}

[DisallowMultipleComponent, RequireComponent(typeof(Camera))]
[ExecuteInEditMode, AddComponentMenu("Chromatica Studio")]
public class ChromaticaStudio : ChromaticaBase
{
	public LUTMode Mode = LUTMode.Single;
	public LUTSplit Split = LUTSplit.None;

	[Range(0f, 1f)]
	public float Contribution = 1f;

	public Texture ScreenMask;
	public Vector4 ScreenMaskTilingOffset = new Vector4(1f, 1f, 0f, 0f);
	public bool ScreenMaskInvert = false;

	[Range(0f, 1f)]
	public float ScreenMaskOpacity = 1f;

	public bool DisableAllBlending = false;

	public Transform VolumeTrigger = null;
	public LayerMask VolumeMask = -1;

	public bool UseVolume { get { return VolumeTrigger != null; } }
	public bool IsBlending { get; protected set; }

	public bool UseTonemapping = false;
	public float Exposure = 0.85f;

	protected Camera m_Camera;

	protected Material m_Material;
	public Material Material
	{
		get
		{
			if (m_Material == null)
			{
				m_Material = new Material(Shader.Find("Hidden/Chroma/LUT"));
				m_Material.name = "Chroma LUT Material";
				m_Material.hideFlags = HideFlags.HideAndDontSave;
			}

			return m_Material;
		}
	}

	protected Material m_MaterialLerp;
	public Material MaterialLerp
	{
		get
		{
			if (m_MaterialLerp == null)
			{
				m_MaterialLerp = new Material(Shader.Find("Hidden/Chroma/Texture Lerp"));
				m_MaterialLerp.name = "Chroma Lerp Material";
				m_MaterialLerp.hideFlags = HideFlags.HideAndDontSave;
			}

			return m_MaterialLerp;
		}
	}

	protected ChromaticaVolume m_CurrentVolume;
	protected float m_CurrentTransitionTime;
	protected float m_CurrentVolumeTimer;

	protected Texture m_TargetLookupTexture;
	protected Texture m_TargetLookupTextureDepth;
	protected Texture m_TargetDepthCurveTexture;

	protected RenderTexture m_LookupTextureRT;
	protected RenderTexture m_LookupTextureBlendRT;
	protected RenderTexture m_LookupTextureDepthRT;
	protected RenderTexture m_LookupTextureDepthBlendRT;
	protected RenderTexture m_DepthCurveTextureRT;
	protected RenderTexture m_DepthCurveTextureBlendRT;

	private ColorSpace _ColorSpace;
	private LUTMode _Mode;

#if UNITY_EDITOR
	public ChromaticaVolume VolumeContext;
	public ChromaticaBase ActiveContext { get { return VolumeContext == null ? (ChromaticaBase)this : VolumeContext; } }

	public List<Operator> ActiveOperators
	{
		get
		{
			if (VolumeContext != null)
				return IsEditingDepthLUT ? VolumeContext.DepthOperators : VolumeContext.Operators;

			return IsEditingDepthLUT ? DepthOperators : Operators;
		}
	}

	public bool IsEditingDepthLUT = false;

	public RenderTexture EditModeLoookupTexture;
	public RenderTexture EditModeLoookupTextureDepth;
#endif

	void Start()
	{
		// Disable if we don't support image effects
		if (!SystemInfo.supportsImageEffects)
		{
			Debug.LogWarning("Image Effects are not supported on this platform.");
			enabled = false;
			return;
		}

		// Shader Model 3.0+ only
		if (SystemInfo.graphicsShaderLevel < 30)
		{
			Debug.LogWarning("ShaderModel 3.0+ required to use Chromatica.");
			enabled = false;
			return;
		}

		// Fallback to LUTMode.Single if we don't support depth textures
		if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth) && Mode == LUTMode.Dual)
		{
			Debug.LogWarning("Depth texture not supported. Switching to LUTMode.Single.");
			Mode = LUTMode.Single;
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		m_Camera = GetComponent<Camera>();

		// Track these so we can refresh the RenderTextures if one change
		_ColorSpace = QualitySettings.activeColorSpace;
		_Mode = Mode;

		if (Mode == LUTMode.Dual)
			m_Camera.depthTextureMode |= DepthTextureMode.Depth;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		Release(m_Material);
		Release(m_MaterialLerp);
		ReleaseRenderTextures();
	}

	void ReleaseRenderTextures()
	{
		Release(m_LookupTextureRT);
		Release(m_LookupTextureBlendRT);
		Release(m_LookupTextureDepthRT);
		Release(m_LookupTextureDepthBlendRT);
		Release(m_DepthCurveTextureRT);
		Release(m_DepthCurveTextureBlendRT);
	}

	void Update()
	{
		if (DisableAllBlending)
			return;

		if (IsBlending)
			m_CurrentVolumeTimer += Time.deltaTime;

		if (!UseVolume)
			return;

		// Volume collision check
		ChromaticaVolume newVolume = CheckForVolumeCollision();

		if (newVolume != m_CurrentVolume)
		{
			float transitionTime = 0f;

			if (m_CurrentVolume != null && newVolume == null)
			{
				transitionTime = m_CurrentVolume.ExitTransitionTime;
			}
			else if (m_CurrentVolume == null && newVolume != null)
			{
				transitionTime = newVolume.TransitionTime;
			}
			else // m_CurrentVolume != null && newVolume != null)
			{
				transitionTime = (m_CurrentVolume.ExitTransitionTime + newVolume.TransitionTime) / 2f;
			}

			m_CurrentVolume = newVolume;

			ChromaticaBase target = (m_CurrentVolume != null) ? m_CurrentVolume : (ChromaticaBase)this;
			Texture lookupTexture = target.LookupTexture;
			Texture lookupTextureDepth = null;
			Texture depthCurveTexture = null;

			if (Mode == LUTMode.Dual)
			{
				lookupTextureDepth = target.LookupTextureDepth;
				depthCurveTexture = target.DepthCurveTexture;
			}

			BlendTo(lookupTexture, lookupTextureDepth, depthCurveTexture, transitionTime);
		}
	}

	void RefreshRenderTextures()
	{
		if (!enabled)
			return;

		ReleaseRenderTextures();

		// We'll use the LUTs from the current volume (if any) to reconstruct the RenderTextures, so that if the
		// trigger starts in a volume it will use its LUTs instead of blending from the start.
		Texture tex = null;

		// Lookup Texture
		m_LookupTextureBlendRT = CreateRenderTexture(256, 16, TextureWrapMode.Repeat, "Lookup Texture Blend RT");
		m_LookupTextureRT = CreateRenderTexture(256, 16, TextureWrapMode.Repeat, "Lookup Texture RT");
		tex = TryGet((m_CurrentVolume == null) ? LookupTexture : m_CurrentVolume.LookupTexture, m_DefaultLookupTexture);
		Graphics.Blit(tex, m_LookupTextureRT);

		if (Mode == LUTMode.Dual)
		{
			// Lookup Texture Depth
			m_LookupTextureDepthBlendRT = CreateRenderTexture(256, 16, TextureWrapMode.Repeat, "Lookup Texture Depth Blend RT");
			m_LookupTextureDepthRT = CreateRenderTexture(256, 16, TextureWrapMode.Repeat, "Lookup Texture Depth RT");
			tex = TryGet((m_CurrentVolume == null) ? LookupTextureDepth : m_CurrentVolume.LookupTextureDepth, m_DefaultLookupTexture);
			Graphics.Blit(tex, m_LookupTextureDepthRT);

			// Curve Texture
			m_DepthCurveTextureBlendRT = CreateRenderTexture(256, 1, TextureWrapMode.Clamp, "Depth Curve Texture Blend RT");
			m_DepthCurveTextureRT = CreateRenderTexture(256, 1, TextureWrapMode.Clamp, "Depth Curve Texture RT");
			tex = (m_CurrentVolume == null) ? DepthCurveTexture : m_CurrentVolume.DepthCurveTexture;
			Graphics.Blit(tex, m_DepthCurveTextureRT);
		}
	}

	RenderTexture CreateRenderTexture(int width, int height, TextureWrapMode wrapMode, string name)
	{
		RenderTexture rt = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, InternalColorSpace.RTrw);
		rt.name = name;
		rt.hideFlags = HideFlags.HideAndDontSave;
		rt.wrapMode = wrapMode;
		rt.anisoLevel = 0;
		rt.useMipMap = false;
		rt.Create();
		return rt;
	}

	[ImageEffectTransformsToLDR]
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Material.SetFloat("_Contribution", Contribution);

		int passOffset = InternalColorSpace.IsLinear ? 8 : 0;

		if (Mode == LUTMode.Dual)
			passOffset += 4;

		// Tonemapping
		if (UseTonemapping)
		{
			Material.EnableKeyword("CHROMA_TONEMAPPING");
			Material.SetFloat("_Exposure", Exposure);
		}
		else
		{
			Material.DisableKeyword("CHROMA_TONEMAPPING");
		}

		// Mask & split
		if (ScreenMask != null)
		{
			Material.SetFloat("_MaskOpacity", ScreenMaskOpacity);
			Material.SetTexture("_Mask", ScreenMask);
			Material.SetFloat("_MaskInvert", ScreenMaskInvert ? 0f : 1f);
			Material.SetVector("_MaskTilingOffset", ScreenMaskTilingOffset);
			passOffset++;
		}
		else if (Split != LUTSplit.None)
		{
			passOffset += 1 + (int)Split;
		}

#if UNITY_EDITOR
		// In edit mode, only render the working LUT
		if (EditModeLoookupTexture != null)
		{
			Material.SetTexture("_LookupTex", EditModeLoookupTexture);
			Material.SetVector("_LutParams", new Vector3(1f / (float)EditModeLoookupTexture.width, 1f / (float)EditModeLoookupTexture.height, Mathf.Sqrt((float)EditModeLoookupTexture.width) - 1f));

			if (Mode == LUTMode.Dual)
			{
				Material.SetTexture("_LookupTexDepth", EditModeLoookupTextureDepth);
				Material.SetTexture("_DepthCurve", ActiveContext.DepthCurveTexture);
			}

			Graphics.Blit(source, destination, Material, passOffset);
			return;
		}
#endif

		// Render without blending (faster, less VRam usage)
		if (DisableAllBlending || !Application.isPlaying)
		{
			// Release RT memory
			if (m_LookupTextureRT != null)
				ReleaseRenderTextures();

			Texture lut = TryGet(LookupTexture, m_DefaultLookupTexture);
			Material.SetTexture("_LookupTex", lut);
			Material.SetVector("_LutParams", new Vector3(1f / (float)lut.width, 1f / (float)lut.height, Mathf.Sqrt((float)lut.width) - 1f));

			if (Mode == LUTMode.Dual)
			{
				Material.SetTexture("_LookupTexDepth", TryGet(LookupTextureDepth, m_DefaultLookupTexture));
				Material.SetTexture("_DepthCurve", DepthCurveTexture);
			}

			Graphics.Blit(source, destination, Material, passOffset);
			return;
		}

		// Check resources and (re)create RTs if needed
		if (m_LookupTextureRT == null || _ColorSpace != QualitySettings.activeColorSpace || _Mode != Mode)
		{
			RefreshRenderTextures();
			_ColorSpace = QualitySettings.activeColorSpace;
			_Mode = Mode;

			if (_Mode == LUTMode.Dual)
				m_Camera.depthTextureMode |= DepthTextureMode.Depth;
		}

		// LUT blending
		Material.SetTexture("_LookupTex", m_LookupTextureRT);
		Material.SetTexture("_LookupTexDepth", m_LookupTextureDepthRT);
		Material.SetTexture("_DepthCurve", m_DepthCurveTextureRT);
		Material.SetVector("_LutParams", new Vector3(1f / (float)m_LookupTextureRT.width, 1f / (float)m_LookupTextureRT.height, Mathf.Sqrt((float)m_LookupTextureRT.width) - 1f));

		if (IsBlending)
		{
			if (m_CurrentVolumeTimer >= m_CurrentTransitionTime)
			{
				IsBlending = false;
				Graphics.Blit(m_TargetLookupTexture, m_LookupTextureRT);

				if (Mode == LUTMode.Dual)
				{
					Graphics.Blit(m_TargetLookupTextureDepth, m_LookupTextureDepthRT);
					Graphics.Blit(m_TargetDepthCurveTexture, m_DepthCurveTextureRT);
				}
			}
			else
			{
				float time = m_CurrentVolumeTimer / m_CurrentTransitionTime;

				MaterialLerp.SetTexture("_To", m_TargetLookupTexture);
				MaterialLerp.SetFloat("_T", time);
				Graphics.Blit(m_LookupTextureRT, m_LookupTextureBlendRT, MaterialLerp);
				Material.SetTexture("_LookupTex", m_LookupTextureBlendRT);
				
				if (Mode == LUTMode.Dual)
				{
					MaterialLerp.SetTexture("_To", m_TargetLookupTextureDepth);
					MaterialLerp.SetFloat("_T", time);
					Graphics.Blit(m_LookupTextureDepthRT, m_LookupTextureDepthBlendRT, MaterialLerp);
					Material.SetTexture("_LookupTexDepth", m_LookupTextureDepthBlendRT);

					MaterialLerp.SetTexture("_To", m_TargetDepthCurveTexture);
					MaterialLerp.SetFloat("_T", time);
					Graphics.Blit(m_DepthCurveTextureRT, m_DepthCurveTextureBlendRT, MaterialLerp);
					Material.SetTexture("_DepthCurve", m_DepthCurveTextureBlendRT);
				}
			}
		}

		Graphics.Blit(source, destination, Material, passOffset);
		return;
	}

	/// <summary>
	/// Manually starts blending to the given LUTs. Note that this behaviour will be overriden as soon
	/// as an interaction with a ChromaticaVolume happens. Pass <code>null</code> to any of the
	/// <code>Texture</code> parameters to use the default <code>Texture</code>.
	/// </summary>
	/// <param name="lut"></param>
	/// <param name="lutDepth"></param>
	/// <param name="curveTex"></param>
	/// <param name="transitionTime"></param>
	public void BlendTo(Texture lut, Texture lutDepth, Texture depthCurveTex, float transitionTime)
	{
		// If we're already blending save the current state
		if (IsBlending)
		{
			Graphics.Blit(m_LookupTextureBlendRT, m_LookupTextureRT);
			Graphics.Blit(m_LookupTextureDepthBlendRT, m_LookupTextureDepthRT);
			Graphics.Blit(m_DepthCurveTextureBlendRT, m_DepthCurveTextureRT);
		}

		IsBlending = true;
		m_CurrentTransitionTime = transitionTime;
		m_CurrentVolumeTimer = 0f;

		m_TargetLookupTexture = TryGet(lut, m_DefaultLookupTexture);
		m_TargetLookupTextureDepth = null;
		m_TargetDepthCurveTexture = null;

		if (Mode == LUTMode.Dual)
		{
			m_TargetLookupTextureDepth = TryGet(lutDepth, m_DefaultLookupTexture);
			m_TargetDepthCurveTexture = TryGet(depthCurveTex, m_DefaultDepthCurveTexture);
		}
	}

	/// <summary>
	/// Returns the volume colliding with the VolumeTrigger transform, if any. In case of multiple
	/// overlapping volumes, the first one found will be returned.
	/// </summary>
	/// <returns></returns>
	public ChromaticaVolume CheckForVolumeCollision()
	{
		if (VolumeTrigger == null)
			return null;

		Collider[] hits = Physics.OverlapSphere(VolumeTrigger.transform.position, 0.01f, VolumeMask);

		if (hits.Length == 0)
			return null;

		ChromaticaVolume volume = null;

		foreach (Collider hit in hits)
		{
			volume = hit.GetComponent<ChromaticaVolume>();

			if (volume != null)
				break; // Volumes could be overlapping each other, only select the first one we find
		}

		return volume;
	}
}
