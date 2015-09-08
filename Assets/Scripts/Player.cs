using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


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
	private SpriteRenderer playerSpriteRenderer;

	private LimitedSizeStack<Vector3> positions = new LimitedSizeStack<Vector3> ();
	private LimitedSizeStack<Sprite> sprites = new LimitedSizeStack<Sprite> ();
	private bool returnInTime = false;

	public Slider timeControlLine;

	
	void Awake ()
	{
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		aud = GetComponent<AudioSource> ();
		playerSpriteRenderer = GetComponentInChildren<SpriteRenderer> ();
//		timeControlLine = (Slider)GameObject.FindGameObjectWithTag ("TimeControlLine");
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

		if (Input.GetButton ("TimeControl")) {
			Time.timeScale = .5f;
			rb2d.isKinematic = true;
			returnInTime = true;
			anim.enabled = false;
		}

		if (Input.GetButtonUp ("TimeControl")) {
			Time.timeScale = 1;
			rb2d.isKinematic = false;
			returnInTime = false;
			anim.enabled = true;
		}

		timeControlLine.value = positions.Count;
	}

	void FixedUpdate ()
	{
		Vector3 oldPosition = transform.position;

		if (!dead) {
			if (SystemInfo.deviceType == DeviceType.Desktop)
				moveSpeed = Input.GetAxis ("Horizontal");


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


			if (returnInTime && positions.Count > 0) {
				transform.position = positions.Pop ();
				playerSpriteRenderer.sprite = sprites.Pop ();
			}

			if (!returnInTime && transform.position != oldPosition) {
				positions.Push (transform.position);
				sprites.Push (playerSpriteRenderer.sprite);
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
		if (grounded)
			jump = true;
	}

	public void ReturnInTime ()
	{
		Time.timeScale = .5f;
		rb2d.isKinematic = true;
		returnInTime = true;
		anim.enabled = false;
	}

	public void ReturnInTimeStop ()
	{
		Time.timeScale = 1;
		rb2d.isKinematic = false;
		returnInTime = false;
		anim.enabled = true;
	}
}
