using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public int noLTiles;
	public int noTTiles;
	public int noCrossTiles;

	public GameObject floor;
	public GUIScript gui;

	void Start(){
		gui = gui.GetComponent<GUIScript> ();
		gui.updateGUI ();
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

}
