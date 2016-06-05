using UnityEngine;
using UnityEditor;
using System;
using Chromatica.Operators;

namespace Chromatica.Studio.Renderers
{
	[Serializable]
	public class OperatorRenderer
	{
		[SerializeField]
		Material m_Material;

		public Material Material
		{
			get
			{
				if (m_Material == null)
				{
					Shader shader = GetShader();
					m_Material = new Material(shader);
					m_Material.hideFlags = HideFlags.HideAndDontSave;
				}

				return m_Material;
			}
		}

		public virtual int SetParameters(Operator op)
		{
			return 0;
		}

		public virtual void PreRender(ChromaticaRenderer renderer, Operator op)
		{
		}

		public virtual Shader GetShader()
		{
			return null;
		}
	}
}
