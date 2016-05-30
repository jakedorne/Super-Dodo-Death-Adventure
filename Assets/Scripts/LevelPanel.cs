using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour {

	public Button button;
	public Text label;
	public int levelNumber;

	// Use this for initialization
	void Start () {
		button = button.GetComponent<Button> ();
	}

	public void setlevelUnlocked(bool value){
		button.interactable = value;
	}

	public void setScore(int score){
		// label.text = "Level " + levelNumber + ":" + score;
	}

}
