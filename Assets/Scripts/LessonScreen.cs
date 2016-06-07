using UnityEngine;
using System.Collections;

public class LessonScreen : MonoBehaviour {

	public float amplitude = 3f;        
	public float speed = 6f;                  
	private float tempVal;

	public enum Axis{x, y, stationary};
	Axis axis = Axis.stationary; 

	void Start(){
	}

	void Update () { 
		if (axis == Axis.x) {
			float xValue = tempVal + amplitude * Mathf.Sin (speed * Time.time);
			transform.position = new Vector3 (xValue, transform.position.y, transform.position.z);
		} else if (axis == Axis.y) {
			float yValue = tempVal + amplitude * Mathf.Sin (speed * Time.time);
			transform.position = new Vector3 (transform.position.x, yValue, transform.position.z);
		}
	}

	public void SetPosition(Vector3 position){
		transform.position = position;

	}

	public void StartBobbing(Axis a){
		this.axis = a;

		if (a== Axis.x) {
			tempVal = transform.position.x;
		} else if (a == Axis.y) {
			tempVal = transform.position.y;
		}

	}

}
