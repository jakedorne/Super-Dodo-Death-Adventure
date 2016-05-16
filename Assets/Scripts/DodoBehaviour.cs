using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DodoBehaviour : MonoBehaviour {

	private GameObject floor;
	private Floor floorScript;

	private int currentX;
	private int currentZ;

	private int lastX;
	private int lastZ;

	private float lerpTime = 1f;
	private float currentLerpTime;

	private Animator anim;

	public static Vector2 MAX_VECTOR2 = new Vector2 (float.MaxValue, float.MaxValue);
    private Vector3 startPosition;
    private Vector3 endPosition;
	private Vector2 target;


	// Use this for initialization
	void Start () {
		floor = GameObject.Find ("Floor");
		floorScript = floor.GetComponent<Floor> ();
		currentX = floorScript.startX;
		currentZ = floorScript.startZ;
		anim = GetComponent<Animator> ();
		target = new Vector2 (floorScript.endX-1, floorScript.endZ-1); //Need to subtract 1 because array starts at 0
        //InvokeRepeating ("moveCycle", 0.5f, 1f);
        moveCycle();
        transform.LookAt(endPosition);

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0)
        {
            Destroy(this.gameObject);
            LevelManager script = FindObjectOfType<LevelManager>();
            script.dodoDeath();
        }
        if (floorScript.getCoordAtVector(transform.position) == target)
        {
            // clean this shit up eventually
            LevelManager script = FindObjectOfType<LevelManager>();
            script.dodoFinished();
            Destroy(this.gameObject);
        }
        //First, check if we have reached our target. If we have, set a new target.
        if (endPosition==Vector3.zero)
        {
			anim.SetBool("isWalking", true);
            //Walking off an edge. Have to scale it down so the dodo doesnt shoot off the end
            transform.position += transform.forward * 0.02f / lerpTime;
        } else if (transform.position != endPosition)
        {
			anim.SetBool("isWalking", true);
            moveDodo(startPosition,endPosition);
        } else
        {
			anim.SetBool("isWalking", false);
            currentLerpTime = 0f;
            moveCycle();
        }
    }

	//These methods are not currently working.
	private void moveCycle() {
		List<Vector2> potentialBlocks = new List<Vector2> ();

		if(floorScript.positionOnBlock(currentX, currentZ+1))potentialBlocks.Add(new Vector2(currentX, currentZ+1));
		if(floorScript.positionOnBlock(currentX, currentZ-1))potentialBlocks.Add(new Vector2(currentX, currentZ-1));
		if(floorScript.positionOnBlock(currentX+1, currentZ)) potentialBlocks.Add(new Vector2(currentX+1, currentZ));
		if(floorScript.positionOnBlock(currentX-1, currentZ))potentialBlocks.Add(new Vector2(currentX-1, currentZ));

		potentialBlocks = removeLastPos (potentialBlocks);

		if (potentialBlocks.Count == 0) {
            endPosition = Vector3.zero;
			return;
		}

		Vector2 bestBlock = findBestBlock (potentialBlocks);

		lastX = currentX;
		lastZ = currentZ;
		currentX = (int)bestBlock.x;
		currentZ = (int)bestBlock.y;

        startPosition = transform.position;
        endPosition = floorScript.getVectorAtCoords((int)bestBlock.x, (int)bestBlock.y);

        transform.LookAt(endPosition);

        //moveDodo(transform.position, floorScript.getVectorAtCoords((int)bestBlock.x, (int)bestBlock.y));
    }

	private void moveDodo(Vector3 startPosition, Vector3 endPosition) {
		//Lerp tutorial from: https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/
		currentLerpTime += Time.deltaTime;
		if (currentLerpTime > lerpTime) {
			currentLerpTime = lerpTime;
		}

		float t = currentLerpTime / lerpTime;
		t = t * t * t * (t * (3f * t - 7f) + 5f);
		transform.position = Vector3.Lerp (startPosition, endPosition, t);
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
