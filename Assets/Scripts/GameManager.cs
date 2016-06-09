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
        float watermelonPoints = 2.5f; //a watermelon is worth 2.5 dodos
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
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        };
        float bronze = 5;
        float silver = 7;
        float gold = 8;
        int nextLevelNumber = 6;
        int rocksAvailable = 1;
        LevelInfo level1 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level1.init(map1, bronze, silver, gold, nextLevelNumber, rocksAvailable);
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
        nextLevelNumber = 1;
        rocksAvailable = 1;
        LevelInfo level2 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level2.init(map2, bronze, silver, gold, nextLevelNumber, rocksAvailable);
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
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
        };
        bronze = 5;
        silver = 8;
        gold = 9;
        nextLevelNumber = 1;
        rocksAvailable = 1;
        LevelInfo level3 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level3.init(map3, bronze, silver, gold, nextLevelNumber, rocksAvailable);
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
		bronze = 5;
		silver = 8;
		gold = 9;
        nextLevelNumber = 1;
        rocksAvailable = 1;
        LevelInfo level4 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
		level4.init(map4, bronze, silver, gold, nextLevelNumber, rocksAvailable);
		levelInfo[levelNumber] = level4;
        //-------------------------------------------- Level 5 --------------------------------------------
        levelNumber = 6;
        int[,] map5 = new int[,]
        {
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
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
        bronze = 8;
        silver = 9;
        gold = 10;
        nextLevelNumber = 1;
        rocksAvailable = 1;
        LevelInfo level5 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
        level5.init(map5, bronze, silver, gold, nextLevelNumber, rocksAvailable);
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
		bronze = 8;
		silver = 9;
		gold = 10;
		nextLevelNumber = 1;
		rocksAvailable = 1;
		LevelInfo level6 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
		level6.init(map6, bronze, silver, gold, nextLevelNumber, rocksAvailable);
		levelInfo[levelNumber] = level6;
		//-------------------------------------------- Level 7 --------------------------------------------
		// Tutorial for level 3
		levelNumber = 8;
		int[,] map7 = new int[,]
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
		bronze = 8;
		silver = 9;
		gold = 10;
		nextLevelNumber = 1;
		rocksAvailable = 1;
		LevelInfo level7 = (LevelInfo)ScriptableObject.CreateInstance("LevelInfo");
		level7.init(map7, bronze, silver, gold, nextLevelNumber, rocksAvailable);
		levelInfo[levelNumber] = level7;
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
