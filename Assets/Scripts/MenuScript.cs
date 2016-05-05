using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public void startGame(){
		SceneManager.LoadScene ("Level Selection");
	}
}
