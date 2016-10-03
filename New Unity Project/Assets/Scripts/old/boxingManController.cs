using UnityEngine;
using System.Collections;

public class boxingManController : MonoBehaviour {
	public float moveSpeed;
	public float jumpPower;
	public Transform groundCheck;
	public LayerMask groundLayer;

	private bool facingRight = true;
	private bool floor;
	private float groundCheckRadius = 0.4f;
	private float move;
	
	public Transform attack;					// attack area generation
	public float attackRate;					// attack speed
	public float attackBetween;					// usually smaller than attack rate; it allows attacking before you're ready to move again
	public float attackDamage;					// attack damage
	public GameObject attack1Box, attack2Box, attack3Box;	//hitboxes when attacking
	public Sprite attackHit1, attackHit2, attackHit3;		//exact frame for attack hits
	private float attackTime;					// records attack time
	private bool attacking = false;				// if the player is attacking
	private bool attackDuring = false;			// activates when a player attempts to attack; actually attackes when the frame is reached
	private float attackCount;					// current attack (in a chain)
	private int attackMax = 3;					// attack chain size

	Rigidbody2D myRB;
	Animator myAnim;
	AnimatorStateInfo currentStateInfo;
	SpriteRenderer currentSprite;
	static int currentState;
	/*
	static int moveAnim = Animator.StringToHash ("Base Layer.Move");
	static int idleAnim = Animator.StringToHash ("Base Layer.Idle");
	static int attack1Anim = Animator.StringToHash ("Base Layer.Attack Blend.Attack 1");
	static int attack2Anim = Animator.StringToHash ("Base Layer.Attack Blend.Attack 2");
	static int attack3Anim = Animator.StringToHash ("Base Layer.Attack Blend.Attack 3");
	*/
	// Use this for initialization
	void Start () {
		currentSprite = GetComponent <SpriteRenderer> ();
		myRB = GetComponent <Rigidbody2D> ();
		myAnim = GetComponent <Animator> ();
	}

	void Update () {
		if (Input.GetAxis ("Jump") != 0 && floor && !attacking) {
			floor = false;
			myRB.velocity = new Vector2 (myRB.velocity.x, 0);
			myRB.AddForce (new Vector2 (0, jumpPower), ForceMode2D.Force);
		}

		if (Input.GetAxis ("Fire1") != 0)
			PUNCH ();
	}
	// Update is called once per frame
	void FixedUpdate () {
		currentStateInfo = myAnim.GetCurrentAnimatorStateInfo (0);
		currentState = currentStateInfo.fullPathHash;
		if (attacking && attackDuring &&			// if you're attacking, but didn't actually reach the hit frame...
		( attackCount == 1 && currentSprite.sprite == attackHit1	//and the current frame is one of the hit frames...
		|| attackCount == 2 && currentSprite.sprite == attackHit2
		|| attackCount == 3 && currentSprite.sprite == attackHit3 )) {
			attack1Box.gameObject.SetActive (true);	//activate the collision box
			attackDuring = false;		//and you reached your hit frame
		}
		else {											//if not...
			attack1Box.gameObject.SetActive (false); //deactivate the collision box
		}


		if (!attacking) {	//attack disables movement
			floor = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, groundLayer); 
			move = Input.GetAxis ("Horizontal");
			myAnim.SetFloat ("speed", Mathf.Abs (move));
			myRB.velocity = new Vector2 (move * moveSpeed, myRB.velocity.y);
		}
		if ( (move < 0 && facingRight) || (move > 0 && !facingRight))
			flip ();
		if ((attackTime + (attackRate - attackBetween)) < Time.time) {
			attacking = false;
			attackCount = 0;
			myAnim.SetFloat ("attackCount", attackCount);
		}
		myAnim.SetBool ("nextAttack", false);
	}

	void flip () {
		facingRight = !facingRight;
		Vector3 flipScale = transform.localScale;
		flipScale.x *= -1;
		transform.localScale = flipScale;
	}

	void PUNCH () {
		if (attackCount < attackMax && attackTime < Time.time) {
			attackDuring = true;
			float attackRate = 0;
			attacking = true;
			myAnim.SetBool ("nextAttack", true);
			attackCount += 1;
			myAnim.SetFloat ("attackCount", attackCount);
			Debug.Log (attackCount + "... PUUUUUUUUUUUUUUUNCH");
			attackRate = 1;
			move = 0;
			attackTime = Time.time + (attackBetween / attackRate);
		}
	}
}
