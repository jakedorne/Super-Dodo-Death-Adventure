﻿using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public void reloadLevel(){
		GameManager.reloadLevel ();
	}

	public void levelSelection(){
		GameManager.levelSelection ();
	}

	public void close(){
		FindObjectOfType<LevelUI> ().resume ();
		FindObjectOfType<LevelManager> ().pause ();
		Destroy (this.gameObject);
	}

}
