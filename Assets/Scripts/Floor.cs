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
	
	// Update is called once per frame
	void Update () {

	}

	public void AddTetrisBlock(int row, int col, TetrisBlock block){
		int[,] blockFormation = block.GetBlocks();
		print(blockFormation);
		if(FormationFits(row, col, blockFormation)){
			for(int i = 0; i < blockFormation.GetLength(0); i++){
				for(int j = 0; j < blockFormation.GetLength(1); j++){
					if(blockFormation[i,j]==1){
						blocks[row+i,col+j]=1;
						Instantiate(blockPrefab, new Vector3((row + i) * blockXLength, 0, (col + j) * blockZLength), Quaternion.identity);
					}
				}
			}
		}
	}

	private bool FormationFits(int row, int col, int[,] formation){
		for(int i = 0; i < formation.GetLength(0); i++){
			for(int j = 0; j < formation.GetLength(1); j++){
				print(formation);
				if(row + i >= mapSize || col + j >= mapSize || (formation[i,j]==1 && blocks[row+i,col+j]==1)){
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

    public Vector3 getVectorAtCoords(int row, int col){
    	return new Vector3(row * blockXLength , 0 , col * blockZLength);
    }
}
