using UnityEngine;
using System.Collections;

public class BlockSelector : MonoBehaviour {

    public int row;
    public int col;
    public TetrisBlock tetrisBlock; //Placeholder - I think this will eventually be passed in from BlockPlacement

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
        Floor.GetComponent<BlockPlacement>().toggleBlockPlacement();
        Floor.GetComponent<Floor>().AddTetrisBlock(row,col,tetrisBlock);
    }
}
