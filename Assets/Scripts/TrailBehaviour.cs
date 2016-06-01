using UnityEngine;
using System.Collections;

public class TrailBehaviour : MonoBehaviour {

	float startY;

	public float range = 0.03f;
	public float speed = 4f;

	// Use this for initialization
	void Start () {
		startY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveTo = transform.position;
		float newY = startY + (Mathf.Sin(Time.time*speed) * range);
		moveTo.y = newY;

		transform.position = moveTo;

	}
}
