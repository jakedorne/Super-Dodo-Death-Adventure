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
		
	public virtual bool interact(){
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
}
