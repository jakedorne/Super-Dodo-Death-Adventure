﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	// current score for each level
	public static int numberOfLevels = 3;
	public static int[] scores;
	public static bool[] levelUnlocked;

    private static Dictionary<int,LevelInfo> levelInfo;

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
		intialised = true;
	}

	public static void finishedLevel(int levelID, int score){
		if (!intialised) {
			intialise();
		}
		if (score > scores [levelID]) {
			scores [levelID] = score;
		} 
        if (levelID + 1 < numberOfLevels)
        {
            levelUnlocked[levelID + 1] = true;
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
    public static void setUpLevels()
    {
        levelInfo = new Dictionary<int,LevelInfo>();
                /* Original level we tested with
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
                */
        //-------------------------------------------- Level 1 --------------------------------------------
        int levelNumber = 2;
        int[,] map1 = new int[,]
        {
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        };
        int bronze = 5;
        int silver = 7;
        int gold = 8;
        LevelInfo level1 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level1.init(map1, bronze, silver, gold);
        levelInfo[levelNumber] = level1;
        //-------------------------------------------- Level 2 --------------------------------------------
        levelNumber = 3;
        int[,] map2 = new int[,]
        {
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 3, 3, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 3, 3, 0, 1, 1, 0, 1, 1, 0 },
                { 0, 0, 0, 3, 3, 1, 1, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
        };
        bronze = 5;
        silver = 7;
        gold = 8;
        LevelInfo level2 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level2.init(map2, bronze, silver, gold);
        levelInfo[levelNumber] = level2;
        //-------------------------------------------- Level 3 --------------------------------------------
        levelNumber = 4;
        int[,] map3 = new int[,]
        {
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
        };
        bronze = 5;
        silver = 8;
        gold = 9;
        LevelInfo level3 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level3.init(map3, bronze, silver, gold);
        levelInfo[levelNumber] = level3;
        //-------------------------------------------- Done --------------------------------------------
    }

    public static int[,] getLevel(int levelNumber){
        return levelInfo[levelNumber].getMap();
	}

    public static int getBronze()
    {
        int levelNumber = SceneManager.GetActiveScene().buildIndex;
        return levelInfo[levelNumber].getBronze();
    }

    public static int getSilver()
    {
        int levelNumber = SceneManager.GetActiveScene().buildIndex;
        return levelInfo[levelNumber].getSilver();
    }

    public static int getGold()
    {
        int levelNumber = SceneManager.GetActiveScene().buildIndex;
        return levelInfo[levelNumber].getGold();
    }

    public void loadNextLevel(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}

	public void quitRequest(){
		Application.Quit();
	}
}
