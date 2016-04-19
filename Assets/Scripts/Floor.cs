using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	public GameObject blockPrefab;

	//Matt Wrote this TEST COMMENT
	//private int floorSize = 20;
	private int[][] blocks = new int[][]{	new int[]{0,1,1,1,0},
											new int[]{0,0,1,0,0},
											new int[]{0,0,1,1,0},
											new int[]{0,1,1,0,0},
											new int[]{0,0,1,0,0}}; // maybe of booleans or some sort of block object??
											// is this actually how i have to initialise fucking arrays in c# / unity wtf??


	// Use this for initialization
	void Start () {
		float x = 0;
		float y = 0;
		float z = 0;

		// absolutely ashamed of this line
		GameObject block = (GameObject) Instantiate(blockPrefab, new Vector3(-1000,-1000,-1000), Quaternion.identity);

		for(int i = 0; i < blocks.Length; i++){
			for(int j = 0; j < blocks[i].Length; j++){
				if(blocks[i][j]==1){
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
}
