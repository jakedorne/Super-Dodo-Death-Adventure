﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// current score for each level
	public static int numberOfLevels = 3;
	public static int[] scores;
	public static bool[] levelUnlocked;

	void Awake(){
		scores = new int[numberOfLevels];
		levelUnlocked = new bool[numberOfLevels];
		for (int i = 0; i < numberOfLevels; i++) {
			scores[i] = -1; // -1 means that the level has not been played yet
			levelUnlocked[i] = false; // Intialise all levels to locked
		}
		// Set the first level to unlocked
		levelUnlocked[0] = true;
	}

	public static void finishedLevel(int levelID, int score){
		if (score > scores [levelID]) {
			scores [levelID] = score;
		}
		// Send back to home screen
		SceneManager.LoadScene("Level Selection");
	}

	public void loadLevel(string name){
		Debug.Log("Level load requested for: "+name);
		SceneManager.LoadScene(name);
	}
		
	public void loadNextLevel(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}

	public void quitRequest(){
		Application.Quit();
	}

	// ================================ LEVELS. THIS SHIT'S GONNA BE UGLY ================================ //
	public static int[,] getLevel(int levelNumber){
		switch (levelNumber) {
		case 1:
			return new int[,] {
				{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },

			};	
		case 2:
			return new int[,] {
				{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },

			};
		case 3:
			return new int[,] {
				{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },

			};
		}
		return null;
	}
}