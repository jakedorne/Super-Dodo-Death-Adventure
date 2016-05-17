using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelsMenu : MonoBehaviour {

	public LevelPanel[] levels; 

	void Start () {
		// Every time start is called, need to update gui to match users progress
		int[] scores = GameManager.scores;
		bool[] levelsUnlocked = GameManager.levelUnlocked;

		// Asumption here is that size of scores and levelsUnlocked is equal to number of levelPanels
		for(int i = 0; i < levels.Length; i++){

			LevelPanel panel = levels [i].GetComponent<LevelPanel> ();

			panel.setlevelUnlocked ( levelsUnlocked[i] );

			if (scores [i] != -1) {
				panel.setScore (scores [i]);
			}

		}
	
	}
}
