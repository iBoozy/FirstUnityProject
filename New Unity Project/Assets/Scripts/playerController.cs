using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	public float moveSpeed;									//player movement multiplier
	public float jumpPower;									//player jump strength
	public Transform groundCheck;							//used to check if the player is on a ground
    public Transform wallCheck;                             //used to check if the player is on a wall
	public LayerMask groundLayer;							//layer used for ground

	private bool facingRight = true;						//if the player is facing right
	private bool floor;										//if the player is on the floor
    private bool wall;                                      //if the player is on a wall
	private float groundCheckRadius = 0.2f;					//size of ground checking circle (diameter)
    private float wallCheckRadius = 0.01f;                   //size of wall checking circle (diameter)
	private float move;										//used for the movement axis
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
	void Update () {            //this update happens as fast as possible!
        doJump();                                           //checks jump related stuff every... faster than a frame

		if (Input.GetAxisRaw ("Fire1") != 0 && attackTime <= Time.time) {
			attackPick ();
		}
	}

	void FixedUpdate () {       //this update happens one time every frame
		currentStateInfo = myAnim.GetCurrentAnimatorStateInfo (0);
		currentState = currentStateInfo.fullPathHash;							//those two lines are useless, for now. : S
		weaponType = myStat.weapon;

        wallInteract();             //if you're on a wall, stuff changes
		if (act == 0) {												//if you aren't doing anything...
			freeMove ();                                            //you can move!
		}

		if (act == 1) {												//if you're attacking, the attack continues
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
        wall = Physics2D.OverlapCircle
            (wallCheck.position, wallCheckRadius, groundLayer);
        //does kind of the same thing for wall checking. Notice how both wall and ground uses the same layer

        myAnim.SetFloat ("move", Mathf.Abs (move));
		if ( (move < 0 && facingRight)											//if the player is moving left but is turned right...
			||	(move > 0 && !facingRight))											//or if the player is moving right but is turned left...
			flip ();                                                            //flips with a function

	}

    void doJump () {                                //this function has nearly everything jump related
        bool jump = false;                          //checks if you can jump
        bool wallJump = false;                      //checks if the jump is a wall jump

        if (floor == true && Input.GetAxis ("Jump") != 0) {         //if you're on the floor and holding the jump button...
            //for some reason Debug.Log prints more than one time, but this doesn't seem to have any gameplay changes...
            Debug.Log("Standard jump");
            jump = true;                                //you will jump!
        }
        else if (floor == false && wall == true && Input.GetButtonDown ("Jump")) {   //or, if you aren't on the floor but you're on a wall...
            Debug.Log("Wall jump");
            jump = true;                                //you will jump...
            wallJump = true;                            //but this is a wall jump
        }
        else if (floor == false && tempJump > 0 && Input.GetButtonDown ("Jump")) {      //or, if you aren't on the floor but you have extra jumps...
            Debug.Log("Air jump");
            jump = true;                                //you will jump...
            tempJump--;                                 //but this will spend a jump
        }
        //look at the order of the functions. The game will have this priority for jumps:
        //Ground jumps > wall jumps > air jumps

        if (jump) {     //if you can jump...
            float tempJumpPower = jumpPower * myStat.jumpPower;                 //takes the standard jump height and multiplies it with the player's jump multiplier
            floor = false;                                                      //you get off the floor
            Debug.Log("You have jumped!");
            myRB.velocity = new Vector2(myRB.velocity.x, 0);                    //still fixes stackable jumps!
            myRB.AddForce(new Vector2(0, tempJumpPower), ForceMode2D.Force);    //gets the previously calculated jump height and jumps

            if (wallJump) {                                                     //if the jump was a wall jump...
                myRB.velocity = new Vector2(0, myRB.velocity.y);                //also fixes stackable jumps, only sideways
                if (!facingRight)                                               //if the player is facing right, the push away effect flips
                    tempJumpPower *= -1;
                myRB.AddForce(new Vector2(-tempJumpPower / 2, 0), ForceMode2D.Force);        //pushes the player away from the wall, by half the jump power
                //bug: force is ineffective if moving towards the wall, is persistent when it does work
            }
        }
        if (floor == true)                                                      //if you're on the floor...
            tempJump = jumpCount;                                               //refill double jumps



        /*          //this is the old jump function:
        //it looked like this when added to Update:
		if (Input.GetButtonDown ("Jump") && (floor || tempJump > 0)) {          //if you recently pressed the Jump button (default SPACE)...
            doJump();                                           //attempts a jump
        }
        //the actual function:
        if (floor == false)     //if you're already off the floor, you spend a jump
            tempJump -= 1;
        else
            floor = false;                                                      //you go off the floor
        Debug.Log("You have jumped!");
        myRB.velocity = new Vector2(myRB.velocity.x, 0);                    //bug fix: stackable jumps
        myRB.AddForce(new Vector2(0, (jumpPower * myStat.jumpPower)), ForceMode2D.Force);	//force pushes upwards
        */
    }

    void wallInteract () {          //what happens when your character is at a wall
        if (wall && !floor) {       //if you're at a wall, but not on the floor...
            Vector2 speedScale = myRB.velocity;         //saves a vector which holds the current velocity
            float fallCap = -2f;                        //a falling speed cap
            if (myRB.velocity.y <= fallCap) {       //if you're falling faster than 2m/s...
                speedScale.y = Mathf.Lerp(speedScale.y, fallCap, 0.18f);            //soft cap activates, pushes closer to 2m/s... but not instantly
                //the lerp should be too weak to actually keep you on 2m/s, it's a bit faster
                myRB.velocity = speedScale;     //puts the speed back
            }
        }
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
				if (attackCount == 0)                       //do this attack sequence
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

	void attackAnimUpdate () {      //activates every time you attack: changes the variables used on animating attacks
        //this can't be activated every frame or the animation change will activate each frame
		act = 1;
		attackCount++;
		myAnim.SetFloat ("attackCount", attackCount);
		myAnim.SetBool ("nextAttack", true);
		myAnim.SetInteger ("weaponType", weaponType);
	}
}