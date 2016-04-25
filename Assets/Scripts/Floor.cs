using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	public GameObject blockPrefab;
	public GameObject tetrisPrefab;
	private static int mapSize = 20;
	private int[,] blocks = new int[mapSize, mapSize];


	// Use this for initialization
	void Start () {
		float x = 0;
		float y = 0;
		float z = 0;

		TetrisBlock tb = tetrisPrefab.GetComponent<TetrisBlock>();

        AddTetrisBlock(0,0, tb);
        AddTetrisBlock(1,1, tb);
        AddTetrisBlock(4,4, tb);
        AddTetrisBlock(0,7, tb);

		// absolutely ashamed of this line
		GameObject block = (GameObject) Instantiate(blockPrefab, new Vector3(-1000,-1000,-1000), Quaternion.identity);
		//print(blocks[0].Length);
		for(int i = 0; i < mapSize; i++){
			for(int j = 0; j < mapSize; j++){
				if(blocks[i,j]==1){
					// creating new block
					block = (GameObject) Instantiate(blockPrefab, new Vector3(x,y,z), Quaternion.identity);
				}
				// increment x position
				x += block.GetComponent<Collider>().bounds.size.x;
			}
			x = 0;
			// increment z position
			z += block.GetComponent<Collider>().bounds.size.z;
		}



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
					}
				}
			}
		}
	}

	private bool FormationFits(int row, int col, int[,] formation){
		for(int i = 0; i < formation.GetLength(0); i++){
			for(int j = 0; j < formation.GetLength(1); j++){
				print(formation);
				if(formation[i,j]==1 && blocks[row+i,col+j]==1){
					return false;
				}
			}
		}
		return true;
	}
}
