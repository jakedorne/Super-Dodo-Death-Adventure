using UnityEngine;
using System.Collections;

public class Tutorial3 : TutorialAbstract {

	int numberOfLessons = 1;

	// Different lessons
	const int BridgeLessonID = 0;

	// Lesson Texts are defined in here for now, might move into a text file
	string BridgeLesson = "Introducing bridges!\n\nBridges are like other blocks, however they aren't as strong.\n\nEach time a dodo walks across a bridge it becomes weaker.";

	bool[] completeLessons;
	string[] lessons;

	public override void intialise(){
		completeLessons = new bool[numberOfLessons];
		lessons = new string[numberOfLessons];
		for (int i = 0; i < numberOfLessons; i++) {
			completeLessons [i] = false;
			if (BridgeLessonID == i) {
				lessons [i] = BridgeLesson;
			}
		}
	}

	public override void updateLessonState ()
	{
		// Check to see if block has been rotated. Then the block rotation lesson 
		// does not need to occur.
		if (false) {
			completeLessons [BridgeLessonID] = true;
		}
	}

	public override string getNextLesson(){
		for (int i = 0; i < numberOfLessons; i++) {
			if (completeLessons [i] == false) {
				return lessons [i];
			}
		}
		return null;
	}

	public override bool isLessonComplete (string lesson)
	{
		for(int i = 0; i < numberOfLessons; i++){
			if (lessons [i] == lesson && completeLessons[i]) {
				return true;
			}
		}

		return false;
	}

	public override bool delaySpawn (){
		return false;
	}
}
