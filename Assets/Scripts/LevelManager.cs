using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager: MonoBehaviour {

	// attributes of level
	public int levelID;
	public List<TetrisBlock.Shape> blocks;
	public int noDodos;

	public GameObject floor;
	public LevelGUI gui;

	private int dodoDeathCount;
	private int dodoFinishedCount;

	void Start(){
		gui = gui.GetComponent<LevelGUI> ();
		gui.updateGUI ();

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
			finish();
		}
	}

	// Called everytime a dodo reaches the end point
	public void dodoFinished(){
		dodoFinishedCount++;
		if (dodoDeathCount + dodoFinishedCount == noDodos) {
			// Finished the level
			finish();
		}
	}

	public void finish(){
		int score = 50; // Just a placeholder
		GameManager.finishedLevel(levelID, score);
	}
}