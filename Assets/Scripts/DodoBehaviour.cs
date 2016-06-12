using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DodoBehaviour : MonoBehaviour {

	private Floor floorScript;

	private PathFinder pathFinder;

    public AudioClip dodoDeath;
    public AudioClip watermelonCollected;
    AudioSource audio;
    private float volume = 0.1f;
    private float volumeVariation = 0.01f;

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

    private bool paused;

	int interval = 1; 
	float nextTime = 0;

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
        paused = false;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            anim.SetBool("isWalking", false);
        } else
        {
            if (transform.position.y < -0.5)
            {
				Vector3 rotation = transform.eulerAngles;
                Destroy(this.gameObject);
                LevelManager script = FindObjectOfType<LevelManager>();
                script.dodoDeath();
                if (spawnBlockOnDeath)
                {
                    //Might need to wait for animation to finish before calling this.
                    Vector2 pos = floorScript.getCoordAtVector(transform.position);
					floorScript.createDeadDodoBlock(pos, rotation);
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
            if (endPosition == Vector3.zero)
            {
                if (floorScript.isTree(floorScript.getCoordAtVector(transform.position)))
                {
                    LevelManager script = FindObjectOfType<LevelManager>();
                    script.dodoDeath();
                    Destroy(this.gameObject);
                }
                anim.SetBool("isWalking", true);
                //Walking off an edge. Have to scale it down so the dodo doesnt shoot off the end
                //Just fall, no movement necessary
                //transform.position += transform.forward * 0.02f / lerpTime;
            }
			else if (!atSamePos(transform.position, endPosition))
            {
                anim.SetBool("isWalking", true);
                moveDodo(startPosition, endPosition);
            }
            else
            {
                anim.SetBool("isWalking", false);
                currentLerpTime = 0f;
                moveCycle();
            }
        }
    }

	public bool noFloorBelow(Vector2 position){
		Vector2 coord = floorScript.getCoordAtVector (transform.position);
		return floorScript.getFloor () [((int)coord.x), ((int)coord.y)] == 0;
	}

	public bool atSamePos(Vector3 position1, Vector3 position2){

		double x1 = System.Math.Round (position1.x, 4);
		double x2 = System.Math.Round (position2.x, 4);

		double z1 = System.Math.Round (position1.z, 2);
		double z2 = System.Math.Round (position2.z, 2);

		bool x = false;
		bool z = false;			

		if (Mathf.Abs ((float) (x1 - x2)) <= 0.001) {
			x = true;
		}

		if (z1 == z2) {
			z = true;
		}
			
		return (x && z);
	}

	//These methods are not currently working.
	private void moveCycle() {

        //If we are not standing on a block, we should stop moving and have no goal.
        if (!floorScript.isBlock(floorScript.getCoordAtVector(transform.position)))
        {
            playDodoDeathSound();
            endPosition = Vector3.zero;
            return;
        }

        //Vector2 endCoord = pathFinder.findNextMove (floorScript.getCoordAtVector (transform.position));
        getEndCoord();

        //If we are told that the goal is our own block, we don't know where to go
        //For now, just go forwards
        if (endPosition == transform.position)
        {
            endPosition = floorScript.getCoordAtVector(transform.position + transform.forward);
        }

		startPosition = transform.position;
		//endPosition = floorScript.getVectorAtCoords((int)endPosition.x, (int)endPosition.z);
        transform.LookAt(endPosition);
        floorScript.updateBlock(floorScript.getCoordAtVector(transform.position));
        //moveDodo(transform.position, floorScript.getVectorAtCoords((int)bestBlock.x, (int)bestBlock.y));
    }

    private void getEndCoord()
    {
        List<Vector2> potentialBlocks = new List<Vector2>();
        Vector2 bestBlock = MAX_VECTOR2;
        Vector2 pos = floorScript.getCoordAtVector(transform.position);
        if (!floorScript.isBlock(pos))
        {
            endPosition = Vector3.zero;
            print("Stop and die, thanks Mr Dodo");
            return;
        }
        Vector2 left = floorScript.getCoordAtVector(transform.position + transform.right * -1);
        Vector2 right = floorScript.getCoordAtVector(transform.position + transform.right);
        Vector2 forward = floorScript.getCoordAtVector(transform.position + transform.forward);

        //print ("World - Left: " + (transform.position + transform.right)*-1 + " Right: " + (transform.position + transform.right) + " Forward: " + (transform.position + transform.forward));
        //print ("Grid - Left: " + left + " Right: " + right + " Forward: " + forward);

        if (floorScript.positionOnBlock((int)forward.x, (int)forward.y))
        {
            //can go forward, therefore go forward
            //print("Yo you good to go forward, to: " + forward);
            bestBlock = forward;
        }
        else
        {
            if (floorScript.positionOnBlock((int)left.x, (int)left.y))
            {
                if (floorScript.positionOnBlock((int)right.x, (int)right.y))
                {
                    //can go both left and right, choose random
                    //print("Yo you good to go either, pick one");
                    int randBlock = Random.Range(0, 2);
                    if (randBlock == 0) bestBlock = left;
                    else bestBlock = right;
                }
                else
                {
                    //can only go left
                    //print("Yo you good to go left to: " + left);
                    bestBlock = left;
                }
            }
            else if (floorScript.positionOnBlock((int)right.x, (int)right.y))
            {
                //can only go right
                //print("Yo you good to go right to: " + right);
                bestBlock = right;
            }
            else
            {
                //walk off edge
                //print("Yo kys");
                if (!floorScript.isTree(forward))
                {
                    //If you can go forward, go forward.
                    bestBlock = forward;
                } else if (floorScript.isTree(left) && !floorScript.isTree(right))
                {
                    //If you can't go forward or left, go right.
                    bestBlock = right;
                } else if (!floorScript.isTree(left) && floorScript.isTree(right))
                {
                    bestBlock = left;
                } else if (!floorScript.isTree(left) && !floorScript.isTree(right))
                {
                    int randBlock = Random.Range(0, 2);
                    if (randBlock == 0) bestBlock = left;
                    else bestBlock = right;
                } else
                {
                    Destroy(this.gameObject);
                    LevelManager script = FindObjectOfType<LevelManager>();
                    script.dodoDeath();
                    return;
                }
            }
        }

        //potentialBlocks = removeLastPos (potentialBlocks);
        /*
        if (floorScript.isTree(bestBlock))
        {
            Destroy(this.gameObject);
            LevelManager script = FindObjectOfType<LevelManager>();
            script.dodoDeath();
            return;
        }
        */
        //bestBlock = findBestBlock (potentialBlocks);

        lastX = currentX;
        lastZ = currentZ;
        currentX = (int)bestBlock.x;
        currentZ = (int)bestBlock.y;

        startPosition = transform.position;
        endPosition = floorScript.getVectorAtCoords((int)bestBlock.x, (int)bestBlock.y);
    }

	private void moveDodo(Vector3 startPosition, Vector3 endPosition) {
		//Lerp tutorial from: https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/

		currentLerpTime += Time.deltaTime;
		if (currentLerpTime > lerpTime) {
			currentLerpTime = lerpTime;
		}

		float t = currentLerpTime / lerpTime;
		transform.position = Vector3.Lerp (startPosition, endPosition, t);
	}

    /* we aren't using this at the moment. Old method.
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
    */

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collectable")
        {
            LevelManager script = FindObjectOfType<LevelManager>();
            script.watermelonCollected();
            Destroy(other.gameObject);
            playWatermelonCollectedSound();
        }
    }

    private void playDodoDeathSound()
    {
        //This may need to be played through another object, as the dodo gets destroyed, which stops the sound.
        audio.PlayOneShot(dodoDeath, volume);
    }

    private void playWatermelonCollectedSound()
    {
        audio.PlayOneShot(watermelonCollected, volume*3);
    }

    public void OnGamePause()
    {
        paused = !paused;
    }
		
}
