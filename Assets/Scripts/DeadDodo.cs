using UnityEngine;
using System.Collections;

public class DeadDodo : MonoBehaviour {

	float restingY;

	public float range = 0.05f;
	public float speed = 3.6f;

	public float startY = -0.5f;

	float lerpTime;
	float currentLerpTime;
	bool floating;

	// Use this for initialization
	void Start () {
		lerpTime = 0.65f;
		floating = true;
		restingY = transform.position.y;
		Vector3 startPos = transform.position;
		startPos.y = startY;
		transform.position = startPos;
	}

	// Update is called once per frame
	void Update () {
		if (floating) {
			if (transform.position.y == restingY) {
				floating = false;
				return;
			}

			Vector3 newPos = transform.position;

			//Lerp tutorial from: https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/
			currentLerpTime += Time.deltaTime;
			if (currentLerpTime > lerpTime) {
				currentLerpTime = lerpTime;
			}
			float t = currentLerpTime / lerpTime;

			float newY = Mathf.Lerp (startY, restingY, t);
			newPos.y = newY;
			transform.position = newPos;

		} else {
			Vector3 moveTo = transform.position;
			float newY = restingY + (Mathf.Sin(Time.time*speed) * range);
			moveTo.y = newY;
			transform.position = moveTo;
		}


	}
}