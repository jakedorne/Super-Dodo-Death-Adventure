using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour {

	public Button tileButton;

	// Use this for initialization
	void Start () {
		tileButton = tileButton.GetComponent<Button> ();
	}

	public void updateInventory(){
		// First remove the inventory that is currently displayed:
		int childs = transform.childCount;
		for (int i = 0; i < childs; i++)
		{
			GameObject.Destroy(transform.GetChild(i).gameObject);
		}

		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		LevelManager manager = managerGO.GetComponent<LevelManager> ();
		List<TetrisBlock.Shape> blocks = manager.blocks;

		Dictionary<TetrisBlock.Shape, int> formattedInventory = new Dictionary<TetrisBlock.Shape, int> ();
		foreach(TetrisBlock.Shape block in blocks){
			if (!formattedInventory.ContainsKey (block)) {
				formattedInventory.Add (block, 1);
			} else {
				int amount = formattedInventory[block];
				formattedInventory [block] = amount + 1;
			}
		}

		foreach (TetrisBlock.Shape key in formattedInventory.Keys) {
			int amount = formattedInventory [key];
			Button button = Instantiate (tileButton);
			button.transform.SetParent (transform, false);
			button.GetComponent<InventoryButton> ().value = key;
			button.GetComponent<InventoryButton> ().amount = amount;
		}

	}

}
