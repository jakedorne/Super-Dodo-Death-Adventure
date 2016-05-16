using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager: MonoBehaviour {

	// attributes of level
	public int levelID;
	public List<TetrisBlock.Shape> blocks;
	public int noDodos;

	public GameObject floor;
	public InventoryUI gui;
	public LevelUI levelgui;

	private int dodoDeathCount;
	private int dodoFinishedCount;

	void Start(){
		gui = gui.GetComponent<InventoryUI> ();
		gui.updateGUI ();

		levelgui = levelgui.GetComponent<LevelUI> ();

		dodoDeathCount = 0;
		dodoFinishedCount = 0;
	}

	public void addTile(TetrisBlock.Shape shape){
		floor.GetComponent<BlockPlacement>().setTetrisShape (shape);
	}

	public void removeTile(TetrisBlock.Shape type){
		blocks.Remove (type);
		gui.updateGUI ();

	}

	// Called everytime a dodo dies
	public void dodoDeath(){
		dodoDeathCount++;
		if (dodoDeathCount + dodoFinishedCount == noDodos) {
			// Finished the level
			levelgui.CompleteLevel();
		}
	}

	// Called everytime a dodo reaches the end point
	public void dodoFinished(){
		dodoFinishedCount++;
		if (dodoDeathCount + dodoFinishedCount == noDodos) {
			// Finished the level
			levelgui.CompleteLevel();
		}
	}

	public void finish(){
		GameManager.finishedLevel(levelID);
	}
}