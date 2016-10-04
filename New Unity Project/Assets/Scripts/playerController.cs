using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	public float moveSpeed;									//player movement multiplier
	public float jumpPower;									//player jump strength
	public Transform groundCheck;							//used to check if the player is on a ground
	public LayerMask groundLayer;							//layer used for ground

	private bool facingRight = true;						//if the player is facing right
	private bool floor;										//if the player is on the floor
	private float groundCheckRadius = 0.4f;					//size of ground checking circle (diameter)
	private float move;										//used on movement
	private int weaponType;									//used for checking player weapon type
	private int act;										//used for checking player action
	//0 = idle. 1 = attacking. 2 = other?
	public Transform attackArea;
	public float attackRate;
	private float attackTime;

    public int jumpCount;
    private int tempJump;

	public int attackCount;
	playerStats myStat;										//gets the player's stats for various things
	playerAttack myAttack;
	public GameObject collisionBox;

	Rigidbody2D myRB;										//player's physics
	Animator myAnim;										//animation variables
	AnimatorStateInfo currentStateInfo;						//used to take the player's animation info
	SpriteRenderer currentSprite;							//used to take the player's current sprite
	static int currentState;								//static variables!

	// Use this for initialization
	void Start () {
		currentSprite = GetComponent <SpriteRenderer> ();		//initializes stuff
		myRB = GetComponent <Rigidbody2D> ();
		myAnim = GetComponent <Animator> ();
		myStat = GetComponent <playerStats> ();
		myAttack = GetComponent <playerAttack> ();
		//AnimatorStateInfo does not need to be initialized
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Jump") && (floor || tempJump > 0)) {			//if you're on the floor, or you still have an extra jump, and recently pressed the Jump button (default SPACE)...
            if (floor == false)     //if you're already off the floor, you spend a jump
                tempJump -= 1;
            else
			    floor = false;														//you go off the floor
            Debug.Log("You have jumped!");
			myRB.velocity = new Vector2 (myRB.velocity.x, 0);					//bug fix: stackable jumps
			myRB.AddForce (new Vector2 (0, (jumpPower * myStat.jumpPower)), ForceMode2D.Force);	//force pushes upwards
		}

        if (floor == true)
            tempJump = jumpCount;

		if (Input.GetAxisRaw ("Fire1") != 0 && attackTime <= Time.time) {
			attackPick ();
		}
		Debug.DrawLine (attackArea.position, new Vector2 (1, 0), Color.green);
	}

	void FixedUpdate () {
		currentStateInfo = myAnim.GetCurrentAnimatorStateInfo (0);
		currentState = currentStateInfo.fullPathHash;							//those two lines are useless, for now. : S
		weaponType = myStat.weapon;

		if (act == 0) {																//if you aren't doing anything, you can move!
			freeMove ();
		}

		if (act == 1) {																//if you're attacking, the attack continues
			myAnim.SetBool ("nextAttack", false);
			if (attackTime <= Time.time - 0.1) {
				act = 0;
				attackCount = 0;
				myAnim.SetFloat ("attackCount", attackCount);
			}
		}
	}

	void freeMove () {
		move = Input.GetAxis ("Horizontal");                                    //gets the horizontal axis and adds it to the movement
        //myRB.velocity = new Vector2 (move * (moveSpeed * myStat.moveSpeed), myRB.velocity.y);	//old: adds it to the Rigidbody2D as velocity (multiplied by base and player speeds)
        //myRB.AddForce(new Vector2(move * Time.fixedDeltaTime * myRB.mass * moveSpeed * 100, 0), ForceMode2D.Force);   //test: adds a force based on a fixed time between frames and the player's weight
        transform.Translate(new Vector2 (Mathf.Abs (move) * myStat.moveSpeed * moveSpeed * Time.fixedDeltaTime, 0), Space.Self);   //moves forward, relative to the world, based on player speeds and the fixed time between frames
		floor = Physics2D.OverlapCircle
			(groundCheck.position, groundCheckRadius, groundLayer);	
		//creates a circle at groundCheck's position, if it touches an object with the layer groundLayer (currently Ground): player is at the floor
		myAnim.SetFloat ("move", Mathf.Abs (move));
		if ( (move < 0 && facingRight)											//if the player is moving left but is turned right...
			||	(move > 0 && !facingRight))											//or if the player is moving right but is turned left...
			flip ();                                                            //flips with a function

	}

	void flip () {																//when activated: flips the player's horizontal scale (mirrors image)
		facingRight = !facingRight;												//player switches facing
		Vector3 flipScale = transform.localScale;								//new Vector3 takes the current scale
		flipScale.x *= -1;														//flips scale's x
		transform.localScale = flipScale;										//feeds the Vector3 back into the transform
		//transform.localScale.x *= -1;											//this doesn't work; not a variable (it's a function)
	}

	void attackPick () {			//picks attacks from a tree
		if (attackTime <= Time.time) {
			if (weaponType == 0) {	//if you aren't using any weapons...
				if (attackCount == 0)
					attack (1f, 1f, 1f, 1f, 1f, 3f);
				else if (attackCount == 1)
					attack (0.8f, 1.5f, 0.5f, 0f, 0.5f, 3f);
				else if (attackCount == 2)
					attack (3f, 0.8f, 1.1f, 2f, -10f, 3f);
				}
			else
				Debug.Log ("This doesn't exist");
		}
	}

	void attack (float tempDmg, float tempSpd, float tempRng, float tempPush, float tempPushUp, float tempCount) {
		if (attackCount < tempCount) {
			if (!facingRight)
				tempPush *= -1;
			Debug.Log ("Attack without a weapon");
			attackAnimUpdate ();
			GameObject hit = Instantiate (collisionBox, attackArea.position, Quaternion.identity) as GameObject;
			hit.transform.parent = gameObject.transform;
			hit.GetComponent <playerAttack> ().damage = myStat.attack * tempDmg;
			hit.GetComponent <playerAttack> ().force = hit.GetComponent <playerAttack> ().force * tempPush;
			hit.GetComponent <playerAttack> ().forceUp = hit.GetComponent <playerAttack> ().forceUp * tempPushUp;
			attackTime = Time.time + (attackRate / myStat.attackSpeed);
		}
	}

	void attackAnimUpdate () {
		act = 1;
		attackCount++;
		myAnim.SetFloat ("attackCount", attackCount);
		myAnim.SetBool ("nextAttack", true);
		myAnim.SetInteger ("weaponType", weaponType);
	}
}