using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public void reloadLevel(){
		GameManager.reloadLevel ();
	}

	public void levelSelection(){
		GameManager.startMenu ();
	}

	public void close(){
		FindObjectOfType<LevelUI> ().pauseButtonClicked ();
	}

}
