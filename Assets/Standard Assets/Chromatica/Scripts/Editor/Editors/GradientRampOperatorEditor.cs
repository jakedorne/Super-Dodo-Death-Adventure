using UnityEngine;
using UnityEditor;
using Chromatica.Operators;

namespace Chromatica.Studio.Editors
{
	public class GradientRampOperatorEditor : OperatorEditor
	{
		public override void DrawContentUI()
		{
			EditorGUILayout.HelpBox("Use the alpha keys to control the blending.", MessageType.Info);

			try
			{
				// Stupid harmless exception thrown by the editor
				((GradientRampOperator)Operator).GradientRamp = GUIUtility.GradientField("Gradient", ((GradientRampOperator)Operator).GradientRamp);
			}
			catch { }
		}
	}
}
