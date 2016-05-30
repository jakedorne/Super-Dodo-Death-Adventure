using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExtinctionMeterUI : MonoBehaviour {

	public float pointerSpeed = 0.20f;

	float decrementConstant; 
	Vector2 pointerDestination;

	void Update(){
		Transform pointer = transform.FindChild ("Pointer");
		Vector2 pointerCurrentPos = new Vector2 (pointer.position.x, pointer.position.z);
		if (pointerCurrentPos != pointerDestination) {
			pointer.transform.position  = Vector2.Lerp(pointer.transform.position, pointerDestination, pointerSpeed);
		}
	}

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

		// Set the current destination as being start position
		pointerDestination = new Vector2 (pointer.position.x, pointer.position.z);
	}

	public void lowerExtinctionCount(){
		Transform pointer = transform.FindChild ("Pointer");
		// pointer.transform.position = new Vector2 ((pointer.position.x - decrementConstant), pointer.position.y);
		LevelManager manager = FindObjectOfType<LevelManager> ();
		Transform endPointer = transform.FindChild ("EndPointer");
		if (manager.noDodosLeft() == 0) {
			pointerDestination = endPointer.transform.position;
		}
		pointerDestination = new Vector2 ((pointer.position.x - decrementConstant), pointer.position.y);
	}


}
