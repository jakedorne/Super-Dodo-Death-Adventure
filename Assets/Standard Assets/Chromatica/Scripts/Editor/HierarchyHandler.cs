using UnityEngine;
using UnityEditor;

namespace Chromatica.Studio
{
	[InitializeOnLoad]
	class HierarchyHandler
	{
		static Texture2D icon;

		static HierarchyHandler()
		{
			icon = Chromatica.Studio.Styles.LoadPNG("CS_Icon");
			EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemCallback;
		}

		static void HierarchyWindowItemCallback(int instanceID, Rect selectionRect)
		{
			GameObject gameObject = (GameObject)EditorUtility.InstanceIDToObject(instanceID);

			if (gameObject != null && gameObject.GetComponent<ChromaticaVolume>() != null)
				GUI.Box(new Rect(selectionRect.x + selectionRect.width - 16f, selectionRect.y + 2f, 12f, 12f), icon, GUIStyle.none);
		}
	}
}