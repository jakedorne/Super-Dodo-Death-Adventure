using UnityEngine;
using System.Collections;

public class ObstaclePlacer : MonoBehaviour
{

    private Floor floorScript;
    private BlockPlacement blockPlacement;
    Vector2 blockPosition;

    private bool paused = false;

    // Use this for initialization
    void Start()
    {
        GameObject Floor = GameObject.Find("Floor");
        floorScript = Floor.GetComponent<Floor>();
        blockPlacement = Floor.GetComponent<BlockPlacement>();
        blockPosition = floorScript.getCoordAtVector(transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (!paused)
        {
            if (blockPlacement.isBlockPlacementOn())
            {
                blockPlacement.turnBlockPlacementOff((int)blockPosition.x - 1, (int)blockPosition.y - 1);
            }
            else
            {
                floorScript.createObstacle(blockPosition);
            }
        }
    }

    void OnMouseOver()
    {
        if (!paused)
        {
            if (blockPlacement.isBlockPlacementOn())
            {
                blockPlacement.showHoverOver((int)blockPosition.x - 1, (int)blockPosition.y - 1);
            }
        }
    }

    public void OnGamePause()
    {
        paused = !paused;
    }

}
