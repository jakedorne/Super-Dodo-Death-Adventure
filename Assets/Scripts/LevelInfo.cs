using UnityEngine;
using System.Collections;

public class LevelInfo : ScriptableObject {

    //The map layout
    private int[,] map;

    //Will probably have the blocks available for the level in here too?

    //Goals for number of dodos left
    private int bronze;
    private int silver;
    private int gold;

    public void init(int[,] map, int bronze, int silver, int gold)
    {
        this.map = map;
        this.bronze = bronze;
        this.silver = silver;
        this.gold = gold;
    }

    public int[,] getMap()
    {
        return map;
    }

    public int getBronze()
    {
        return bronze;
    }

    public int getSilver()
    {
        return silver;
    }

    public int getGold()
    {
        return gold;
    }

}
