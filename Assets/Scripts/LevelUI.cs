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

	public void CompleteLevel(int score){
		RectTransform comLevel =  Instantiate (completeLevel);
		Transform scoreLabel = comLevel.FindChild ("Score");
		scoreLabel.GetComponent<Text> ().text = "Score: " + score;
		comLevel.transform.SetParent (transform, false);
	}

	public void updateInventory(){
		inventoryUI.updateInventory ();
	}

	public void lowerExtinctionCount (){
		extinctionMeterUI.lowerExtinctionCount ();
	}
}
