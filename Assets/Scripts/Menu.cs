using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public void startGame(){
		SceneManager.LoadScene ("Level Selection");
	}
}
