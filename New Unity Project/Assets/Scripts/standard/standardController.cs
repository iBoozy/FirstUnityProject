using UnityEngine;
using System.Collections;

public class standardController : MonoBehaviour {
	// add this script to walk around
	// needs: Rigidbody2D, a object to check floor contact

	public float moveSpeed;
	public float jumpPower;

	public Transform groundCheck;
	public LayerMask groundLayer;
	private bool facingRight = true;
	private bool floor;
	private float groundCheckRadius = 0.4f;
	private float move;
	private Rigidbody2D myRB;
	// Use this for initialization
	void Start () {
		myRB = GetComponent <Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Jump") != 0 && floor == true) {
			floor = false;
			myRB.velocity = new Vector2 (myRB.velocity.x, 0);
			myRB.AddForce (new Vector2 (0, jumpPower), ForceMode2D.Force);
		}
		floor = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, groundLayer); 
		move = Input.GetAxis ("Horizontal");
		myRB.velocity = new Vector2 (move * moveSpeed, myRB.velocity.y);
		if ( (move < 0 && facingRight) || (move > 0 && !facingRight))
			flip ();
	}

	void FixedUpdate () {
		floor = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, groundLayer); 
		move = Input.GetAxis ("Horizontal");
		myRB.velocity = new Vector2 (move * moveSpeed, myRB.velocity.y);
		if ( (move < 0 && facingRight) || (move > 0 && !facingRight))
			flip ();
	}

	void flip () {
		facingRight = !facingRight;
		Vector3 flipScale = transform.localScale;
		flipScale.x *= -1;
		transform.localScale = flipScale;
	}
}
