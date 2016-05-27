using UnityEngine;
using System.Collections;

public class BasicBlock : MonoBehaviour, Block {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	public virtual bool interact(){
		// do nothing. is basically only an extension of block for consistency..
		return false;
	}
}
