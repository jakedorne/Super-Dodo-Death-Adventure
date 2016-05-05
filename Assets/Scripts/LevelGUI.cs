﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelGUI : MonoBehaviour {

	public Button lTile;
	public Button tTile;
	public Button crossTile;

	// Use this for initialization
	void Start () {
		lTile = lTile.GetComponent<Button> ();
		tTile = tTile.GetComponent<Button> ();
		crossTile = crossTile.GetComponent<Button> ();
	}

	public void updateGUI(){
		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		LevelManager manager = managerGO.GetComponent<LevelManager> ();

		if (manager.noLTiles <= 0) {
			lTile.interactable = false;
		} else {
			lTile.interactable = true;
		}
		lTile.GetComponentInChildren<Text> ().text = "L:"+manager.noLTiles;

		if (manager.noTTiles <= 0) {
			tTile.interactable = false;
		} else {
			tTile.interactable = true;
		}
		tTile.GetComponentInChildren<Text> ().text = "T:"+manager.noTTiles;

		if (manager.noCrossTiles <= 0) {
			crossTile.interactable = false;
		} else {
			crossTile.interactable = true;
		}
		crossTile.GetComponentInChildren<Text> ().text = "+:"+manager.noCrossTiles;
	}

}