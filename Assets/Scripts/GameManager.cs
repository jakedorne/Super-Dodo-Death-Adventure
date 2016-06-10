using UnityEngine;
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
			scores [i] = 0; // -1 means that the level has not been played yet
			levelUnlocked [i] = true; // Intialise all levels to locked
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
        if (levelID + 1 < numberOfLevels)
        {
            levelUnlocked[levelID + 1] = true;
        }
        // Send back to home screen
        loadNextLevel();
		//SceneManager.LoadScene("Level Selection");
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
        //For now watermelons have not been implemented, so watermelons are worth 0.
        float watermelonPoints = 0f; //a watermelon will be worth 2.5 dodos
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
        //Outdated level
        int levelNumber = 2;
        int[,] map1 = new int[,]
        {
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        };
        string levelName = "placeholder";
        float bronze = 5;
        float silver = 7;
        float gold = 8;
        int nextLevelNumber = 1;
        int rocksAvailable = 1;
        LevelInfo level1 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level1.init(map1, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
        levelInfo[levelNumber] = level1;
        //-------------------------------------------- Level 2 --------------------------------------------
        //Outdated level
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
        levelName = "placeholder";
        bronze = 5;
        silver = 7;
        gold = 8;
        nextLevelNumber = 1;
        rocksAvailable = 1;
        LevelInfo level2 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level2.init(map2, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
        levelInfo[levelNumber] = level2;
        //-------------------------------------------- Level 3 --------------------------------------------
        //Outdated level
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
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
        };
        levelName = "placeholder";
        bronze = 5;
        silver = 8;
        gold = 9;
        nextLevelNumber = 1;
        rocksAvailable = 1;
        LevelInfo level3 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level3.init(map3, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
        levelInfo[levelNumber] = level3;
		//-------------------------------------------- Level 4 --------------------------------------------
		// Tutorial Level 1, set Level4 for now.
		levelNumber = 5;
		int[,] map4 = new int[,]
		{
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
		};
        levelName = "Tutorial 1";
        bronze = 5;
		silver = 8;
		gold = 9;
        nextLevelNumber = 10;
        rocksAvailable = 1;
        LevelInfo level4 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
		level4.init(map4, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
		levelInfo[levelNumber] = level4;
        //-------------------------------------------- Level 5 --------------------------------------------
        //Stage 1 Level 3
        levelNumber = 6;
        int[,] map5 = new int[,]
        {
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        };
        levelName = "The watermelon sacrifice";
        bronze = 0 + (watermelonPoints * 0);
        silver = -2 + (watermelonPoints * 1);
        gold = -1 + (watermelonPoints * 1);
        nextLevelNumber = 11;
        rocksAvailable = 1;
        LevelInfo level5 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level5.init(map5, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
        levelInfo[levelNumber] = level5;
		//-------------------------------------------- Level 6 --------------------------------------------
		// Tutorial for level 2
		levelNumber = 7;
		int[,] map6 = new int[,]
		{
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0 },
			{ 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0 },
			{ 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
		};
        levelName = "Tutorial 2";
        bronze = 8;
		silver = 9;
		gold = 10;
		nextLevelNumber = 8;
		rocksAvailable = 1;
		LevelInfo level6 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
		level6.init(map6, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
		levelInfo[levelNumber] = level6;
        //-------------------------------------------- Level 7 --------------------------------------------
        //Stage 2 Level 3
        levelNumber = 8;
        int[,] map7 = new int[,]
        {
            { 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0 },
            { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 0, 5, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        };
        levelName = "Options";
        bronze = -3 + (watermelonPoints * 0);
        silver = -4 + (watermelonPoints * 1);
        gold = -2 + (watermelonPoints * 1);
        nextLevelNumber = 14;
        rocksAvailable = 1;
        LevelInfo level7 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level7.init(map7, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
        levelInfo[levelNumber] = level7;
		//-------------------------------------------- Level 8 --------------------------------------------
		// Tutorial for level 3
		levelNumber = 9;
		int[,] map8 = new int[,]
		{
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0 },
			{ 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0 },
			{ 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
		};
        levelName = "Tutorial 3";
		bronze = 8;
		silver = 9;
		gold = 10;
		nextLevelNumber = 1;
		rocksAvailable = 1;
		LevelInfo level8 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
		level8.init(map8, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
		levelInfo[levelNumber] = level8;
        //-------------------------------------------- Level 9 --------------------------------------------
        // Stage 1 Level 2
        levelNumber = 10;
        int[,] map9 = new int[,]
        {
            { 1, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 0, 0, 0, 1, 1, 1, 5, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 3, 0, 0, 0 },
            { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
            { 0, 0, 3, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
        };
        levelName = "Help dumb dodos with rocks";
        bronze = 0 + (watermelonPoints * 0);
        silver = -2 + (watermelonPoints * 1);
        gold = -1 + (watermelonPoints * 1);
        nextLevelNumber = 6;
        rocksAvailable = 4;
        LevelInfo level9 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level9.init(map9, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
        levelInfo[levelNumber] = level9;
        //-------------------------------------------- Level 10 -------------------------------------------
        // Stage 1 Level 4
        levelNumber = 11;
        int[,] map10 = new int[,]
        {
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5 },
            { 1, 1, 1, 0, 0, 0, 0, 3, 3, 3, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 0, 0 },
            { 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 0, 0 },
            { 0, 0, 0, 0, 3, 3, 3, 3, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 3, 3, 3, 3, 0, 0, 0, 0 },
            { 0, 0, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0 },
            { 0, 0, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 3, 3, 3, 0, 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
            { 0, 5, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
        };
        levelName = "Your choice";
        bronze = 0 + (watermelonPoints * 0);
        silver = -1 + (watermelonPoints * 1);
        gold = 0 + (watermelonPoints * 1);
        nextLevelNumber = 12;
        rocksAvailable = 0;
        LevelInfo level10 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level10.init(map10, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
        levelInfo[levelNumber] = level10;
        //-------------------------------------------- Level 11 -------------------------------------------
        // Stage 1 Level 5
        levelNumber = 12;
        int[,] map11 = new int[,]
        {
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 3, 3, 3, 3, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 3, 0, 0, 5, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
        };
        levelName = "Lets just go the easy way";
        bronze = 0 + (watermelonPoints * 0);
        silver = -1 + (watermelonPoints * 1);
        gold = 0 + (watermelonPoints * 1);
        nextLevelNumber = 1;
        rocksAvailable = 0;
        LevelInfo level11 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level11.init(map11, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
        levelInfo[levelNumber] = level11;
        //-------------------------------------------- Level 12 -------------------------------------------
        // Stage 2 Level 2
        levelNumber = 13;
        int[,] map12 = new int[,]
        {
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 1, 0, 5, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
        };
        levelName = "For the greater good";
        bronze = -6 + (watermelonPoints * 1);
        silver = -2 + (watermelonPoints * 0);
        gold = -2 + (watermelonPoints * 1);
        nextLevelNumber = 8;
        rocksAvailable = 1;
        LevelInfo level12 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level12.init(map12, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
        levelInfo[levelNumber] = level12;
        //-------------------------------------------- Level 13 -------------------------------------------
        // Stage 2 Level 4
        levelNumber = 14;
        int[,] map13 = new int[,]
        {
            { 1, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 3, 3, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 3, 3, 3, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 3 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
        };
        levelName = "A directional problem";
        bronze = -3 + (watermelonPoints * 0);
        silver = -2 + (watermelonPoints * 0);
        gold = -2 + (watermelonPoints * 1);
        nextLevelNumber = 15;
        rocksAvailable = 0;
        LevelInfo level13 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level13.init(map13, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
        levelInfo[levelNumber] = level13;
        //-------------------------------------------- Level 14 -------------------------------------------
        // Stage 2 Level 5
        levelNumber = 15;
        int[,] map14 = new int[,]
        {
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 5 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 1, 0 },
            { 3, 0, 0, 5, 0, 0, 0, 0, 0, 1, 0, 0 },
            { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 3 },
            { 0, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
            { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        };
        levelName = "A long journey";
        bronze = -3 + (watermelonPoints * 1);
        silver = -2 + (watermelonPoints * 2);
        gold = -2 + (watermelonPoints * 3);
        nextLevelNumber = 1;
        rocksAvailable = 1;
        LevelInfo level14 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level14.init(map14, bronze, silver, gold, nextLevelNumber, rocksAvailable, levelName);
        levelInfo[levelNumber] = level14;
        //-------------------------------------------- Done --------------------------------------------
    }

    public static int[,] getLevel(int levelNumber){
        return levelInfo[levelNumber].getMap();
	}

    public static float getBronze()
    {
        int levelNumber = SceneManager.GetActiveScene().buildIndex;
        return levelInfo[levelNumber].getBronze();
    }

    public static float getSilver()
    {
        int levelNumber = SceneManager.GetActiveScene().buildIndex;
        return levelInfo[levelNumber].getSilver();
    }

    public static float getGold()
    {
        int levelNumber = SceneManager.GetActiveScene().buildIndex;
        return levelInfo[levelNumber].getGold();
    }

    public static void loadNextLevel(){
        int levelNumber = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(levelInfo[levelNumber].getNextLevelNumber());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

	public void quitRequest(){
		Application.Quit();
	}
}

//// TEMPLATE
/*		
{
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
{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
}; */
