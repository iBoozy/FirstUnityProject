using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour {

	Rigidbody2D myRB;
	enemyStats myStat;
    bool facingRight;
    RaycastHit2D[] detect = new RaycastHit2D[0];            //arrays! Makes an array of RaycastHit2D to store the objects the Enemy sees
    //looks like that, for what I'm using it for, it doesn't need the size... I'd have to see, though

	// Use this for initialization
	void Start () {
		myRB = GetComponent <Rigidbody2D> ();
		myStat = GetComponent <enemyStats> ();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.DrawRay(transform.position, (Vector2.left * myStat.viewRange), Color.cyan);            //only visible in Scene View: draws a ray that looks a lot like the vision ray (which isn't visible)
        detect = Physics2D.RaycastAll(transform.position, (Vector2.left * 10), myStat.viewRange);
        //creates a ray that gives data of everything it hits to the Array, going to the left of this object with a range found at it's stats
        foreach (RaycastHit2D entity in detect) {               //for each (see?) of the objects that the Ray detected...
            //each object detected by the Ray (inside detect) is known as entity, and is checked below
            if (entity.collider.tag == "Player") {              //if any of the objects has the tag Player (that's you!)...
                moveForward();                                  //activates a function to move forward!
                break;                                          //stops checking for more objects
            }
        }
	}

    void moveForward () {
        transform.Translate(new Vector2 (1 * Time.fixedDeltaTime, 0), Space.Self);      //I'm kinda lazy. : S
    }
    void flip () {                  //oddly familiar... used later? Or...
        facingRight = !facingRight;
        Vector3 flipScale = transform.localScale;
        flipScale.x *= -1;
        transform.localScale = flipScale;
    }
}
