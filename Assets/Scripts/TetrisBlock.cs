using UnityEngine;
using System.Collections;

public class TetrisBlock : MonoBehaviour {

	private int[,] 	blocks = new int[,]{
					{1,0,0},
					{1,0,0},
					{1,1,0}
				};
	public Shape shape = Shape.L;

	public enum Shape{L, T, CROSS, IDK};

	// Use this for initialization
	void Start () {
		SetShape(shape);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Rotate(){
		int[,] temp = new int[3,3];
		for(int i=0; i<blocks.GetLength(0); i++){
        	for(int j=blocks.GetLength(1)-1; j>=0; j--){
            	temp[i,j] = blocks[j,i];
        	}
   		}
   		blocks = temp;
	}

	public void SetShape(Shape newShape){
		shape = newShape;
		switch(newShape){
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
