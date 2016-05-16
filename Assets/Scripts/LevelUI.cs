using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {

	public RectTransform completeLevel;
	public bool levelCompleted = false;

	void Update(){
		
		if (Input.GetKeyDown (KeyCode.Space) && levelCompleted) {
			print ("enter");
			GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
			LevelManager manager = managerGO.GetComponent<LevelManager> ();
			manager.finish ();

		}
			

	}

	// Use this for initialization
	void Start () {
		completeLevel = completeLevel.GetComponent<RectTransform> ();
	}

	public void CompleteLevel(){
		int x = (Screen.width / 2);
		int y = (Screen.height / 2);
		int z = 0;
		Vector3 position = new Vector3 (x, y, z);
		RectTransform comLevel =  Instantiate (completeLevel);
		//comLevel.transform.position = position;
		comLevel.transform.SetParent (transform, false);
		levelCompleted = true;

	}

}
