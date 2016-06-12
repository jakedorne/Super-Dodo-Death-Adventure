using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SidePanelUI : MonoBehaviour {

	public Button tileButton;
	Transform blockInventory;
	Transform rockInventory;
	Transform liveDodoCount;
	Transform levelName;
	Transform watermelonCount;

	// Use this for initialization
	void Awake () {
		tileButton = tileButton.GetComponent<Button> ();
		int children = transform.childCount;
		for (int i = 0; i < children; i++)
		{
			Transform child = transform.GetChild (i);
			if (child.name == "Blocks") {
				blockInventory = child;
			} else if (child.name == "RockCount") {
				rockInventory = child;
			} else if (child.name == "LiveDodoCount") {
				liveDodoCount = child;
			} else if (child.name == "LevelName") {
				levelName = child;
			} else if (child.name == "WatermelonCount") {
				watermelonCount = child;
			}
		}
	}

	public void updateInventory(){
		// First remove the inventory that is currently displayed:
		updateRockInventory();
		updateBlockInventory ();
		updateDodoCounts ();
		// Set level title
		LevelManager manager = GameObject.FindObjectOfType<LevelManager>();
		levelName.GetComponent<Text> ().text = manager.getLevelName ();
		// Set watermelonCount
		watermelonCount.GetComponent<Text>().text = "" + manager.getNumberOfWatermelon();
	}

	public void updateDodoCounts(){
		LevelManager manager = GameObject.FindObjectOfType<LevelManager>();
		liveDodoCount.GetComponent<Text>().text = "" + (manager.noDodos - manager.getDodoDeathCount());
	}

	public void updateRockInventory(){
		LevelManager manager = GameObject.FindObjectOfType<LevelManager>();
		rockInventory.GetComponent<Text>().text = "" + manager.getNumberOfRocks ();
	}
		
	public void updateBlockInventory(){
		int children = blockInventory.childCount;
		for (int i = 0; i < children; i++)
		{
			GameObject.Destroy(blockInventory.GetChild(i).gameObject);
		}

		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		LevelManager manager = managerGO.GetComponent<LevelManager> ();
		List<TetrisBlock.Type> blocks = manager.blocks;

		Dictionary<TetrisBlock.Type, int> formattedInventory = new Dictionary<TetrisBlock.Type, int> ();
		foreach(TetrisBlock.Type block in blocks){
			if (!formattedInventory.ContainsKey (block)) {
				formattedInventory.Add (block, 1);
			} else {
				int amount = formattedInventory[block];
				formattedInventory [block] = amount + 1;
			}
		}

		foreach (TetrisBlock.Type key in formattedInventory.Keys) {
			int amount = formattedInventory [key];
			Button button = Instantiate (tileButton);
			button.transform.SetParent (blockInventory, false);
			button.GetComponent<InventoryButton> ().value = key;
			button.GetComponent<InventoryButton> ().amount = amount;
		}
	}

	public void tetrisBlockSelected(TetrisBlock.Type shapeSelected){
		int childs = blockInventory.childCount;
		for (int i = 0; i < childs; i++)
		{
			InventoryButton tileButton = blockInventory.GetChild (i).GetComponent<InventoryButton> ();
			if (tileButton.value == shapeSelected) {
				tileButton.buttonClicked ();
				return;
			}
		}
	}

	public void deselectBlocks(){
		int childs = blockInventory.childCount;
		for (int i = 0; i < childs; i++)
		{
			InventoryButton tileButton = blockInventory.GetChild (i).GetComponent<InventoryButton> ();
			tileButton.unselectButton ();
		}
	}

}
