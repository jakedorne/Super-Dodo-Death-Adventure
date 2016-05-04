using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	public GameObject blockPrefab;
	public GameObject tetrisPrefab;

	private TetrisBlock tetrisBlock;
	private GameObject blockObject;

	private float blockXLength;
	private float blockZLength;

	// indexes in grid
	public int startX;
	public int startZ;
	public int endX;
	public int endZ;

	private static int mapSize = 20;
	private int[,] blocks = new int[mapSize, mapSize];


	// Use this for initialization
	void Start () {
		tetrisBlock = tetrisPrefab.GetComponent<TetrisBlock>();
		// absolutely ashamed of this line -- is used for finding the length of the blocks..
		blockObject = (GameObject) Instantiate(blockPrefab, new Vector3(-1000,-1000,-1000), Quaternion.identity);

		blockXLength = blockObject.GetComponent<Collider>().bounds.size.x;
		blockZLength = blockObject.GetComponent<Collider>().bounds.size.z;

		// testing random shapes and shit
        AddTetrisBlock(0,0, tetrisBlock);
        tetrisBlock.SetShape(TetrisBlock.Shape.CROSS);
        AddTetrisBlock(4,0, tetrisBlock);
        tetrisBlock.SetShape(TetrisBlock.Shape.T);
        AddTetrisBlock(0,7, tetrisBlock);
		tetrisBlock.Rotate();
        AddTetrisBlock(8,0, tetrisBlock);
	}

	/// <summary>
	/// Adds tetris block at index on floor and returns true if successfully placed.
	/// </summary>
	public bool AddTetrisBlock(int row, int col, TetrisBlock block){
		int[,] blockFormation = block.GetBlocks();
		if(FormationFits(row, col, blockFormation)){
			for(int i = 0; i < blockFormation.GetLength(0); i++){
				for(int j = 0; j < blockFormation.GetLength(1); j++){
					if(blockFormation[i,j]==1){
						blocks[row+i,col+j]=1;
						Instantiate(blockPrefab, new Vector3((row + i) * blockXLength, 0, (col + j) * blockZLength), Quaternion.identity);
					}
				}
			}
			return true;
		}
		return false;
	}


	/// <summary>
	/// Returns true if a tetris block formation can be placed at an index.
	/// </summary>
	private bool FormationFits(int row, int col, int[,] formation){
		for(int i = 0; i < formation.GetLength(0); i++){
			for(int j = 0; j < formation.GetLength(1); j++){
				if ((row + i >= mapSize || row + i < 0) && formation [i, j] == 1) {
					// part of block is off map
					return false;
				} else if ((col + j >= mapSize || col + j < 0) && formation [i, j] == 1) {
					// part of block is off map
					return false;
				} else if (formation[i,j]==1 && blocks[row+i,col+j]==1){
					// part of block is on another block
					return false;
				}
			}
		}
		return true;
	}

    public int[,] getFloor()
    {
        return blocks;
    }

	/// <summary>
	/// Returns the world position of an index on the floor.
	/// </summary>
    public Vector3 getVectorAtCoords(int x, int z){
    	return new Vector3(x * blockXLength + blockXLength/2, 0 , z * blockZLength + blockZLength/2);
    }

	/// <summary>
	/// Returns true if there is a block placed on the given index of floor
	/// </summary>
    public bool positionOnBlock(int x, int z){
    	if(x >= 0 && x < blocks.GetLength(0) && z >= 0 && z < blocks.GetLength(1)){
    		return blocks[x, z] == 1;
    	} else {
    		return false;
    	}
    }

	/// <summary>
	/// Returns the x and z index of the floor given a world position.
	/// </summary>
    public Vector2 getCoordAtVector(Vector3 vector){
		return new Vector2((vector.x - blockXLength) / blockXLength, (vector.z- blockZLength) / blockZLength);
    }

}
