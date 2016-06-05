using UnityEngine;
using UnityEditor;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class PosterizeOperatorEditor : OperatorEditor
	{
		SerializedProperty p_levels;

		public override void Init(Operator op)
		{
			base.Init(op);

			p_levels = target.FindProperty("Levels");
		}

		public override void DrawContentUI()
		{
			target.Update();

			EditorGUILayout.PropertyField(p_levels);

			target.ApplyModifiedProperties();
		}
	}
}
