using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public void reloadLevel(){
		GameManager.reloadLevel ();
	}

	public void levelSelection(){
		GameManager.levelSelection ();
	}

	public void close(){
		Destroy (this.gameObject);
	}

}
