using UnityEngine;
using System.Collections;

public class JumpBlock : MonoBehaviour, Block {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual bool interact(){
		return false;
	}
}
