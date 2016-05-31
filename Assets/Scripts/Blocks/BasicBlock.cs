using UnityEngine;
using System.Collections;

public class BasicBlock : Block {

    private bool lerping = false;

    private float distanceToFall = 1f;

    private float lerpTime = 0.2f;
    private float currentLerpTime;

    private Vector3 startPosition;
    private Vector3 endPosition;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (lerping)
        {
            lerpBlock();
            if (transform.position == endPosition)
            {
                lerping = false;
            }
        }
	}
		
	public override bool interact(){
		// do nothing. is basically only an extension of block for consistency..
		return false;
	}

    public void fallToPlace()
    {
        currentLerpTime = 0;
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x,transform.position.y-distanceToFall,transform.position.z);
        lerping = true;
    }

    private void lerpBlock()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float t = currentLerpTime / lerpTime;
        //t = t * t * t * (t * (3f * t - 7f) + 5f);
        transform.position = Vector3.Lerp(startPosition, endPosition, t);
    }
}
