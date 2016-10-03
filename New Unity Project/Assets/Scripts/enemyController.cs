using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour {

	Rigidbody2D myRB;
	enemyStats myStat;

	// Use this for initialization
	void Start () {
		myRB = GetComponent <Rigidbody2D> ();
		myStat = GetComponent <enemyStats> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
