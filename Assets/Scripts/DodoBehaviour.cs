using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DodoBehaviour : MonoBehaviour {

	private Floor floorScript;

	private PathFinder pathFinder;

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

    private bool spawnBlockOnDeath = true; //Wanted to try this out and see how it feels.


    // Use this for initialization
    void Start () {
		floorScript = GameObject.Find ("Floor").GetComponent<Floor> ();
		pathFinder = GameObject.Find ("PathFinder").GetComponent<PathFinder> ();
        Spawner spawnerScript = GameObject.Find("Dodo Spawner").GetComponent<Spawner>();
        lerpTime = spawnerScript.dodoSpawnTimer * 0.2f; //Tie these variables together so speed of spawning is related to speed of dodo movement.
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
            if (spawnBlockOnDeath)
            {
                //Might need to wait for animation to finish before calling this.
                Vector2 pos = floorScript.getCoordAtVector(transform.position);
                floorScript.createDeadDodoBlock(pos);
            }
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
            //Just fall, no movement necessary
            //transform.position += transform.forward * 0.02f / lerpTime;
        } else if (transform.position != endPosition)
        {
			//print ("I should be moving to: "  + endPosition);
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

        //If we are not standing on a block, we should stop moving and have no goal.
        if (!floorScript.isBlock(floorScript.getCoordAtVector(transform.position)))
        {
            endPosition = Vector3.zero;
            return;
        }

		Vector2 endCoord = pathFinder.findNextMove (floorScript.getCoordAtVector (transform.position));

        //If we are told that the goal is our own block, we don't know where to go
        //For now, just go forwards
        if (endCoord == floorScript.getCoordAtVector(transform.position))
        {
            endCoord = floorScript.getCoordAtVector(transform.position + transform.forward);
        }

		startPosition = transform.position;
		endPosition = floorScript.getVectorAtCoords((int)endCoord.x, (int)endCoord.y);
        transform.LookAt(endPosition);
        floorScript.updateBlock(floorScript.getCoordAtVector(transform.position));
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
