using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {

	public GameObject pauseMenuPrefab;
	public RectTransform completeLevel;
	SidePanelUI inventoryUI;
	Transform pauseButton;
	GameObject pauseMenu;
	bool paused = false;

	// Use this for initialization
	void Awake () {
		completeLevel = completeLevel.GetComponent<RectTransform> ();
		inventoryUI = transform.FindChild ("SideGUI").GetComponent<SidePanelUI>();
	}

	void Start(){
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild (i).name == "PauseButton") {
				pauseButton = transform.GetChild (i);
			}
		}
	}

	public void pauseButtonClicked(){
		if (paused) {
			paused = false;
			resume ();
		} else {
			paused = true;
			pause ();
		}
	}

	public void resume(){
		string filename = "pause0";
		Sprite sprite = Resources.Load<Sprite> (filename);
		pauseButton.gameObject.GetComponent<Image> ().sprite = sprite;

		FindObjectOfType<LevelManager> ().pause ();

		Destroy (pauseMenu);
	}

	public void pause(){
		pauseMenu = Instantiate (pauseMenuPrefab);
		pauseMenu.transform.SetParent (transform);
		pauseMenu.transform.position = new Vector2 ((Screen.width / 2), (Screen.height / 2));

		string filename = "pause1";
		Sprite sprite = Resources.Load<Sprite> (filename);
		pauseButton.gameObject.GetComponent<Image> ().sprite = sprite;
			
		FindObjectOfType<LevelManager> ().pause ();
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
