using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject dodoPrefab;
    public float dodoSpawnTimer;

	// SpecialCase for first tutorial
	public bool firstTutorial = false;

	private int dodoCount;
	private GameObject[] dodoList;
	private GameObject dodo;
	private GameObject floor;
	private GameObject pathFinder;
	private int dodosToSpawn;

	private int startX;
	private int startZ;

	private bool started = false;
    private bool paused = false;

	private float countdown = 10.0f;


	// Use this for initialization
	void Start () {
		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		dodosToSpawn = managerGO.GetComponent<LevelManager> ().noDodos;
		dodoList = new GameObject [dodosToSpawn];
		floor = GameObject.Find("Floor");
		pathFinder = GameObject.Find ("PathFinder");
		startX = floor.GetComponent<Floor> ().startX;
		startZ = floor.GetComponent<Floor> ().startZ;

		// Special case for beginning tutorial
		if(FindObjectOfType<TutorialAbstract>() != null && FindObjectOfType<TutorialAbstract>().delaySpawn()){
			countdown = 3.0f;
			paused = true;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if(floor.GetComponent<Floor>().getNumberOfBlocksPlaced() > 0 && !started){
			started = true;
			beginSpawning ();
		}
		/*
		if (!started && !paused) {
			countdown -= Time.deltaTime;
			countdownText.text = "" + Mathf.Ceil (countdown).ToString ();
			if (countdown <= 0) {
				beginSpawning ();
				countdownText.text = "";
			}
		} */
	}

	public void beginSpawning(){
		started = true;
		pathFinder.GetComponent<PathFinder> ().rebuildTree ();
		InvokeRepeating ("spawnDodo", 0f, dodoSpawnTimer);
	}

	void spawnDodo() {
		dodoList[dodoCount] = (GameObject)Instantiate (dodoPrefab, floor.GetComponent<Floor> ().getVectorAtCoords (startX, startZ), Quaternion.identity);
		dodoCount++;
		if (dodoCount >= dodosToSpawn) CancelInvoke ("spawnDodo");
	}

	public bool hasStarted(){
		return started;
	}

    public void OnGamePause()
    {
        //Pause the invoke so dodos stop spawning... How??
    }

	public void resumeGame(){
		paused = false;
	}
}
