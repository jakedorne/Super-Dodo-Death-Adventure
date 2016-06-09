using UnityEngine;
using System.Collections;

public class CameraEffects : MonoBehaviour {

	private float shakeIntensity;
	private float shakeDecay;
	private Vector3 originalPosition;
	private Quaternion originalRotation;

	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
		originalRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (shakeIntensity > 0) {
			transform.position = originalPosition + Random.insideUnitSphere * shakeIntensity;
			shakeIntensity -= shakeDecay;
		}
	}

	public void Shake(){
		shakeIntensity = 0.1f;
		shakeDecay = 0.003f;
	}
}
