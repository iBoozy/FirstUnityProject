using UnityEngine;
using System.Collections;

public class enemyAttack : MonoBehaviour {
	public float damage, damageRate, force, forceUp;
	private float damageTime;
	// Use this for initialization
	void Start () {
		damageTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.tag == "Player" && damageTime < Time.time) {
			playerStats player = other.gameObject.GetComponent <playerStats> ();
			player.doDamage (damage);
			pushBack (other.transform);
			damageTime = Time.time + damageRate;
		}
	}

	void pushBack (Transform target) {
		Vector2 pushDirection = new Vector2((target.position.x - transform.position.x),
			(target.position.y - transform.position.y) ).normalized;
		pushDirection.x *= force;
		pushDirection.y *= forceUp;
		Rigidbody2D targetRB = target.gameObject.GetComponent <Rigidbody2D> ();
		targetRB.velocity = Vector2.zero;
		targetRB.AddForce (pushDirection, ForceMode2D.Impulse);
	}
}
