using UnityEngine;
using System.Collections;

public class LevelManager: MonoBehaviour {

	// attributes of level
	public int levelID;
	public int noLTiles;
	public int noTTiles;
	public int noCrossTiles;
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

	public void addTile(string type){
		TetrisBlock.Shape shape = TetrisBlock.Shape.IDK;
		switch (type) {
		case "L":
			if (noLTiles <= 0) 
				return; 
			shape = TetrisBlock.Shape.L;
			break;
		case "T":
			if (noTTiles <= 0)
				return;
			shape = TetrisBlock.Shape.T;
			break;
		case "+":
			if (noCrossTiles <= 0)
				return;
			shape = TetrisBlock.Shape.CROSS;
			break;
		}
		floor.GetComponent<BlockPlacement>().setTetrisShape (shape);
	}

	public void removeTile(TetrisBlock.Shape type){
		switch (type) {
		case TetrisBlock.Shape.L:
			noLTiles--;
			break;
		case TetrisBlock.Shape.T:
			noTTiles--;
			break;
		case TetrisBlock.Shape.CROSS:
			noCrossTiles--;
			break;
		}
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
