using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour {

	public GameObject lessonScreenPrefab;

	public GameObject SelectionLessonPlaceholder;
	public GameObject RotationLessonPlaceholder;
	public GameObject PlacementLessonPlaceholder;

	const float DELAYONLESSON = 2.0f;

	// Different lessons
	const int SelectionLessonID = 0;
	const int RotationLessonID = 1;
	const int PlacementLessonID = 2;

	// Lesson Texts are defined in here for now, might move into a text file
	string SelectionLesson = "To get started, select a block from your inventory.\n\nThis can be done by selecting the block with the mouse, or you can iterate through your inventory by right clicking the mouse or using the W and S keys.";
	string RotationLesson = "Try rotating the block.\n\nYou can do this by using the scroll wheel on your mouse, or using the A and D keys.";
	string PlacementLesson = "Now try place the block.\n\nWhen you have positioned it where you would like to place it, left click on the mouse.";

	int lessonID = 0;
	GameObject levelScreen;

	float timeLeftBeforeLesson;
	bool timerOn;

	LevelManager manager;

	// Use this for initialization
	void Start () {
		timeLeftBeforeLesson = DELAYONLESSON;
		timerOn = true;
		manager = FindObjectOfType<LevelManager> ();
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

		if (lessonID == SelectionLessonID) {
			if (manager.getCurrentBlock() != null){
				nextLesson ();
			}
		}

		if (lessonID == RotationLessonID) {
			if (manager.getCurrentBlock ().hasBeenRotated) {
				nextLesson ();
			}
		}

		if (lessonID == PlacementLessonID) {
			if (manager.getNumberOfBlocksPlaced() > 0) {
				completeTutorial ();
			}
		}
	}

	public void presentLesson(){
		
		// Set listener to button so we know when lesson is done
		if(lessonID == SelectionLessonID){
			
			levelScreen = Instantiate (lessonScreenPrefab) as GameObject;

			levelScreen.transform.SetParent (transform);

			//RectTransform rt = SelectionLessonPlaceholder.GetComponent<RectTransform> ();
			//Vector3 poition = new Vector3 (rt.rect.xMin, rt.rect.yMin, rt.position.z);
			//print ("pos: " + poition.ToString ());
			levelScreen.GetComponent<LessonScreen> ().SetPosition (SelectionLessonPlaceholder.transform.position);
			levelScreen.GetComponent<LessonScreen> ().StartBobbing (LessonScreen.Axis.x);

			Transform screensChild = levelScreen.transform.GetChild (0); // get text
			screensChild.GetComponent<Text>().text = SelectionLesson;
		}

		// Block rotattion
		if (lessonID == RotationLessonID) {
			
			levelScreen = Instantiate (lessonScreenPrefab) as GameObject;

			levelScreen.transform.SetParent (transform);
			levelScreen.GetComponent<LessonScreen> ().SetPosition(RotationLessonPlaceholder.transform.position);
			levelScreen.GetComponent<LessonScreen> ().StartBobbing (LessonScreen.Axis.y);

			Transform screensChild = levelScreen.transform.GetChild (0); // get text
			screensChild.GetComponent<Text>().text = RotationLesson;

		}

		// Block placement
		if(lessonID == PlacementLessonID) {
			
			levelScreen = Instantiate (lessonScreenPrefab) as GameObject;

			levelScreen.transform.SetParent (transform);
			levelScreen.GetComponent<LessonScreen> ().SetPosition(PlacementLessonPlaceholder.transform.position);
			levelScreen.GetComponent<LessonScreen> ().StartBobbing (LessonScreen.Axis.y);

			Transform screensChild = levelScreen.transform.GetChild (0); // get text
			screensChild.GetComponent<Text>().text = PlacementLesson;

		}
	}

	public void completeTutorial(){
		Destroy (levelScreen);
		lessonID = -1;
		FindObjectOfType<Spawner> ().beginSpawning();
	}

	public void nextLesson(){
		Destroy (levelScreen);
		lessonID = lessonID+1;
		timerOn = true;
		timeLeftBeforeLesson = DELAYONLESSON;
	}
}
