using UnityEngine;
using System.Collections;

public class BlockPlacement : MonoBehaviour {

    public GameObject selectorBlock; //I'm expecting this to be an opaque version of the original block
    //For now it is just a white block. SelectorBlock MUST be the same size as the original block.

    private bool placingBlocks;
    private int mapSize;
    private int[,] blocks;
    private GameObject[,] selectorBlocks;

    // Use this for initialization
    void Start () {
        placingBlocks = false;
        getFloor();
        mapSize = blocks.GetLength(0); //I expect this map to be square. Need to change slightly if rectangular
        selectorBlocks = new GameObject[mapSize, mapSize];
        setUpPlacementGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            toggleBlockPlacement();
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
        getFloor();

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (blocks[i, j] == 0)
                {
                    //Will show placement grid if placingBlocks is true, or hide it otherwise.
                    //i and j switch because the dimensions of the block grid switched
                    selectorBlocks[j, i].SetActive(placingBlocks);
                }
            }
        }
    }

    /// <summary>
    /// Update the floor to make sure the grid used here is up to date.
    /// </summary>
    private void getFloor()
    {
        //Help. What is the best way to get the blocks grid from the Floor class.
        blocks = this.GetComponent<Floor>().getFloor();
    }

    public void toggleBlockPlacement()
    {
        //Could instead have two methods - A turnOn and turnOff, but I think a toggle will be better
        placingBlocks = !placingBlocks;
        togglePlacementGrid();
    }

}
