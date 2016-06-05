using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
[ExecuteInEditMode, AddComponentMenu("")]
public class ChromaticaVolume : ChromaticaBase
{
#if UNITY_EDITOR
	public static Color BoundsColor = new Color(0.15f, 0.69f, 0.93f, 1f);
#endif

	public float TransitionTime = 1f;
	public float ExitTransitionTime = 1f;

	protected BoxCollider m_BoxCollider;

	protected override void OnEnable()
	{
		base.OnEnable();
		m_BoxCollider = GetComponent<BoxCollider>();
		m_BoxCollider.isTrigger = true;
	}
	
#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Matrix4x4 matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Gizmos.matrix = matrix;
		Gizmos.color = BoundsColor;
		Gizmos.DrawWireCube(m_BoxCollider.center, m_BoxCollider.size);
	}

	void OnDrawGizmosSelected()
	{
		Matrix4x4 matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Gizmos.matrix = matrix;
		Gizmos.color = new Color(BoundsColor.r, BoundsColor.g, BoundsColor.b, 0.2f);
		Gizmos.DrawCube(m_BoxCollider.center, m_BoxCollider.size);
	}
#endif
}
