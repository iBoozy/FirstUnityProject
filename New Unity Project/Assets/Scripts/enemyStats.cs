using UnityEngine;
using System.Collections;

public class enemyStats : MonoBehaviour {

	public float maxHP, maxMP, attack, defense;							//many variables!
	public bool canJump, canMove;
	public GameObject deathFX;
	enemyController enemy;
	public int[] dropList;
	public float[] dropRate;
	public GameObject item;
	int i, j, k;
	private float HP, MP;												//more variables
	// Use this for initialization
	void Start () {
		HP = maxHP;																//HP and MP starts full
		MP = maxMP;
		enemy = GetComponent <enemyController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (HP > maxHP)
			HP = maxHP;
		if (MP > maxMP)
			MP = maxMP;
	}

	public void doDamage (float damage) {
		float fullDmg = Mathf.Clamp ((damage - defense), 0, Mathf.Infinity);
		Debug.Log ("Takes " + fullDmg + " points of damage");
		HP -= fullDmg;
		if (HP <= 0)
			defeated ();
		}
	public void push (float force, float forceUp) {
		enemy.GetComponent <Rigidbody2D> ().velocity = new Vector2 (0, 0);	
		enemy.GetComponent <Rigidbody2D> ().AddForce (new Vector2(force, forceUp) );
	}
	void defeated () {
		Destroy (gameObject);
		itemDrop ();
		Instantiate (deathFX, transform.position, transform.rotation);
	}

	void itemDrop () {
		for (i = 0; i < dropList.Length; i++) {											//checks each item
			if (Random.value <= dropRate [i])											//if a random value equals or is lower than the drop rating...
				Instantiate (item, transform.position, transform.rotation);				//database is not set up; can only drop one item
		}
	}
}
