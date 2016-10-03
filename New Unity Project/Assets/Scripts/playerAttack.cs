using UnityEngine;
using System.Collections;

public class playerAttack : MonoBehaviour {

	public float damage = Mathf.NegativeInfinity, attackLife, force, forceUp;
	playerController player;

	// Use this for initialization
	void Start () {
		player = GetComponentInParent <playerController> ();
		Destroy (gameObject, attackLife);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Enemy" && damage != Mathf.NegativeInfinity) {
			other.GetComponent <enemyStats> ().doDamage (damage);
			other.GetComponent <enemyStats> ().push (force, forceUp);
			Destroy (gameObject);

		}
	}
}
