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
	private bool levelCompleted;
	private int score;

	void Start(){
		gui = gui.GetComponent<InventoryUI> ();

		gui.updateInventory ();

		levelgui = levelgui.GetComponent<LevelUI> ();

		dodoDeathCount = 0;
		dodoFinishedCount = 0;

		levelCompleted = false;
	}

	void Update(){
		// For testing purposes
		if (Input.GetKeyDown (KeyCode.K)) {
			dodoDeath ();
		}
		if(Input.GetKeyDown(KeyCode.L)){
			dodoFinished();
		}

		if (Input.GetMouseButtonDown (0) && levelCompleted) {
			GameManager.finishedLevel(levelID, score);
		}
	}

	public void addTile(TetrisBlock.Shape shape){
		floor.GetComponent<BlockPlacement>().setTetrisShape (shape);
	}

	public void removeTile(TetrisBlock.Shape type){
		blocks.Remove (type);
		gui.updateInventory ();

	}

	// Called everytime a dodo dies
	public void dodoDeath(){
		dodoDeathCount++;
		if (dodoDeathCount + dodoFinishedCount == noDodos) {
			// Finished the level
			calculateScore();
			levelCompleted = true;
			levelgui.CompleteLevel(score);
		}
	}

	// Called everytime a dodo reaches the end point
	public void dodoFinished(){
		dodoFinishedCount++;
		if (dodoDeathCount + dodoFinishedCount == noDodos) {
			// Finished the level
			calculateScore();
			levelCompleted = true;
			levelgui.CompleteLevel(score);
		}
	}

	public void calculateScore(){
		// Needs to be discussed
		score = dodoFinishedCount * 10;
	}
		
		
}