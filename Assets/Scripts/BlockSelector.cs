using UnityEngine;
using System.Collections;

public class BlockSelector : MonoBehaviour {

    public int row;
    public int col;

    private GameObject Floor;

	// Use this for initialization
	void Start () {
        Floor = GameObject.Find("Floor");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Floor.GetComponent<BlockPlacement>().turnBlockPlacementOff(row,col);
    }

    void OnMouseOver()
    {
        Floor.GetComponent<BlockPlacement>().showHoverOver(row, col);
    }
}
