﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Floor : MonoBehaviour {

	public GameObject blockPrefab;
    public GameObject tree;
	public GameObject tetrisPrefab;
	public GameObject spikePrefab;

	public Spawner spawner;
	public PathFinder pathfinder;

	private float blockXLength;
	private float blockZLength;

	public static float GAME_HEIGHT = 0.35f;

	// indexes in grid
	public int startX;
	public int startZ;
	public int endX;
	public int endZ;

	private static int mapSize;

	// 0 = empty, 1 = block down, 2 = block hovered on (green), 3 = unplaceable space, 4 = tree, 9 = block hovered on (red)
	private int[,] map;
	private Block[,] blocks;


	// Use this for initialization
	void Start () {
		// absolutely ashamed of this line -- is used for finding the length of the blocks..
		GameObject blockObject = (GameObject) Instantiate(blockPrefab, new Vector3(-1000,-1000,-1000), Quaternion.identity);
		spawner = GetComponentInChildren<Spawner> ();
		pathfinder = GameObject.Find ("PathFinder").GetComponent<PathFinder> ();
		print (spawner);

		blockXLength = blockObject.GetComponent<Collider>().bounds.size.x;
		blockZLength = blockObject.GetComponent<Collider>().bounds.size.z;



		map = GameManager.getLevel (SceneManager.GetActiveScene().buildIndex);
        mapSize = map.GetLength(0);
		blocks = new Block[mapSize, mapSize];
		renderMap ();
        this.GetComponent<BlockPlacement>().initialisePlacementBlocks();
    }

	private void renderMap(){
		for(int i = 0; i < map.GetLength(0); i++){
			for(int j = 0; j < map.GetLength(1); j++){
				if (map [i, j] == 1 || map[i,j] == 4) {
					Instantiate(blockPrefab, new Vector3(blockXLength * i, 0, blockZLength * j), Quaternion.identity);
                    if (map[i,j] == 4)
                    {
                        Instantiate(tree, new Vector3((i) * blockXLength, 0 + blockPrefab.GetComponent<MeshRenderer>().bounds.size.y, (j) * blockZLength), Quaternion.identity);
                    }
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
				case TetrisBlock.Type.BRIDGE:
					AddBridgeBlock (row, col, block);
					break;
				default:
					AddBasicBlock (row, col, block);
					break;
			}

			// starts spawning on first block placement
			pathfinder.rebuildTree ();
            if (!spawner.hasStarted())
            {
                spawner.beginSpawning();
            }
            return true;
		}
		return false;
	}

	public void AddBasicBlock(int row, int col, TetrisBlock block){
		int[,] blockFormation = block.GetBlocks ();
		print (blockFormation);
		for(int i = 0; i < blockFormation.GetLength(0); i++){
			for(int j = 0; j < blockFormation.GetLength(1); j++){
				if(blockFormation[i,j]==1){
					map[row + i, col + j] = 1;
					GameObject placedBlock = (GameObject) Instantiate(block.block, new Vector3((row + i) * blockXLength, 1, (col + j) * blockZLength), Quaternion.identity);
					blocks [row + i, col + j] = placedBlock.GetComponent<BasicBlock> ();
                    placedBlock.GetComponent<BasicBlock>().fallToPlace();
				}
			}
		}
	}

	public void AddBridgeBlock(int row, int col, TetrisBlock block){
		int[,] blockFormation = block.GetBlocks ();
		GameObject bridge = (GameObject) Instantiate(block.block, getVectorAtCoords(row + 1,col + 1), Quaternion.identity);
		BreakableBlock script = bridge.GetComponent<BreakableBlock> ();
		script.setPosition (new Vector2 (row + 1, col + 1));

		if (blockFormation [1, 1] == 1 && blockFormation [2, 1] == 1) {
			bridge.transform.Rotate (0, 90, 0);
			script.setOrientation ('v');
		} else {
			script.setOrientation ('h');
		}

		blocks [row + 1, col + 1] = script;

		for(int i = 0; i < blockFormation.GetLength(0); i++){
			for(int j = 0; j < blockFormation.GetLength(1); j++){
				if(blockFormation[i,j]==1){
					map[row + i, col + j] = 1;
				}
			}
		}
	}

	public void updateBlock(Vector2 position){
		Block updatedBlock = blocks [(int)position.x, (int)position.y];
		if (updatedBlock) {
			updatedBlock.interact ();

			if (updatedBlock is BreakableBlock) {
				BreakableBlock bridgeBlock = (BreakableBlock)updatedBlock;
				if (bridgeBlock.getHealth () <= 0) {
					removeBridgeAt (new Vector2 ((int)position.x, (int)position.y));
				}
			}
		}
	}

	public void removeBridgeAt(Vector2 position){
		BreakableBlock block = (BreakableBlock)blocks [(int)position.x, (int)position.y];
		int x = (int)position.x;
		int z = (int)position.y;

		if (block.getOrientation () == 'h') {
			map[x,z] = 0;
			map[x,z+1] = 0;
			map[x,z-1] = 0;
		} else {
			map[x,z] = 0;
			map[x+1,z] = 0;
			map[x-1,z] = 0;
		}
		blocks [x, z] = null;

		pathfinder.rebuildTree ();
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
				} else if (formation[i,j]==1 && (map[row+i,col+j]==1 || map[row+i,col+j]==3 || map[row + i, col + j] == 4)){
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
        map[(int)blockPosition.x, (int)blockPosition.y] = 4;
        Instantiate(tree, new Vector3((x) * blockXLength, 0 + blockPrefab.GetComponent<MeshRenderer>().bounds.size.y, (z) * blockZLength), Quaternion.identity);
		pathfinder.rebuildTree ();
    }

    public void createDeadDodoBlock(Vector2 blockPosition)
    {
        int x = (int)blockPosition.x;
        int z = (int)blockPosition.y;
        if (x>=0 && z>=0 && x<mapSize && z<mapSize && map[x,z]==0)
        {
            this.GetComponent<BlockPlacement>().playPlacementSound();
            map[x, z] = 1; //Eventually change this to its own number perhaps?
            Instantiate(blockPrefab, new Vector3(blockXLength * x, 0, blockZLength * z), Quaternion.identity);
            this.GetComponent<BlockPlacement>().updateOnDodoDeath((int)x,(int)z);
			pathfinder.rebuildTree ();
        }
    }

    public bool isBlock(Vector2 pos)
    {
        int x = (int)pos.x;
        int z = (int)pos.y;
        if (x >= 0 && z >= 0 && x < mapSize && z < mapSize &&map[x,z]==1) return true;
        return false;
    }

    public bool isTree(Vector2 pos)
    {
        int x = (int)pos.x;
        int z = (int)pos.y;
        if (x >= 0 && z >= 0 && x < mapSize && z < mapSize && map[x, z] == 4) return true;
        return false;
    }

}
