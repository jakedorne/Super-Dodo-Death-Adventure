using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour {

	public TetrisBlock.Shape value;

	// Use this for initialization
	void Start () {
		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		LevelManager manager = managerGO.GetComponent<LevelManager> ();

		// Set listener 
		GetComponent<Button> ().onClick.AddListener (() => manager.addTile (value));

		// Set face value
		GetComponent<Button> ().transform.GetChild (0).GetComponent<Text> ().text = value.ToString();
	}
}
