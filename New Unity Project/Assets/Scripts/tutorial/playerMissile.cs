using UnityEngine;
using System.Collections;

public class playerMissile : MonoBehaviour {
	public float projSpeed;
	Rigidbody2D myRB;

	// Use this for initialization
	void Awake () {
		myRB = GetComponent <Rigidbody2D> ();
		if (transform.localRotation.z != 0)		// if the rocket is not pointing directly forward...
			projSpeed *= -1;					// flip the force multiplier
		myRB.AddForce (new Vector2 (1, 0)* projSpeed, ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void removeForce () {
		myRB.velocity = new Vector2 (0, 0);
	}
}
