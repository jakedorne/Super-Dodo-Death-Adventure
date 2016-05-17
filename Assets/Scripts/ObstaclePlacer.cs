using UnityEngine;
using System.Collections;

public class ObstaclePlacer : MonoBehaviour
{

    private Floor floorScript;

    // Use this for initialization
    void Start()
    {
        GameObject Floor = GameObject.Find("Floor");
        floorScript = Floor.GetComponent<Floor>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        Vector2 blockPosition = floorScript.getCoordAtVector(transform.position);
        floorScript.createObstacle(blockPosition);
    }

}
