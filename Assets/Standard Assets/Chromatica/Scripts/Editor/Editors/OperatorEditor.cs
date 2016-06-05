using UnityEngine;
using UnityEditor;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class OperatorEditor
	{
		public Operator Operator { get; private set; }
		public SerializedObject target { get; private set; }

		public OperatorEditor()
		{
		}

		public virtual void Init(Operator op)
		{
			Operator = op;
			target = new SerializedObject(op);
		}

		public virtual void Reset()
		{
		}

		public virtual void DrawContentUI()
		{
		}
	}
}
