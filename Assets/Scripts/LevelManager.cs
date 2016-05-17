using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager: MonoBehaviour {

	// attributes of level
	public int levelID;
	public List<TetrisBlock.Shape> blocks;
	public int noDodos;

	public GameObject floor;
	public LevelUI levelgui;

	private int dodoDeathCount;
	private int dodoFinishedCount;
	private bool levelCompleted;
	private int score;

	void Start(){
		levelgui = levelgui.GetComponent<LevelUI> ();
		levelgui.updateInventory ();

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
		if (Input.GetKeyDown (KeyCode.Space) && levelCompleted) {
			GameManager.reloadLevel ();
		}
	}

	public void addTile(TetrisBlock.Shape shape){
		floor.GetComponent<BlockPlacement>().setTetrisShape (shape);
	}

	public void removeTile(TetrisBlock.Shape type){
		blocks.Remove (type);
		levelgui.updateInventory ();

	}

	// Called everytime a dodo dies
	public void dodoDeath(){
		dodoDeathCount++;
		levelgui.lowerExtinctionCount ();
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