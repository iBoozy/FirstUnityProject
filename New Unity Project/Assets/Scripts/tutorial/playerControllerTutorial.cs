using UnityEngine;
using System.Collections;

public class playerControllerTutorial : MonoBehaviour {

	// Global variables
	public float maxSpeed;
	public float jumpHeight;
	public LayerMask groundLayer;
	public Transform groundCheck;
	public Transform gunTip;
	public GameObject bullet;
	public float fireRate = 0.5f;
	public float nextFire;

	private bool grounded;
	private bool facingRight = true;
	private float groundCheckRadius = 0.2f;
	private float move;

	Rigidbody2D myRB;
	Animator myAnim;

	// Use this for initialization
	void Start () {
		myRB = GetComponent <Rigidbody2D> ();
		myAnim = GetComponent <Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (grounded && Input.GetAxis ("Jump") > 0) {
			grounded = false;
			myRB.velocity = new Vector2 (myRB.velocity.x, 0f);	// when you jump, you reset your vertical speed: fixed jump stacking bug
			myAnim.SetBool ("isGrounded", grounded);
			myRB.AddForce (new Vector2 (0, jumpHeight) );
		}

		if (Input.GetAxisRaw ("Fire1") > 0) 
			fireRocket ();
	}

	void FixedUpdate () {

		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, groundLayer);
		myAnim.SetBool("isGrounded", grounded);
		move = Input.GetAxis ("Horizontal");
		myAnim.SetFloat ("speed", Mathf.Abs (move));
		myAnim.SetFloat ("speedY", myRB.velocity.y);

		myRB.velocity = new Vector2 (move * maxSpeed, myRB.velocity.y);

		if (move > 0 && !facingRight)
			flip ();
		else if (move < 0 && facingRight)
			flip ();
	}

	void flip () {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void fireRocket () {
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			if (facingRight) {
				Instantiate (bullet, gunTip.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
			} else if (!facingRight) {
				Instantiate (bullet, gunTip.position, Quaternion.Euler (new Vector3 (0, 0, 180)));
			}
		}
	}
}
