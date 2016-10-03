using UnityEngine;
using System.Collections;

public class lifetime : MonoBehaviour {

	public float time;
	// Use this for initialization
	void Awake () {
		Destroy (gameObject, time);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
