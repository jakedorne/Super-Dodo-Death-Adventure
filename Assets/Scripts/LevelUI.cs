using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {

	public RectTransform completeLevel;
	InventoryUI inventoryUI;
	ExtinctionMeterUI extinctionMeterUI;

	// Use this for initialization
	void Awake () {
		completeLevel = completeLevel.GetComponent<RectTransform> ();
		inventoryUI = transform.FindChild ("Inventory").GetComponent<InventoryUI>();
		extinctionMeterUI = transform.FindChild ("ExtinctionMeter").GetComponent<ExtinctionMeterUI>();
	}

	public void CompleteLevel(int score, int numDodos){
		RectTransform comLevel =  Instantiate (completeLevel);
		Transform scoreLabel = comLevel.FindChild ("Score");
        // The trophy info should probably be stored in a seperate label, but for now I put it in the score one.
        string trophy = "You got no trophy.";
        print(score + ", " + numDodos + ", " + GameManager.getGold());
        if (score > (numDodos + GameManager.getGold()) * 10)
        {
            trophy = "You got Platinum!";
        }
        else if (score == (numDodos + GameManager.getGold()) * 10)
        {
            trophy = "You got Gold!";
        }
        else if (score >= (numDodos + GameManager.getSilver()) *10)
        {
            trophy = "You got Silver!";
        }
        else if (score >= (numDodos + GameManager.getBronze()) *10)
        {
            trophy = "You got Bronze!";
        }
        scoreLabel.GetComponent<Text>().text = "Score: " + score + "\n " + trophy;
        comLevel.transform.SetParent (transform, false);
	}

	public void updateInventory(){
		inventoryUI.updateInventory ();
	}

	public void lowerExtinctionCount (){
		extinctionMeterUI.lowerExtinctionCount ();
	}

	public void tetrisBlockSelected(TetrisBlock.Type shapeSelected){
		inventoryUI.tetrisBlockSelected (shapeSelected);
	}

	public void deselectBlocks(){
		inventoryUI.deselectBlocks();
	}
}
