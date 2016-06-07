using UnityEngine;
using System.Collections;

public class TetrisBlock : MonoBehaviour {

	private int[,] 	blocks;

	// prefabs
	public GameObject blockPrefab;
	public GameObject bridgePrefab;

	public GameObject block;
	public Type type;

	public enum Type{L, T, CROSS, C, LINE, S, Z, J, BRIDGE, JUMP};

	// Used for the tutorial
	public bool hasBeenRotated = false;

	// Use this for initialization
	void Awake () {
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
		if (!hasBeenRotated) {
			hasBeenRotated = true;
		}
	}

	public void RotateRight(){
		// the shame...
		RotateLeft (); RotateLeft (); RotateLeft ();
		if (!hasBeenRotated) {
			hasBeenRotated = true;
		}
	}

	public void SetShape(Type newShape){
		print ("Set Shape");
		type = newShape;
		switch(newShape){
			case Type.L:
				blocks = new int[,]{
					{0,1,0},
					{0,1,0},
					{0,1,1}
				};
				break;
			case Type.T:
				blocks = new int[,]{
					{1,1,1},
					{0,1,0},
					{0,1,0}
				};
				break;
			case Type.CROSS:
				blocks = new int[,]{
					{0,1,0},
					{1,1,1},
					{0,1,0}
				};
				break;
			case Type.C:
				blocks = new int[,]{
					{0,1,1},
					{0,1,0},
					{0,1,1}
				};
				break;
			case Type.LINE:
				blocks = new int[,]{
					{0,1,0},
					{0,1,0},
					{0,1,0}
				};
				break;
			case Type.S:
				blocks = new int[,]{
					{0,1,0},
					{0,1,1},
					{0,0,1}
				};
				break;
            case Type.Z:
                blocks = new int[,]{
                    {0,1,0},
                    {1,1,0},
                    {1,0,0}
                };
                break;
            case Type.J:
                blocks = new int[,]{
                    {0,1,0},
                    {0,1,0},
                    {1,1,0}
                };
                break;
		case Type.BRIDGE:
			blocks = new int[,] {
				{ 0, 1, 0 },
				{ 0, 1, 0 },
				{ 0, 1, 0 }
			};
				break;

        }
		// Set the prefab that should be used
		if (type == Type.BRIDGE) {
			block = bridgePrefab;
		} else {
			block = blockPrefab;
		}
	}

	public int[,] GetBlocks(){
		return blocks;
	}
}
