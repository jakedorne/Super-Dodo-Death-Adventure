using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour {

	public GameObject lessonScreenPrefab;

	// Lesson Texts are defined in here for now, might move into a text file
	string lesson1 = "You need to select a block from the inventory UI";

	int lessonID = 0;
	GameObject levelScreen;

	float timeLeftBeforeLesson;
	bool timerOn;

	// Use this for initialization
	void Start () {
		timeLeftBeforeLesson = 3.0f;
		timerOn = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (timerOn) {
			timeLeftBeforeLesson -= Time.deltaTime;
			if (timeLeftBeforeLesson < 0) {
				presentLesson ();
				timerOn = false;
			}
		}
	}

	public void presentLesson(){
		// Set listener to button so we know when lesson is done
		if(lessonID == 0){
			Vector3 bricksLocation = Vector3.zero;
			for (int i = 0; i < transform.childCount; i++) {
				Transform child = transform.GetChild (i);
				if (child.name == "Inventory") {
					// Find first brick
					Transform brick = child.GetChild (0);
					brick.GetComponent<Button> ().onClick.AddListener (() => nextLesson ());
					bricksLocation = brick.position;
				}
			}

			// Where to show lesson screen, at the moment the x-extension is hard coded, it should be calculated using resolution.
			Vector3 lessonPosition = new Vector3 (bricksLocation.x + 150.0f, bricksLocation.y, bricksLocation.z);
			levelScreen = Instantiate (lessonScreenPrefab, lessonPosition, Quaternion.identity) as GameObject;
			levelScreen.transform.SetParent (transform);
			Transform screensChild = levelScreen.transform.GetChild (0); // get text
			screensChild.GetComponent<Text>().text = lesson1;
		}
	}

	public void nextLesson(){
		Destroy (levelScreen);


	}
}
