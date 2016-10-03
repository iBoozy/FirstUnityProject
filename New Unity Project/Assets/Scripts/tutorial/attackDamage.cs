using UnityEngine;
using System.Collections;

public class attackDamage : MonoBehaviour {

	playerStats damage;
	// Use this for initialization
	void Start () {
		damage = GetComponentInParent <playerStats> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (damage == null)											// bug fix: first attack ever returns null
			damage = GetComponentInParent <playerStats> ();
		
		if (other.tag == "Enemy") {										//if an enemy was hit...
			enemyHealth health = other.GetComponent <enemyHealth> ();	//take his health
			if (health != null)											//if his health exists...
				health.addDamage (damage.attack);						//do damage with a function
		}
		else if (other.tag == "Player") {								//or a player was hit...
			playerStats life = other.GetComponent <playerStats> ();		//search for his stats
			if (life != null)											//if he has stats...
				life.doDamage (damage.attack);							//do damage with a function
		}
	}
}
