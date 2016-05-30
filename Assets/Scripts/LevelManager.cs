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
    private bool paused;

	void Start(){
		levelgui = levelgui.GetComponent<LevelUI> ();
		levelgui.updateInventory ();

		dodoDeathCount = 0;
		dodoFinishedCount = 0;

		levelCompleted = false;

        GameManager.setUpLevels();
	}

	void Update(){
		// For testing purposes
		if (Input.GetKeyDown (KeyCode.K)) {
			dodoDeath ();
		}
		if(Input.GetKeyDown(KeyCode.L)){
			dodoFinished();
		}

		if (Input.GetMouseButtonDown (1) || Input.GetKeyDown("w")) {
			if (blocks.Count != 0) {
				TetrisBlock.Shape automaticBlock = getAutomaticBlock ();
				addTile(automaticBlock);
				levelgui.tetrisBlockSelected (automaticBlock);
			}
		}
		if (Input.GetMouseButtonDown (0) && levelCompleted) {
			GameManager.finishedLevel(levelID, score);
		}
		if (Input.GetKeyDown (KeyCode.Space) && levelCompleted) {
			GameManager.reloadLevel ();
		}
        if (Input.GetKeyDown("p"))
        {
            pause();
        }
	}

	public TetrisBlock.Shape getAutomaticBlock(){
		TetrisBlock.Shape block;
		TetrisBlock currentBlock = floor.GetComponent<BlockPlacement> ().getSelectedShape ();
		if (currentBlock == null) {
			// Get the first block
			block = blocks [0];
		} else {
			// If there is a block selected, need to deselect it
			levelgui.deselectBlocks();
			// need to find the next block to toggle to
			List<TetrisBlock.Shape> uniqueBlockTypes = new List<TetrisBlock.Shape> ();
			foreach(TetrisBlock.Shape inventoryBlock in blocks){
				bool duplicate = false;
				foreach(TetrisBlock.Shape uniqueBlock in uniqueBlockTypes){
					if (uniqueBlock == inventoryBlock) {
						duplicate = true;
						break;
					}
				}
				if (!duplicate) {
					uniqueBlockTypes.Add (inventoryBlock);
				}
			}
			int currentIndex = uniqueBlockTypes.IndexOf(currentBlock.shape);
			if (currentIndex == (uniqueBlockTypes.Count - 1)) {
				block = uniqueBlockTypes [0];
			} else {
				block = uniqueBlockTypes [currentIndex + 1];
			}
		}
		return block;
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

    private void pause()
    {
        //Messing around with different pause options
        //Timescale stops all animations. Not ideal.
        /*
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }
        */
        
        //Pause function can stop dodos but not animations which is cool.
        //BUT unsure how to pause the invoke to stop more dodos from spawning.
        /*
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
        {
            go.SendMessage("OnGamePause", SendMessageOptions.DontRequireReceiver);
        }
        */
    }
		
		
}