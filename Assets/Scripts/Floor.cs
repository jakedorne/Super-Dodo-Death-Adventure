﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Floor : MonoBehaviour {

	public GameObject blockPrefab;
    public GameObject tree;
	public GameObject tetrisPrefab;
	public GameObject spikePrefab;

	public Spawner spawner;

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
	private int[,] map;
	private Block[,] blocks;


	// Use this for initialization
	void Start () {
		// absolutely ashamed of this line -- is used for finding the length of the blocks..
		GameObject blockObject = (GameObject) Instantiate(blockPrefab, new Vector3(-1000,-1000,-1000), Quaternion.identity);
		spawner = GetComponentInChildren<Spawner> ();
		print (spawner);

		blockXLength = blockObject.GetComponent<Collider>().bounds.size.x;
		blockZLength = blockObject.GetComponent<Collider>().bounds.size.z;

		map = GameManager.getLevel (SceneManager.GetActiveScene().buildIndex);
        mapSize = map.GetLength(0);
		renderMap ();
	}

	private void renderMap(){
		for(int i = 0; i < map.GetLength(0); i++){
			for(int j = 0; j < map.GetLength(1); j++){
				if (map [i, j] == 1) {
					Instantiate(blockPrefab, new Vector3(blockXLength * i, 0, blockZLength * j), Quaternion.identity);
				} 
				else if(map[i,j]==3){
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
			switch (block.type) {
				case TetrisBlock.Type.BASIC:
						print ("basic case");
						AddBasicBlock (row, col, block);
						break;
				case TetrisBlock.Type.BRIDGE:
						print ("bridge case");
						AddBridgeBlock (row, col, block);
						break;
			}

			// starts spawning on first block placement
			if (!spawner.hasStarted ()) {
				spawner.beginSpawning ();
			}
			return true;
		}
		return false;
	}

	public void AddBasicBlock(int row, int col, TetrisBlock block){
		print ("Coords: " + row + ", " + col);
		int[,] blockFormation = block.GetBlocks ();
		for(int i = 0; i < blockFormation.GetLength(0); i++){
			for(int j = 0; j < blockFormation.GetLength(1); j++){
				if(blockFormation[i,j]==1){
					map[row + i, col + j] = 1;
					GameObject placedBlock = (GameObject) Instantiate(block.block, new Vector3((row + i) * blockXLength, 0, (col + j) * blockZLength), Quaternion.identity);
					//blocks [row + i, col + j] = placedBlock.GetComponent<BasicBlock> ();
				}
			}
		}
	}

	public void AddBridgeBlock(int row, int col, TetrisBlock block){

		print ("adding bridge block");
		int[,] blockFormation = block.GetBlocks ();
		GameObject bridge;
		bridge = (GameObject) Instantiate(block.block, getVectorAtCoords(row + 1,col + 1), Quaternion.identity);

		if (blockFormation [1,1] == 1 && blockFormation[2,1]==1) {
			bridge.transform.Rotate (0, 90, 0);
		}

		for(int i = 0; i < blockFormation.GetLength(0); i++){
			for(int j = 0; j < blockFormation.GetLength(1); j++){
				if(blockFormation[i,j]==1){
					map[row + i, col + j] = 1;
					//blocks [row + i, col + j] = bridge.GetComponent<BreakableBlock>();
				}
			}
		}
	}


	/// <summary>
	/// Returns true if a tetris block formation can be placed at an index.
	/// </summary>
	public bool FormationFits(int row, int col, int[,] formation){
		for(int i = 0; i < formation.GetLength(0); i++){
			for(int j = 0; j < formation.GetLength(1); j++){
                //Debug.Log("row: " + row + ", col: " + col + ", i: " + i + ", j: " + j);
				if ((row + i >= map.GetLength(0) || row + i < 0) && formation [i, j] == 1) {
					// part of block is off map
					return false;
				} else if ((col + j >= map.GetLength(1) || col + j < 0) && formation [i, j] == 1) {
					// part of block is off map
					return false;
				} else if (formation[i,j]==1 && (map[row+i,col+j]==1 || map[row+i,col+j]==3)){
					// part of block is on another block or unplaceable spot
					return false;
				}
			}
		}
		return true;
	}

    public int[,] getFloor()
    {
        return map;
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
    	if(x >= 0 && x < map.GetLength(0) && z >= 0 && z < map.GetLength(1)){
    		return map[x, z] == 1;
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
        map[(int)blockPosition.x, (int)blockPosition.y] = 3;
        Instantiate(tree, new Vector3((x) * blockXLength, 0 + blockPrefab.GetComponent<MeshRenderer>().bounds.size.y, (z) * blockZLength), Quaternion.identity);
    }

}
