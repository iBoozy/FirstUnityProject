using UnityEngine;
using System.Collections;

public class playerShoot : MonoBehaviour {

	public float weaponDamage;
	playerMissile myPC;
	public GameObject explosion;

	// Use this for initialization
	void Awake () {
		myPC = GetComponentInParent <playerMissile> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Shootable")) {
			if (other.tag == "Enemy") {
				enemyHealth hurtEnemy = other.gameObject.GetComponent <enemyHealth> ();
				hurtEnemy.addDamage (weaponDamage);
			}
			myPC.removeForce ();
			Instantiate (explosion, transform.position, transform.localRotation);
			Destroy (gameObject);
		}
	}
}
