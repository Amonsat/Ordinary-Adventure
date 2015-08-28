using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	[HideInInspector]
	public bool
		facingRight = true;
	[HideInInspector]
	public bool
		jump = false;
	
	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public Transform groundCheck;
	public bool dead = false;
	
	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;

	
	void Awake ()
	{
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		if (!dead) {
			grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
		
			if (Input.GetButtonDown ("Jump") && grounded) {
				jump = true;
			}

			anim.SetBool ("Grounded", grounded);

			if (Input.GetButtonDown ("Fire1"))
				anim.SetTrigger ("MeleeAttack");
		}
	}
	
	void FixedUpdate ()
	{
		if (!dead) {
			float h = Input.GetAxis ("Horizontal");
		
			anim.SetFloat ("Speed", Mathf.Abs (h));

			transform.position += new Vector3 (h, 0, 0) * .2f;

//		if (h * rb2d.velocity.x < maxSpeed) {
//			rb2d.AddForce (Vector2.right * h * moveForce);
//		}
//		
//		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed) {
//			rb2d.velocity = new Vector2 (Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
//		}
		
			if (h > 0 && !facingRight) {
				Flip ();
			} else if (h < 0 && facingRight) {
				Flip ();
			}
		
			if (jump) {
				anim.SetTrigger ("Jump");
				rb2d.AddForce (new Vector2 (0f, jumpForce));
				jump = false;
			}
		}
	}
	
	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void Reload ()
	{
		GM.instance.Reload ();
	}
}
