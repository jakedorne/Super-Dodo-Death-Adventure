using UnityEngine;
using System.Collections;

public class LevelInfo : ScriptableObject {

    //The map layout
    private int[,] map;

    //Will probably have the blocks available for the level in here too?

    //Goals for number of dodos left
    private float bronze;
    private float silver;
    private float gold;

    private string levelName;

    private int rocksAvailable;

    private int nextLevelNumber; //equals 1 if there is no next level (go back to level select)

    public void init(int[,] map, float bronze, float silver, float gold, int nextLevelNumber, int rocksAvailable, string levelName)
    {
        this.map = map;
        this.bronze = bronze;
        this.silver = silver;
        this.gold = gold;
        this.nextLevelNumber = nextLevelNumber;
        this.rocksAvailable = rocksAvailable;
        this.levelName = levelName;
    }

    public int[,] getMap()
    {
        return map;
    }

    public float getBronze()
    {
        return bronze;
    }

    public float getSilver()
    {
        return silver;
    }

    public float getGold()
    {
        return gold;
    }

    public int getNextLevelNumber()
    {
        return nextLevelNumber;
    }

    public int getRocksAvailable()
    {
        return rocksAvailable;
    }

}
