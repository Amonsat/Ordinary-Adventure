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

	public AudioClip soundJump;
	public AudioClip soundDie;
	
	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;
	private AudioSource aud;
	private float moveSpeed;

	
	void Awake ()
	{
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		aud = GetComponent<AudioSource> ();
	}

	void Update ()
	{
		if (!dead) {
			grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
		
			if (Input.GetButtonDown ("Jump") && grounded) {
				jump = true;
			}

			anim.SetBool ("Grounded", grounded);
		}
	}
	
	void FixedUpdate ()
	{
		if (!dead) {

//			#if UNITY_STANDALONE_WIN
			moveSpeed = Input.GetAxis ("Horizontal");
//			#endif

			anim.SetFloat ("Speed", Mathf.Abs (moveSpeed));
			transform.position += new Vector3 (moveSpeed, 0, 0) * .2f;
		
			if (moveSpeed > 0 && !facingRight) {
				Flip ();
			} else if (moveSpeed < 0 && facingRight) {
				Flip ();
			}
		
			if (jump) {
				aud.clip = soundJump;
				aud.Play ();
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

	public void die ()
	{
		GetComponent<Player> ().dead = true;
		GetComponent<Collider2D> ().isTrigger = true;
		GetComponent<Rigidbody2D> ().isKinematic = true;
		GetComponent<Animator> ().SetTrigger ("Dead");

		GM.instance.GameOver ();
	}

	public void Move (float speed)
	{
		moveSpeed = speed;
	}

	public void Jump ()
	{
		jump = true;
	}
}
