using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {

	public RectTransform completeLevel;

	// Use this for initialization
	void Start () {
		completeLevel = completeLevel.GetComponent<RectTransform> ();
	}

	public void CompleteLevel(int score){
		RectTransform comLevel =  Instantiate (completeLevel);
		Transform scoreLabel = comLevel.FindChild ("Score");
		scoreLabel.GetComponent<Text> ().text = "Score: " + score;
		comLevel.transform.SetParent (transform, false);
	}

}
