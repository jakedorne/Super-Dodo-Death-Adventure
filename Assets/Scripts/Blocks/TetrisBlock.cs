using UnityEngine;
using System.Collections;

public class TetrisBlock : MonoBehaviour {

	private int[,] 	blocks;

	// prefabs
	public GameObject blockPrefab;
	public GameObject bridgePrefab;

	public GameObject block;
	public Shape shape;
	public Type type;

	public enum Shape{L, T, CROSS, C, LINE, S, Z, J};
	public enum Type{BASIC, BRIDGE, JUMP};

	// Use this for initialization
	void Awake () {
		// just because most likely case.
		SetType (Type.BRIDGE);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RotateLeft(){
		int[,] temp = new int[blocks.GetLength(1),blocks.GetLength(0)];
		for(int i=0; i<temp.GetLength(0); i++){
			for(int j = 0; j < temp.GetLength(1); j++){
				temp[j,blocks.GetLength(0)-1-i] = blocks[i,j];
			}
		}
		blocks = temp;
	}

	public void RotateRight(){
		// the shame...
		RotateLeft (); RotateLeft (); RotateLeft ();
	}

	public void SetType(Type type){
		this.type = type;
		switch(type){
			case Type.BASIC:
				block = blockPrefab;
				break;
			case Type.BRIDGE:
				block = bridgePrefab;
				break;
		}
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
					{0,1,0},
					{0,1,0},
					{0,1,0}
				};
				break;
			case Shape.S:
				blocks = new int[,]{
					{0,1,0},
					{0,1,1},
					{0,0,1}
				};
				break;
            case Shape.Z:
                blocks = new int[,]{
                    {0,1,0},
                    {1,1,0},
                    {1,0,0}
                };
                break;
            case Shape.J:
                blocks = new int[,]{
                    {0,1,0},
                    {0,1,0},
                    {1,1,0}
                };
                break;

        }
	}

	public int[,] GetBlocks(){
		return blocks;
	}
}
