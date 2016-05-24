using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour {

	public TetrisBlock.Shape value;
	public int amount;

	const string UNCLICKED_EXTENSION = "0";
	const string CLICKED_EXTENSION = "1";

	// Use this for initialization
	void Start () {
		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		LevelManager manager = managerGO.GetComponent<LevelManager> ();
		// Set face image
		string filename = value.ToString() + UNCLICKED_EXTENSION;
		Sprite sprite = Resources.Load<Sprite>(filename);
		GetComponent<Image> ().sprite = sprite;
		GetComponent<Button> ().transform.GetChild (0).GetComponent<Text> ().text = "" + amount;
	}

	public void buttonClicked(){
		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		LevelManager manager = managerGO.GetComponent<LevelManager> ();
		string filename = value.ToString() + CLICKED_EXTENSION;
		Sprite sprite = Resources.Load<Sprite>(filename);
		GetComponent<Image> ().sprite = sprite;
		manager.addTile (value);
	}
		
}
