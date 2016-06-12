using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {

	public RectTransform completeLevel;
	SidePanelUI inventoryUI;
	ExtinctionMeterUI extinctionMeterUI;

	// Use this for initialization
	void Awake () {
		completeLevel = completeLevel.GetComponent<RectTransform> ();
		inventoryUI = transform.FindChild ("SideGUI").GetComponent<SidePanelUI>();
		//extinctionMeterUI = transform.FindChild ("ExtinctionMeter").GetComponent<ExtinctionMeterUI>();
	}

	public void CompleteLevel(int score, int numDodos){
		RectTransform comLevel =  Instantiate (completeLevel);
		Transform scoreLabel = comLevel.FindChild ("Score");
        // The trophy info should probably be stored in a seperate label, but for now I put it in the score one.
        string filename = "";
        print(score + ", " + numDodos + ", " + GameManager.getGold());
		if (score >= (numDodos + GameManager.getGold ()) * 10) {
			filename = "fm_gold";
		} else if (score >= (numDodos + GameManager.getSilver ()) * 10) {
			filename = "fm_silver";
		} else if (score >= (numDodos + GameManager.getBronze ()) * 10) {
			filename = "fm_bronze";
		} else {
			filename = "fm_poop";
		}
		Sprite sprite = Resources.Load<Sprite>(filename);
		comLevel.GetComponent<Image> ().sprite = sprite;
		scoreLabel.GetComponent<Text>().text = ""+score;
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
