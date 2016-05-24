﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// current score for each level
	public static int numberOfLevels = 3;
	public static int[] scores;
	public static bool[] levelUnlocked;

	static bool intialised = false;

	void Awake(){
		if (!intialised) {
			intialise();
		}
	}

	static void intialise(){
		scores = new int[numberOfLevels];
		levelUnlocked = new bool[numberOfLevels];
		for (int i = 0; i < numberOfLevels; i++) {
			scores [i] = -1; // -1 means that the level has not been played yet
			levelUnlocked [i] = false; // Intialise all levels to locked
		}
		// Set the first level to unlocked
		levelUnlocked [0] = true;
        // For now, set level 2 to unlocked too (eventually change this to unlock after completing level 1)
        intialised = true;
	}

	public static void finishedLevel(int levelID, int score){
		if (!intialised) {
			intialise();
		}
		if (score > scores [levelID]) {
			scores [levelID] = score;
		}
        if (levelID+1 < numberOfLevels)
        {
            levelUnlocked[levelID+1] = true;
        }
		// Send back to home screen
		SceneManager.LoadScene("Level Selection");
	}

	public void loadLevel(string name){
		SceneManager.LoadScene(name);
	}

	public static void reloadLevel(){
		Scene scene = SceneManager.GetActiveScene(); 
		SceneManager.LoadScene(scene.name);
	}

	// ================================ LEVELS. THIS SHIT'S GONNA BE UGLY ================================ //
	public static int[,] getLevel(int levelNumber){
		switch (levelNumber) {
		case 2:
			return new int[,] {
                /* for later as a first level
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                */
				{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 3, 3 },
				{ 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 3, 3 },
				{ 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 3, 3 },
				{ 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 3 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
				{ 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 1, 1 },
				{ 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 0, 1 },
                

			};	
		case 3:
			return new int[,] {
                { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },

            };
		case 4:
			return new int[,] {
				{ 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
				{ 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
				{ 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },

			};
		}
		return null;
	}

	public void loadNextLevel(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}

	public void quitRequest(){
		Application.Quit();
	}
}
