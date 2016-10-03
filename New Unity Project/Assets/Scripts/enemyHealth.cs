using UnityEngine;
using System.Collections;

public class enemyHealth : MonoBehaviour {

	public float enemyHealthMax;
	float health;
	// Use this for initialization
	void Start () {
		health = enemyHealthMax;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (health > enemyHealthMax)	//if health is higher than maximum health...
			health = enemyHealthMax;	//health is equal to maximum health
	}

	public void addDamage (float damage) {
		health -= damage;
		if (health <= 0)
			makeDead ();
	}

	public void makeDead () {
		Destroy (gameObject);
	}
}
