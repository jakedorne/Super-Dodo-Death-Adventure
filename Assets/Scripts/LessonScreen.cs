using UnityEngine;
using System.Collections;

public class LessonScreen : MonoBehaviour {

	public float amplitude = 3f;        
	public float speed = 6f;                  
	private float tempVal;

	void Start () {
		tempVal = transform.position.x;
	}

	void Update () {        
		float xValue = tempVal + amplitude * Mathf.Sin(speed * Time.time);
		transform.position = new Vector3(xValue, transform.position.y, transform.position.z);
	}

}
