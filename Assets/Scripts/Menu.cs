using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public void startGame(){
		GameManager.levelSelection ();
	}

	public void quitGame(){
		Application.Quit();
	}
}
