using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Floor : MonoBehaviour {

	public GameObject blockPrefab;
    public GameObject tree;
	public GameObject tetrisPrefab;
	public GameObject spikePrefab;

	private float blockXLength;
	private float blockZLength;

	public static float GAME_HEIGHT = 0.35f;

	// indexes in grid
	public int startX;
	public int startZ;
	public int endX;
	public int endZ;

	private static int mapSize;

	// 0 = empty, 1 = block down, 2 = block hovered on (green), 3 = unplaceable space, 9 = block hovered on (red)
	private int[,] blocks;


	// Use this for initialization
	void Start () {
		// absolutely ashamed of this line -- is used for finding the length of the blocks..
		GameObject blockObject = (GameObject) Instantiate(blockPrefab, new Vector3(-1000,-1000,-1000), Quaternion.identity);

		blockXLength = blockObject.GetComponent<Collider>().bounds.size.x;
		blockZLength = blockObject.GetComponent<Collider>().bounds.size.z;

		blocks = GameManager.getLevel (SceneManager.GetActiveScene().buildIndex);
        mapSize = blocks.GetLength(0);
		renderMap ();
	}

	private void renderMap(){
		for(int i = 0; i < blocks.GetLength(0); i++){
			for(int j = 0; j < blocks.GetLength(1); j++){
				if (blocks [i, j] == 1) {
					Instantiate(blockPrefab, new Vector3(blockXLength * i, 0, blockZLength * j), Quaternion.identity);
				} 
				else if(blocks[i,j]==3){
					Instantiate(spikePrefab, new Vector3(blockXLength * i, -2f, blockZLength * j), Quaternion.identity);
				}
			}
		}
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
                        if (block.shape == TetrisBlock.Shape.TREE)
                        {
                            blocks[row + i, col + j] = 3;
                            Instantiate(blockPrefab, new Vector3((row + i) * blockXLength, 0, (col + j) * blockZLength), Quaternion.identity);
                            Instantiate(tree, new Vector3((row + i) * blockXLength, 0 + blockPrefab.GetComponent<MeshRenderer>().bounds.size.y, (col + j) * blockZLength), Quaternion.identity);
                        } else {
                            blocks[row + i, col + j] = 1;
                            Instantiate(blockPrefab, new Vector3((row + i) * blockXLength, 0, (col + j) * blockZLength), Quaternion.identity);
                        }

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
	public bool FormationFits(int row, int col, int[,] formation){
		for(int i = 0; i < formation.GetLength(0); i++){
			for(int j = 0; j < formation.GetLength(1); j++){
                //Debug.Log("row: " + row + ", col: " + col + ", i: " + i + ", j: " + j);
				if ((row + i >= blocks.GetLength(0) || row + i < 0) && formation [i, j] == 1) {
					// part of block is off map
					return false;
				} else if ((col + j >= blocks.GetLength(1) || col + j < 0) && formation [i, j] == 1) {
					// part of block is off map
					return false;
				} else if (formation[i,j]==1 && (blocks[row+i,col+j]==1 || blocks[row+i,col+j]==3)){
					// part of block is on another block or unplaceable spot
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
		return new Vector3(x * blockXLength, GAME_HEIGHT , z * blockZLength);
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
		return new Vector2(Mathf.Round(vector.x / blockXLength), Mathf.Round(vector.z / blockZLength));
    }

    public void createObstacle(Vector2 blockPosition) {
        float x = blockPosition.x;
        float z = blockPosition.y;
        blocks[(int)blockPosition.x, (int)blockPosition.y] = 3;
        Instantiate(tree, new Vector3((x) * blockXLength, 0 + blockPrefab.GetComponent<MeshRenderer>().bounds.size.y, (z) * blockZLength), Quaternion.identity);
    }

}
