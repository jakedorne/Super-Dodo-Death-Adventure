using UnityEngine;
using System.Collections;

public class WatermelonRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAroundLocal(new Vector3(0,10,0),0.03f);
	}
}
