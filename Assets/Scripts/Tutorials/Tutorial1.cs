using UnityEngine;
using System.Collections;

public class Tutorial1 : TutorialAbstract {

	int numberOfLessons = 3;

	// Different lessons
	const int SelectionLessonID = 0;
	const int RotationLessonID = 1;
	const int PlacementLessonID = 2;

	// Lesson Texts are defined in here for now, might move into a text file
	string SelectionLesson = "To get started, select a block from your inventory.\n\nThis can be done by selecting the block with the mouse, or you can iterate through your inventory by right clicking the mouse or using the W and S keys.";
	string RotationLesson = "Try rotating the block.\n\nYou can do this by using the scroll wheel on your mouse, or using the A and D keys.";
	string PlacementLesson = "Now try place the block.\n\nWhen you have positioned it where you would like to place it, left click on the mouse.";

	bool[] completeLessons;
	string[] lessons;

	public override void intialise(){
		completeLessons = new bool[numberOfLessons];
		lessons = new string[numberOfLessons];
		for (int i = 0; i < numberOfLessons; i++) {
			completeLessons [i] = false;
			if (SelectionLessonID == i) {
				lessons [i] = SelectionLesson;
			}
			else if (RotationLessonID == i) {
				lessons [i] = RotationLesson;
			}
			else if (PlacementLessonID == i) {
				lessons [i] = PlacementLesson;
			}
		}
	}

	public override void updateLessonState ()
	{
		// Check to see if block has been rotated. Then the block rotation lesson 
		// does not need to occur.
		if (base.manager.getCurrentBlock() != null && manager.getCurrentBlock ().hasBeenRotated) {
			completeLessons [RotationLessonID] = true;
		}
		// Check to see if the user has selected a block, if so, block selection lesson
		// does not need to occur.
		if (manager.getCurrentBlock() != null){
			completeLessons [SelectionLessonID] = true;
		}

		if(manager.getNumberOfBlocksPlaced() > 0) {
			completeLessons [PlacementLessonID] = true;
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
		
}
