using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour {

	public int rows;
	public int cols;

	// Use this for initialization
	void Start () {
		RectTransform parent = gameObject.GetComponent<RectTransform> ();
		GridLayoutGroup grid = gameObject.GetComponent<GridLayoutGroup> ();

		float xUnit = parent.rect.width / ((cols*4) + 3);
		float yUnit = parent.rect.height / ((rows*4) + 3);
		float width = xUnit * 4;
		float height = yUnit * 4;

		grid.spacing = new Vector2 (xUnit, yUnit);

		grid.cellSize = new Vector2 (width, height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
