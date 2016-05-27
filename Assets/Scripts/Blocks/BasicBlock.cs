using UnityEngine;
using System.Collections;

public class BasicBlock : Block {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	public override bool interact(){
		// do nothing. is basically only an extension of block for consistency..
		return false;
	}
}
