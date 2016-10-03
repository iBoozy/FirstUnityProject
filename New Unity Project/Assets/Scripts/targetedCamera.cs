using UnityEngine;
using System.Collections;

public class targetedCamera : MonoBehaviour {

	public Transform target;
	public float smoothing;
	public float lowY;

	Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - target.position;
		lowY = transform.position.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (target != null) {		// bug fix: dying causes error spam
			Vector3 targetCamPos = target.position + offset;
			transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
			if (transform.position.y < lowY)
				transform.position = new Vector3 (transform.position.x, lowY, transform.position.z);
		}
	}
}
