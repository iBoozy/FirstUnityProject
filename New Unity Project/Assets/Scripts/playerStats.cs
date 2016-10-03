using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerStats : MonoBehaviour {

	public float maxHP, maxMP, attack, defense, jumpPower, moveSpeed,
	HP5, MP5, attackSpeed, attackRange;											//many variables! They mean power
	//Most are self explainatory. More:
	//HP5: recovers this much HP after 5 seconds
	public GameObject deathFX;													//object created when dead
	private float HP, MP;											//more power variables, only private
	public int weapon = 0;

	public Slider HPBar;														//health bar
	public Slider MPBar;														//magic bar
	playerController player;													//a player has to have player stats

	// Use this for initialization
	void Start () {
		HP = maxHP;																//HP and MP starts full
		MP = maxMP;
		player = GetComponent <playerController> ();							//initializes the player
		HPBar.maxValue = maxHP;													//updates the sliders
		MPBar.maxValue = maxMP;
	}
	
	// Update is called once per frame
	void Update () {
		HPBar.value = HP;														//the sliders are updated each frame. resource intensive/unnecessary?
		MPBar.value = MP;
		if (HP > maxHP)															//if your HP is higher than your max HP...
			HP = maxHP;															//your HP is max HP
		if (MP > maxMP)
			MP = maxMP;
		if (HP <= 0)
			defeated ();
	}

	void FixedUpdate () {
		HP += (HP5 / 5) * Time.deltaTime;
		MP += (MP5 / 5) * Time.deltaTime;
	}

	public void doDamage (float damage) {										//function to cause damage
		HP -= (Mathf.Clamp ((damage - defense), 0, Mathf.Infinity));			//HP is decreased by (damage, minus defense). Cannot be decreased to less than 0
		HPBar.value = HP;														//update the HP bar
		if (HP <= 0)															//if your HP got to 0 or less...
			defeated ();														//YOU DIED (nearby airplane noises?)
	}
	public void push (float force, float forceUp) {								//function for being pushed
		player.GetComponent <Rigidbody2D> ().velocity = new Vector2 (0, 0);		//your speed is reset
		player.GetComponent <Rigidbody2D> ().AddForce (new Vector2(force, forceUp) );	//force happens
																				//bug: players that can move are pushed up/down only because moving uses their velocity variable
	}

	void defeated () {															//function for dying : (
		MP = 0;																	//this doesn't actually affect the GUI for now: the player is deleted before he uses Update
		Destroy (gameObject);													//Completely obliterates all evidence of your existence
		Instantiate (deathFX, transform.position, transform.rotation);			//... except for a object placed, which lasts a few seconds
	}
}
