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

	Vector3 currentRotation;

	int count = 0;


	// Use this for initialization
	void Start () {
		floor = GameObject.Find ("Floor");
		floorScript = floor.GetComponent<Floor> ();
		currentX = floorScript.startX;
		currentZ = floorScript.startZ;
		currentRotation = transform.rotation.eulerAngles;

		InvokeRepeating ("moveDodo", 2f, 2f);

	
	}
	
	// Update is called once per frame
	void Update () {

			

	}

	//These methods are not currently working.
	private void moveDodo() {
		float offset = 12f;


		List<Vector2> potentialBlocks = new List<Vector2> ();

		if(floorScript.positionOnBlock(currentX+1, currentZ)) potentialBlocks.Add(new Vector2(currentX+1, currentZ));
		if(floorScript.positionOnBlock(currentX-1, currentZ))potentialBlocks.Add(new Vector2(currentX-1, currentZ));
		if(floorScript.positionOnBlock(currentX, currentZ+1))potentialBlocks.Add(new Vector2(currentX, currentZ+1));
		if(floorScript.positionOnBlock(currentX, currentZ-1))potentialBlocks.Add(new Vector2(currentX, currentZ-1));

		//if (floorScript.positionOnBlock ((int)left.x, (int)left.y)) potentialBlocks.Add (left, (transform.right * -1)/offset);
		//if (floorScript.positionOnBlock ((int)right.x, (int)right.y)) potentialBlocks.Add (right, (transform.right)/offset);
		//if (floorScript.positionOnBlock ((int)forward.x, (int)forward.y)) potentialBlocks.Add (forward, (transform.forward)/offset);

		potentialBlocks = removeLastPos (potentialBlocks);

		if (potentialBlocks.Count == 0)
			return;

		Vector2 bestBlock = findBestBlock (potentialBlocks);

		//if (bestBlock == left) {
			//transform.Rotate (0, 270, 0);
		//} else if (bestBlock == right) {
			//transform.Rotate (0, 90, 0);
		//}

		lastX = currentX;
		lastZ = currentZ;
		currentX = (int)bestBlock.x;
		currentZ = (int)bestBlock.y;

		transform.position = floorScript.getVectorAtCoords((int)bestBlock.x, (int)bestBlock.y);
	}
	
	public List<Vector2> removeLastPos(List<Vector2> potentialBlocks) {
		Vector2 toRemove = Vector2.zero;
		foreach (Vector2 gridPos in potentialBlocks) {
			if ((int)gridPos.x == lastX && (int)gridPos.y == lastZ) {
				toRemove = gridPos;
				break;
			}
		}

		if(toRemove != Vector2.zero) potentialBlocks.Remove (toRemove);
		return potentialBlocks;
	}
	

	//Could be the problem
	private Vector2 findBestBlock(List<Vector2> potentialBlocks) {
		Vector2 bestGridPos = Vector2.zero;
		Vector2 goalGridPos = new Vector2 (floorScript.endX, floorScript.endZ);

		foreach (Vector2 gridPos in potentialBlocks) {
			if (bestGridPos == Vector2.zero) {
				bestGridPos = gridPos;
			} else if (Vector3.Distance (gridPos, goalGridPos) < Vector3.Distance (bestGridPos, goalGridPos)) {
				bestGridPos = gridPos;
			}
		}

		return bestGridPos;
	}
		
}
