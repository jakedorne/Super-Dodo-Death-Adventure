using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour {

	public TetrisBlock.Shape value;
	public int amount;

	// Use this for initialization
	void Start () {
		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		LevelManager manager = managerGO.GetComponent<LevelManager> ();
		// Set face value
		GetComponent<Button> ().transform.GetChild (0).GetComponent<Text> ().text = value.ToString() + ":" + amount;
	}

	public void buttonClicked(){
		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		LevelManager manager = managerGO.GetComponent<LevelManager> ();
		GetComponent<Button> ().interactable = false;
		manager.addTile (value);
	}
}
