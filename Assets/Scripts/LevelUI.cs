using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {

	public GameObject pauseMenuPrefab;
	public RectTransform completeLevel;
	SidePanelUI inventoryUI;

	// Use this for initialization
	void Awake () {
		completeLevel = completeLevel.GetComponent<RectTransform> ();
		inventoryUI = transform.FindChild ("SideGUI").GetComponent<SidePanelUI>();
	}

	public void pause(){
		GameObject pauseMenu = Instantiate (pauseMenuPrefab);
		pauseMenu.transform.SetParent (transform);
		pauseMenu.transform.position = new Vector2 ((Screen.width / 2), (Screen.height / 2));
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild (i).name == "PauseButton") {
				Transform pauseButton = transform.GetChild (i);
				pauseButton.gameObject.GetComponent<Button> ().interactable = false;

				string filename = "pause1";
				Sprite sprite = Resources.Load<Sprite>(filename);
				pauseButton.gameObject.GetComponent<Image>().sprite = sprite;
			}
		}
	}
		

	public void CompleteLevel(int score, int numDodos){
		RectTransform comLevel =  Instantiate (completeLevel);
		Transform scoreLabel = comLevel.FindChild ("Score");
        // The trophy info should probably be stored in a seperate label, but for now I put it in the score one.
        string filename = "";
        print(score + ", " + numDodos + ", " + GameManager.getGold());
		if (score >= (numDodos + GameManager.getGold ()) * 10) {
			filename = "fm_gold";
		} else if (score >= (numDodos + GameManager.getSilver ()) * 10) {
			filename = "fm_silver";
		} else if (score >= (numDodos + GameManager.getBronze ()) * 10) {
			filename = "fm_bronze";
		} else {
			filename = "fm_poop";
		}
		Sprite sprite = Resources.Load<Sprite>(filename);
		comLevel.GetComponent<Image> ().sprite = sprite;
		scoreLabel.GetComponent<Text>().text = ""+score;
        comLevel.transform.SetParent (transform, false);
	}

	public void updateInventory(){
		inventoryUI.updateInventory ();
	}

	public void tetrisBlockSelected(TetrisBlock.Type shapeSelected){
		inventoryUI.tetrisBlockSelected (shapeSelected);
	}

	public void deselectBlocks(){
		inventoryUI.deselectBlocks();
	}
}
