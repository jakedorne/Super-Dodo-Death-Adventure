using UnityEngine;
using System.Collections;

public class BreakableBlock : Block {

	private int health;
	private TetrisBlock tetrisBlock;
	private Vector2 position;
	private char orientation;

	// Use this for initialization
	void Start () {
		health = 3;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	public override bool interact(){
		print ("Interact called");
		health--;
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
}
