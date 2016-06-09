using UnityEngine;
using System.Collections;

public class BreakableBlock : Block {

	private int health;
	private TetrisBlock tetrisBlock;
	private Vector2 position;
	private char orientation;

	public GameObject bridge;
	public GameObject bridge1;
	public GameObject bridge2;
	public GameObject bridge3;

	public Mesh[] meshes = new Mesh[4];

	// Use this for initialization
	void Start () {
		health = 3;
		meshes [0] = bridge3.GetComponent<MeshFilter> ().sharedMesh;
		meshes [1] = bridge2.GetComponent<MeshFilter> ().sharedMesh;
		meshes [2] = bridge1.GetComponent<MeshFilter> ().sharedMesh;
		meshes [3] = bridge.GetComponent<MeshFilter> ().sharedMesh;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	public override bool interact(){
		print ("Interact called");
		health--;
		gameObject.GetComponent<MeshFilter> ().mesh = meshes [health]; 
		if (health <= 0) {
			Die ();
			return true;
		} 
		return false;
	}

	void Die(){
		Destroy (gameObject);
	}

	public int getHealth(){
		return this.health;
	}

	public void setOrientation(char c){
		this.orientation = c;
	}
		
	public void setPosition(Vector2 position){
		this.position = position;
	}

	public char getOrientation(){
		return orientation;
	}

	public Vector2 getPosition(){
		return this.position;
	}

	public void setParent(TetrisBlock tetris){
		print ("parent set");
		this.tetrisBlock = tetris;
	}
}
