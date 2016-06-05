using UnityEngine;
using Chromatica.Operators;
using Chromatica.Utils;
using System.Collections.Generic;

[AddComponentMenu("")]
public class ChromaticaBase : MonoBehaviour
{
	public Texture LookupTexture;
	public Texture LookupTextureDepth;
	public AnimationCurve DepthCurve;

	private Texture2D m_DepthCurveTexture;
	public Texture2D DepthCurveTexture
	{
		get
		{
			if (m_DepthCurveTexture == null)
			{
				m_DepthCurveTexture = new Texture2D(256, 1, TextureFormat.ARGB32, false, InternalColorSpace.TEXrw);
				m_DepthCurveTexture.name = "Depth Curve Texture";
				m_DepthCurveTexture.wrapMode = TextureWrapMode.Clamp;
				m_DepthCurveTexture.anisoLevel = 0;
				m_DepthCurveTexture.filterMode = FilterMode.Bilinear;
				m_DepthCurveTexture.hideFlags = HideFlags.DontSave;
				UpdateCurve(m_DepthCurveTexture);
			}

			return m_DepthCurveTexture;
		}
	}

	protected static Texture2D m_DefaultLookupTexture;
	protected static Texture2D m_DefaultDepthCurveTexture;

	public List<Operator> Operators = new List<Operator>();
	public List<Operator> DepthOperators = new List<Operator>();

#if UNITY_EDITOR
	public bool IsDepthCurveEditorExpanded = false;
	public bool HasBeenReset = false;
#endif

	protected virtual void Reset()
	{
		DepthCurve = new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f));
		UpdateCurve();

#if UNITY_EDITOR
		HasBeenReset = true;
#endif
	}

	protected virtual void OnEnable()
	{
		if (DepthCurve == null)
			DepthCurve = new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f));

		if (m_DefaultLookupTexture == null)
		{
			m_DefaultLookupTexture = Resources.Load<Texture2D>("CS_NeutralLUT");
			m_DefaultLookupTexture.hideFlags = HideFlags.None; // Compatibility fix with Chromatica 1.x and 2.0
		}

		if (m_DefaultDepthCurveTexture == null)
		{
			m_DefaultDepthCurveTexture = new Texture2D(256, 1, TextureFormat.ARGB32, false, InternalColorSpace.TEXrw);
			m_DefaultDepthCurveTexture.name = "Default Depth Curve Texture";
			m_DefaultDepthCurveTexture.wrapMode = TextureWrapMode.Clamp;
			m_DefaultDepthCurveTexture.anisoLevel = 0;
			m_DefaultDepthCurveTexture.filterMode = FilterMode.Bilinear;
			m_DefaultDepthCurveTexture.hideFlags = HideFlags.DontSave;
			UpdateDefaultCurve();
		}
	}

	protected virtual void OnDisable()
	{
		Release(m_DepthCurveTexture);
	}

	protected void Release(UnityEngine.Object obj)
	{
		if (obj != null)
			DestroyImmediate(obj);
	}

	protected Texture TryGet(Texture texture, Texture defaultTexture)
	{
		return (texture == null) ? defaultTexture : texture;
	}

	public Texture2D UpdateCurve()
	{
		if (m_DepthCurveTexture == null)
			return DepthCurveTexture;
		else
			return UpdateCurve(m_DepthCurveTexture);
	}

	Texture2D UpdateCurve(Texture2D texture)
	{
		if (DepthCurve == null)
			return m_DefaultDepthCurveTexture;

		Color[] pixels = texture.GetPixels();

		for (int i = 0; i < 256; i++)
		{
			float z = Mathf.Clamp01(DepthCurve.Evaluate((float)i / 255f));
			pixels[i] = new Color(z, z, z, z);
		}

		texture.SetPixels(pixels);
		texture.Apply();

		return texture;
	}

	static Texture2D UpdateDefaultCurve()
	{
		Color[] pixels = m_DefaultDepthCurveTexture.GetPixels();

		for (int i = 0; i < 256; i++)
		{
			float z = Mathf.Clamp01((float)i / 255f);
			pixels[i] = new Color(z, z, z, z);
		}

		m_DefaultDepthCurveTexture.SetPixels(pixels);
		m_DefaultDepthCurveTexture.Apply();

		return m_DefaultDepthCurveTexture;
	}
}
