using UnityEngine;
using System.Collections;

public class TetrisBlock : MonoBehaviour {

	private int[,] blocks;
	public Shape shape = Shape.L;

	public enum Shape{L, T, CROSS, IDK};

	// Use this for initialization
	void Start () {
		SetShape();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void SetShape(){
		switch(shape){
			case Shape.L:
				blocks = new int[,]{
					{1,0,0},
					{1,0,0},
					{1,1,0}
				};
				break;
			case Shape.T:
				blocks = new int[,]{
					{1,1,1},
					{0,1,0},
					{0,1,0}
				};
				break;
			case Shape.CROSS:
				blocks = new int[,]{
					{0,1,0},
					{1,1,1},
					{0,1,0}
				};
				break;
			case Shape.IDK:
				blocks = new int[,]{
					{0,0,0},
					{0,0,0},
					{0,0,0}
				};
				break;

		}
	}

	public int[,] GetBlocks(){
		return blocks;
	}
}
