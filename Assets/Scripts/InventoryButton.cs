using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour {

	public TetrisBlock.Type value;
	public int amount;

	public AudioClip clickSound;
	AudioSource audio;

	const string UNCLICKED_EXTENSION = "0";
	const string CLICKED_EXTENSION = "1";

	private float volume = 0.1f;
	private float globalVolume = 1;

	private bool pause = false;

	// Use this for initialization
	void Start () {
		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		LevelManager manager = managerGO.GetComponent<LevelManager> ();
		// Set face image
		string filename = value.ToString() + UNCLICKED_EXTENSION;
		Sprite sprite = Resources.Load<Sprite>(filename);
		GetComponent<Image> ().sprite = sprite;
		GetComponent<Button> ().transform.GetChild (0).GetComponent<Text> ().text = "" + amount;
		audio = GetComponent<AudioSource>();
	}

	void Update(){
		if (Input.GetKeyDown("m"))
		{
			if (globalVolume == 1)
			{
				globalVolume = 0;
				AudioListener.volume = 0;
			} else
			{
				globalVolume = 1;
				AudioListener.volume = 1;
			}
		}
	}

	public void OnGamePause(){
		if (!pause) {
			GetComponent<Button> ().interactable = false;
			pause = true;
		} else {
			GetComponent<Button> ().interactable = true;
			pause = false;
		}
	}
		
	public void buttonClicked(){
		// First deselect all blocks in case one is already selected
		FindObjectOfType<SidePanelUI> ().deselectBlocks ();
		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		LevelManager manager = managerGO.GetComponent<LevelManager> ();
		string filename = value.ToString() + CLICKED_EXTENSION;
		Sprite sprite = Resources.Load<Sprite>(filename);
		GetComponent<Image> ().sprite = sprite;
		manager.addTile (value);
		audio.PlayOneShot(clickSound, volume);
	}

	public void unselectButton(){
		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		LevelManager manager = managerGO.GetComponent<LevelManager> ();
		string filename = value.ToString() + UNCLICKED_EXTENSION;
		Sprite sprite = Resources.Load<Sprite>(filename);
		GetComponent<Image> ().sprite = sprite;
	}


		
}
