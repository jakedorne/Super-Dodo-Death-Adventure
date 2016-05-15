using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DodoBehaviour : MonoBehaviour {

	private GameObject floor;
	private Floor floorScript;

	private int currentX;
	private int currentZ;
	private Vector3 currentRotation;

	private int lastX;
	private int lastZ;

	private float lerpTime = 100f;
	private float currentLerpTime;

	public static Vector2 MAX_VECTOR2 = new Vector2 (float.MaxValue, float.MaxValue);


	// Use this for initialization
	void Start () {
		floor = GameObject.Find ("Floor");
		floorScript = floor.GetComponent<Floor> ();
		currentX = floorScript.startX;
		currentZ = floorScript.startZ;
		currentRotation = transform.rotation.eulerAngles;

		InvokeRepeating ("moveCycle", 0.5f, 1f);

	
	}
	
	// Update is called once per frame
	void Update () {

			

	}

	//These methods are not currently working.
	private void moveCycle() {
		List<Vector2> potentialBlocks = new List<Vector2> ();

		if(floorScript.positionOnBlock(currentX, currentZ+1))potentialBlocks.Add(new Vector2(currentX, currentZ+1));
		if(floorScript.positionOnBlock(currentX, currentZ-1))potentialBlocks.Add(new Vector2(currentX, currentZ-1));
		if(floorScript.positionOnBlock(currentX+1, currentZ)) potentialBlocks.Add(new Vector2(currentX+1, currentZ));
		if(floorScript.positionOnBlock(currentX-1, currentZ))potentialBlocks.Add(new Vector2(currentX-1, currentZ));


		//if (floorScript.positionOnBlock ((int)left.x, (int)left.y)) potentialBlocks.Add (left, (transform.right * -1)/offset);
		//if (floorScript.positionOnBlock ((int)right.x, (int)right.y)) potentialBlocks.Add (right, (transform.right)/offset);
		//if (floorScript.positionOnBlock ((int)forward.x, (int)forward.y)) potentialBlocks.Add (forward, (transform.forward)/offset);

		foreach (Vector2 gridPos in potentialBlocks) {
			print ("Potential Block: " + gridPos);
		}

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

		//transform.position = floorScript.getVectorAtCoords ((int)bestBlock.x, (int)bestBlock.y);
		moveDodo(transform.position, floorScript.getVectorAtCoords((int)bestBlock.x, (int)bestBlock.y));
	}

	private void moveDodo(Vector3 startPosition, Vector3 endPosition) {
		//Lerp tutorial from: https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/
		transform.LookAt(endPosition);
		currentLerpTime = 0f;
		while (transform.position != endPosition) {
			currentLerpTime += Time.deltaTime;
			if (currentLerpTime > lerpTime) {
				currentLerpTime = lerpTime;
			}

			float t = currentLerpTime / lerpTime;
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			transform.position = Vector3.Lerp (startPosition, endPosition, t);
		}
	}
	
	private List<Vector2> removeLastPos(List<Vector2> potentialBlocks) {
		Vector2 toRemove = MAX_VECTOR2;
		foreach (Vector2 gridPos in potentialBlocks) {
			if ((int)gridPos.x == lastX && (int)gridPos.y == lastZ) {
				toRemove = gridPos;
				break;
			}
		}

		if (toRemove != MAX_VECTOR2) { 
			potentialBlocks.Remove (toRemove);
			print ("Removed: " + toRemove);
		}
		return potentialBlocks;
	}
	

	//Could be the problem
	private Vector2 findBestBlock(List<Vector2> potentialBlocks) {
		Vector2 bestGridPos = MAX_VECTOR2;
		Vector2 goalGridPos = new Vector2 (floorScript.endX, floorScript.endZ);

		foreach (Vector2 gridPos in potentialBlocks) {
			if (bestGridPos == MAX_VECTOR2) {
				bestGridPos = gridPos;
			} else if (Vector2.Distance (gridPos, goalGridPos) < Vector2.Distance (bestGridPos, goalGridPos)) {
				bestGridPos = gridPos;
			}
		}

		return bestGridPos;
	}
		
}
