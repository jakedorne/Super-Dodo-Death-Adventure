using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DodoBehaviour : MonoBehaviour {

	private GameObject floor;
	private Floor floorScript;

	int currentX;
	int currentZ;

	int lastX;
	int lastZ;


	// Use this for initialization
	void Start () {
		floor = GameObject.Find ("Floor");
		floorScript = floor.GetComponent<Floor> ();
		currentX = floorScript.startX;
		currentZ = floorScript.startZ;

		//InvokeRepeating ("moveDodo", 2f, 2f);

	
	}
	
	// Update is called once per frame
	void Update () {

			

	}

	//These methods are not currently working, hence they are commented out to avoid console spam.
	private void moveDodo() {
		Dictionary<Vector2, Vector3> potentialBlocks = new Dictionary<Vector2, Vector3> ();

		Vector2 left = floorScript.getCoordAtVector (transform.right * -1);
		Vector2 right = floorScript.getCoordAtVector (transform.right);
		Vector2 forward = floorScript.getCoordAtVector (transform.forward);

		if (floorScript.positionOnBlock ((int)left.x, (int)left.y)) potentialBlocks.Add (left, transform.right * -1);
		if (floorScript.positionOnBlock ((int)right.x, (int)right.y)) potentialBlocks.Add (right, transform.right);
		if (floorScript.positionOnBlock ((int)forward.x, (int)forward.y)) potentialBlocks.Add (forward, transform.forward);

		Vector2 bestBlock = findBestBlock (potentialBlocks);

		if (bestBlock == left) {
			transform.Rotate (0, 270, 0);
		} else if (bestBlock == right) {
			transform.Rotate (0, 90, 0);
		}

		transform.position = potentialBlocks [bestBlock];
	}

	private Vector2 findBestBlock( Dictionary<Vector2, Vector3> potentialBlocks) {
		Vector2 bestGridPos = Vector2.zero;
		Vector3 bestWorldPos = Vector3.zero;

		Vector3 goalWorldPos = floorScript.getVectorAtCoords (floorScript.endX, floorScript.endZ);

		foreach (Vector2 gridPos in potentialBlocks.Keys) {
			if (bestGridPos == Vector2.zero) {
				bestGridPos = gridPos;
				bestWorldPos = potentialBlocks [gridPos];
			}
			else if (Vector3.Distance (potentialBlocks[gridPos], goalWorldPos) < Vector3.Distance (bestWorldPos, goalWorldPos)) {
				bestGridPos = gridPos;
				bestWorldPos = potentialBlocks [gridPos];
			}
		}

		return bestGridPos;
	}
		
}
