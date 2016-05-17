﻿using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject dodoPrefab;
    public float dodoSpawnTimer;

	private int dodoCount;
	private GameObject[] dodoList;
	private GameObject dodo;
	private GameObject floor;
	private int dodosToSpawn;

	private int startX;
	private int startZ;

	private bool started = false;


	// Use this for initialization
	void Start () {
		GameObject managerGO = GameObject.FindGameObjectWithTag ("LevelManager");
		dodosToSpawn = managerGO.GetComponent<LevelManager> ().noDodos;
		dodoList = new GameObject [dodosToSpawn];
		floor = GameObject.Find("Floor");
		startX = floor.GetComponent<Floor> ().startX;
		startZ = floor.GetComponent<Floor> ().startZ;

	}
	
	// Update is called once per frame
	void Update () {

	
	}

	public void beginSpawning(){
		started = true;
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
}
