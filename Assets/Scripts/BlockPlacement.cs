﻿using UnityEngine;
using System.Collections;

public class BlockPlacement : MonoBehaviour {

    public GameObject selectorBlock; //I'm expecting this to be an opaque version of the original block

    public AudioClip blockRotation;
    public AudioClip blockPlacement;
    public AudioClip blockDenied;
    public AudioClip rockPlacement;
    AudioSource audio;

    private bool paused = false;

    private bool placingBlocks;
    private int mapSize;
    private int[,] blocks;
    private GameObject[,] selectorBlocks;
    private TetrisBlock tetrisBlock;
	private Color selectorBlockColor;

    private float volume = 0.1f;
    private float volumeVariation = 0.01f;

    private float globalVolume = 1;

    // Use this for initialization
    void Start () {

    }

    public void initialisePlacementBlocks()
    {
        selectorBlockColor = Color.white;
        selectorBlockColor.a = 0.1f;
        placingBlocks = false;
        mapSize = this.GetComponent<Floor>().getFloor().GetLength(0); //I expect this map to be square. Need to change slightly if rectangular
        blocks = new int[mapSize, mapSize];
        getFloor();
        selectorBlocks = new GameObject[mapSize, mapSize];
        setUpPlacementGrid();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (Input.GetKeyDown("space"))
            {
                if (placingBlocks)
                {
                    GameObject manager = GameObject.FindGameObjectWithTag("LevelManager");
                    manager.GetComponent<LevelManager>().levelgui.deselectBlocks();
                    turnBlockPlacementOff();
                }
            }
            //Block rotation
            if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKeyDown("a"))
            {
                if (placingBlocks)
                {
                    playRotationSound();
                    tetrisBlock.RotateLeft();
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKeyDown("d"))
            {
                if (placingBlocks)
                {
                    playRotationSound();
                    tetrisBlock.RotateRight();
                }
            }
            else if (Input.GetKeyDown("m"))
            {
                if (globalVolume == 1)
                {
                    globalVolume = 0;
                    AudioListener.volume = 0;
                }
                else
                {
                    globalVolume = 1;
                    AudioListener.volume = 1;
                }
            }
        }
    }

    /// <summary>
    /// Initial setup of the placement grid. After this point, selector blocks are simply toggled active and inactive as needed.
    /// </summary>
    private void setUpPlacementGrid()
    {
        float x = 0;
        float y = 0;
        float z = 0;

        float blockWidth = selectorBlock.GetComponent<MeshRenderer>().bounds.size.x;
        float blockDepth = selectorBlock.GetComponent<MeshRenderer>().bounds.size.z;
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                // creating new block
                GameObject block = (GameObject)Instantiate(selectorBlock, new Vector3(x, y, z), Quaternion.identity);
                selectorBlocks[i, j] = block;
                //i and j switch because the dimensions of the block grid switched
                block.GetComponent<BlockSelector>().row = j;
                block.GetComponent<BlockSelector>().col = i;
				block.GetComponent<Renderer>().material.color = selectorBlockColor; //TODO: this may need to be changed once proper materials are used.
				block.SetActive(false);
                // increment x position
                x += blockWidth;
            }
            x = 0;
            // increment z position
            z += blockDepth;
        }
    }

    /// <summary>
    /// Render a selectorBlock in any grid position that does not contain a block.
    /// </summary>
    private void togglePlacementGrid()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (blocks[i, j] == 0)
                {
                    //Will show placement grid if placingBlocks is true, or hide it otherwise.
                    //i and j switch because the dimensions of the block grid switched
                    selectorBlocks[j, i].SetActive(placingBlocks);
					selectorBlocks[j, i].GetComponent<Renderer>().material.color = selectorBlockColor; //TODO: this may need to be changed once proper materials are used.
                } else if (blocks[i, j] == 2)
                {
                    selectorBlocks[j, i].SetActive(placingBlocks);
                    selectorBlocks[j, i].GetComponent<Renderer>().material.color = Color.green; //TODO: this may need to be changed once proper materials are used.
                } else if (blocks[i, j] == 9)
                {
                    selectorBlocks[j, i].SetActive(placingBlocks);
                    selectorBlocks[j, i].GetComponent<Renderer>().material.color = Color.red; //TODO: this may need to be changed once proper materials are used.
                } else if (blocks[i, j] == 3)
                {
                    Color opaqueRed = Color.red;
                    opaqueRed.a = 0.3f;
                    selectorBlocks[j, i].SetActive(placingBlocks);
                    selectorBlocks[j, i].GetComponent<Renderer>().material.color = opaqueRed; //TODO: this may need to be changed once proper materials are used.
                }
            }
        }
    }

    /// <summary>
    /// Update the floor to make sure the grid used here is up to date.
    /// </summary>
    private void getFloor()
    {
        //Create a clone of the floor.
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                blocks[i, j] = this.GetComponent<Floor>().getFloor()[i, j];
            }
        }
    }

    public void turnBlockPlacementOn(TetrisBlock tetrisBlock)
    {
        placingBlocks = true;
        getFloor();
        togglePlacementGrid();
        this.tetrisBlock = tetrisBlock;
    }

    public void turnBlockPlacementOff(int row, int col)
    {
        bool blockAdded = this.GetComponent<Floor>().AddTetrisBlock(row, col, tetrisBlock);
        GameObject manager = GameObject.FindGameObjectWithTag("LevelManager");
        if (blockAdded)
        {
            playPlacementSound();
            placingBlocks = false;
            togglePlacementGrid();
            getFloor();
            // update inventory so that the button is no longer selected
            manager.GetComponent<LevelManager>().removeTile(tetrisBlock.type);
            manager.GetComponent<LevelManager>().levelgui.deselectBlocks();
            this.tetrisBlock = null;
        }
        else
        {
            PlayBlockDeniedSound();
        }
    }

    public void turnBlockPlacementOff()
    {
        placingBlocks = false;
        getFloor();
        togglePlacementGrid();
        this.tetrisBlock = null;
		// deselect block
		GameObject manager = GameObject.FindGameObjectWithTag ("LevelManager");
		manager.GetComponent<LevelManager> ().levelgui.deselectBlocks();
    }

    public void showHoverOver(int row, int col)
    {
        getFloor();
        //Mostly copied from Floor class
        int[,] formation = tetrisBlock.GetBlocks();
        int blockType = 0;
		if (this.GetComponent<Floor>().BlockFits(row,col,tetrisBlock))
        {
            blockType = 2; //If the block fits, show in green
        } else
        {
            blockType = 9; //If the block doesn't fit, show in red
        }
        for (int i = 0; i < formation.GetLength(0); i++)
        {
            for (int j = 0; j < formation.GetLength(1); j++)
            {
                if ((row + i >= mapSize || row + i < 0) && formation[i, j] == 1)
                {
                    // part of block is off map
                } else if ((col + j >= mapSize || col + j < 0) && formation[i, j] == 1)
                {
                    // part of block is off map
                } else if (formation[i, j] == 1 && blocks[row + i, col + j] == 1)
                {
                    // part of block is on another block
                } else if (formation[i, j] == 1 && (blocks[row + i, col + j] == 0 || blocks[row + i, col + j] == 3))
                {
                    // part of block is on another block or unplaceable spot
                    blocks[row + i, col + j] = blockType;
                }
            }
        }
        togglePlacementGrid();
    }

	public void setTetrisShape(TetrisBlock.Type shape){
		//If we don't instantiate a new one each time, any rotation will stay with each subsequent placement
		GameObject tetrisPrefab = (GameObject)Instantiate(this.GetComponent<Floor>().tetrisPrefab, new Vector3(-1000, -1000, -1000), Quaternion.identity);
		TetrisBlock tetrisBlock = tetrisPrefab.GetComponent<TetrisBlock>();
		tetrisBlock.SetShape(shape);
		turnBlockPlacementOn(tetrisBlock);
	}

    public bool isBlockPlacementOn()
    {
        if (tetrisBlock != null)
        {
            return true;
        }
        return false;
    }

	public TetrisBlock getSelectedShape(){
		return tetrisBlock;
	}

    public void updateOnDodoDeath(int row, int col)
    {
        selectorBlocks[col, row].SetActive(false);
    }

    private void playRotationSound()
    {
        audio.PlayOneShot(blockRotation, volume);
    }

    public void playPlacementSound()
    {
        audio.PlayOneShot(blockPlacement, volume);
    }

    public void PlayBlockDeniedSound()
    {
        audio.PlayOneShot(blockDenied, volume);
    }

    public void PlayRockPlacementSound()
    {
        audio.PlayOneShot(rockPlacement, volume);
    }

    public void OnGamePause()
    {
        turnBlockPlacementOff();
        paused = !paused;
    }
}
