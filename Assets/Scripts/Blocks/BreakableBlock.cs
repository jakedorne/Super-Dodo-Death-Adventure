using UnityEngine;
using System.Collections;

public class BreakableBlock : MonoBehaviour, Block {

	private int health;

	// Use this for initialization
	void Start () {
		health = 3;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Block.interact(){
		health--;
		if (health <= 0) {
			Die ();
		}
	}

	void Die(){
		
	}
}
