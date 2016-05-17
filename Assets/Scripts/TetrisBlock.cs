using UnityEngine;
using System.Collections;

public class TetrisBlock : MonoBehaviour {

	private int[,] 	blocks = new int[,]{
					{0,1,0},
					{0,1,0},
					{0,1,1}
				};
	public Shape shape = Shape.L;

	public enum Shape{L, T, CROSS, C, LINE, S};

	// Use this for initialization
	void Start () {
		SetShape(shape);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Rotate(){
		int[,] temp = new int[blocks.GetLength(1),blocks.GetLength(0)];
		for(int i=0; i<temp.GetLength(0); i++){
        	for(int j = 0; j < temp.GetLength(1); j++){
				temp[j,blocks.GetLength(0)-1-i] = blocks[i,j];
        	}
   		}
   		blocks = temp;
	}

	public void SetShape(Shape newShape){
		shape = newShape;
		switch(newShape){
			case Shape.L:
				blocks = new int[,]{
					{0,1,0},
					{0,1,0},
					{0,1,1}
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
			case Shape.C:
				blocks = new int[,]{
					{0,1,1},
					{0,1,0},
					{0,1,1}
				};
				break;
			case Shape.LINE:
				blocks = new int[,]{
					{0,1,0,0},
					{0,1,0,0},
					{0,1,0,0},
					{0,1,0,0}
				};
				break;
			case Shape.S:
				blocks = new int[,]{
					{0,1,0},
					{0,1,1},
					{0,0,1}
				};
				break;

        }
	}

	public int[,] GetBlocks(){
		return blocks;
	}
}
