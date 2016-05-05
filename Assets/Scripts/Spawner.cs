using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject dodoPrefab;

	private int dodoCount;
	private GameObject[] dodoList;
	private GameObject dodo;
	private GameObject floor;
	private int dodosToSpawn;

	private int startX;
	private int startZ;


	// Use this for initialization
	void Start () {
		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		dodosToSpawn = managerGO.GetComponent<LevelManager> ().noDodos;
		dodoList = new GameObject [dodosToSpawn];
		floor = GameObject.Find("Floor");
		startX = floor.GetComponent<Floor> ().startX;
		startZ = floor.GetComponent<Floor> ().startZ;

		InvokeRepeating ("spawnDodo", 0f, 5f);
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	void spawnDodo() {
		dodoList[dodoCount] = (GameObject)Instantiate (dodoPrefab, floor.GetComponent<Floor> ().getVectorAtCoords (startX, startZ), Quaternion.identity);
		dodoCount++;
		if (dodoCount >= dodosToSpawn) CancelInvoke ("spawnDodo");
	}
}
