using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExtinctionMeterUI : MonoBehaviour {

	float decrementConstant; 

	void Start(){
		// Set the position of the pointer
		Transform pointer = transform.FindChild ("Pointer");
		Transform endPointer = transform.FindChild ("EndPointer");

		// hide the end pointer
		endPointer.gameObject.SetActive(false);

		// Figure out the incremental drop for each dodo death
		float difference = pointer.position.x - endPointer.position.x;
		LevelManager manager = FindObjectOfType<LevelManager> ();

		decrementConstant = difference / manager.noDodos;
	}

	public void lowerExtinctionCount(){
		Transform pointer = transform.FindChild ("Pointer");
		pointer.transform.position = new Vector2 ((pointer.position.x - decrementConstant), pointer.position.y);
	}


}
