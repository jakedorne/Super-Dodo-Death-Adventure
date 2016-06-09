using UnityEngine;
using System.Collections;

public class Tutorial2 : TutorialAbstract{

	int numberOfLessons = 1;

	// Different lessons
	const int DeadDodoLessonID = 0;

	// Lesson Texts are defined in here for now, might move into a text file
	string DeadDodoLesson = "All is not lost when a dodo dies!\n\nDead dodos float in the water and add to the path. Sometimes sacrificing a dodo is necessary to get to the end.";

	bool[] completeLessons;
	string[] lessons;

	public override void intialise(){
		completeLessons = new bool[numberOfLessons];
		lessons = new string[numberOfLessons];
		for (int i = 0; i < numberOfLessons; i++) {
			completeLessons [i] = false;
			if (DeadDodoLessonID == i) {
				lessons [i] = DeadDodoLesson;
			}
		}
	}

	public override void updateLessonState ()
	{
		// Check to see if block has been rotated. Then the block rotation lesson 
		// does not need to occur.
		if (false) {
			completeLessons [DeadDodoLessonID] = true;
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
