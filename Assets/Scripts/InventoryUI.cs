using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour {

	public Button tileButton;
	Transform blockInventory;
	Transform rockInventory;

	// Use this for initialization
	void Start () {
		tileButton = tileButton.GetComponent<Button> ();
		int children = transform.childCount;
		for (int i = 0; i < children; i++)
		{
			if (transform.GetChild (i).name == "Blocks") {
				blockInventory = transform.GetChild (i);
			} else if (transform.GetChild (i).name == "Rocks") {
				rockInventory = transform.GetChild (i);
			}	
		}
	}

	public void updateInventory(){
		// First remove the inventory that is currently displayed:
		updateRockInventory();
		updateBlockInventory ();
	}

	public void updateRockInventory(){
		LevelManager manager = GameObject.FindObjectOfType<LevelManager>();
		rockInventory.GetChild(0).GetComponent<Text>().text = "" + manager.getNumberOfRocks ();
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
