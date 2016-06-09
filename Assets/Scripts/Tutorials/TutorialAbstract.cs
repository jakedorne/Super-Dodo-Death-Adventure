﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class TutorialAbstract : MonoBehaviour {

	public GameObject lessonScreenPrefab;
	public GameObject infoObject;
	public GameObject lessonPlaceholder;

	const float DELAYONLESSON = 2.0f;

	GameObject levelScreen;
	float timeLeftBeforeLesson;
	bool timerOn;
	protected LevelManager manager;
	string currentLesson;

	// Abstract methods
	abstract public void updateLessonState();
	abstract public string getNextLesson ();
	abstract public bool isLessonComplete (string lesson);
	abstract public void intialise();
	abstract public bool delaySpawn ();

	// Use this for initialization
	void Start () {
		timeLeftBeforeLesson = DELAYONLESSON;
		timerOn = true;
		manager = FindObjectOfType<LevelManager> ();
		intialise ();
	}
	
	// Update is called once per frame
	void Update () {
		updateLessonState ();
		if (currentLesson != null && isLessonComplete (currentLesson)) {
			prepareNextLesson ();
			currentLesson = null;
		}
		if (timerOn) {
			timeLeftBeforeLesson -= Time.deltaTime;
			if (timeLeftBeforeLesson < 0) {
				presentLesson ();
				timerOn = false;
			}
		}
	}

	public void presentLesson(){
		if (getNextLesson () == null) {
			completeTutorial ();
		}
		else{
			levelScreen = Instantiate (lessonScreenPrefab) as GameObject;
			levelScreen.transform.SetParent (transform);
			levelScreen.GetComponent<LessonScreen> ().SetPosition (lessonPlaceholder.transform.position);
			levelScreen.GetComponent<LessonScreen> ().StartBobbing (LessonScreen.Axis.y);
			Transform screensChild = levelScreen.transform.GetChild (0); // get text
			currentLesson = getNextLesson ();
			screensChild.GetComponent<Text>().text = currentLesson;
			// notify user
			string filename = "info1";
			Sprite sprite = Resources.Load<Sprite>(filename);
			infoObject.GetComponent<Image> ().sprite = sprite;
		}
	}

	public void completeTutorial(){
		Destroy (levelScreen);
		if (delaySpawn()) {
			// start spawn
			FindObjectOfType<Spawner> ().resumeGame();
		}


		// disable notification
		string filename = "info0";
		Sprite sprite = Resources.Load<Sprite>(filename);
		infoObject.GetComponent<Image> ().sprite = sprite;

		// ensure timer is off
		timerOn = false;
	}

	public void prepareNextLesson(){
		print ("Prepare next lesson");
		Destroy (levelScreen);
		timerOn = true;
		timeLeftBeforeLesson = DELAYONLESSON;

		// disable notification
		string filename = "info0";
		Sprite sprite = Resources.Load<Sprite>(filename);
		infoObject.GetComponent<Image> ().sprite = sprite;
	}


}
