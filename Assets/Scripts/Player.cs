using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Player : MonoBehaviour
{

	[HideInInspector]
	public bool
		facingRight = true;
	public int health = 3;
	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public float fireRate;



	[Header("Audio")]
	public AudioClip
		soundJump;
	public AudioClip soundDie;
	public AudioClip soundNo;

	[Header("References")]
	public Slider
		healthSlider;
	public GameObject kunai;
	public Transform kunaiSpawn;
	public Transform groundCheck;



	private bool jump = false;
	private bool dead = false;
//	public int Health {
//		get { return health; }
//		set { 
//			health = value; 
//			healthSlider.value = health;
//		}
//	}

	private float nextFire;


	
	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;
	private AudioSource aud;
	private float moveSpeed;
	private SpriteRenderer playerSpriteRenderer;
	private AudioSource audioSource;

	private LimitedSizeStack<Vector3> positions = new LimitedSizeStack<Vector3> ();
	private LimitedSizeStack<Sprite> sprites = new LimitedSizeStack<Sprite> ();
	private bool returnInTime = false;

	public Slider timeControlLine;
	public Transform attackCheck;
	public Transform jumpAttackCheck;

	
	void Awake ()
	{
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		aud = GetComponent<AudioSource> ();
		playerSpriteRenderer = GetComponentInChildren<SpriteRenderer> ();
		audioSource = GameObject.Find ("GM").GetComponent<AudioSource> ();
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

			if (Input.GetButton ("TimeControl")) {
				ReturnInTime ();
			}
			
			if (Input.GetButtonUp ("TimeControl")) {
				ReturnInTimeStop ();
			}
			
			if (Input.GetButtonDown ("Throw")) {
				Throw ();
			}
			
			if (Input.GetButtonDown ("Attack")) {
				Attack ();
			}

			timeControlLine.value = positions.Count;
		}






	}

	void FixedUpdate ()
	{
		Vector3 oldPosition = transform.position;

		if (!dead) {
			if (SystemInfo.deviceType == DeviceType.Desktop)
				moveSpeed = Input.GetAxis ("Horizontal");


			anim.SetFloat ("Speed", Mathf.Abs (moveSpeed));
			transform.position += new Vector3 (moveSpeed, 0, 0) * .2f;
//			print (Vector2.right * moveSpeed * 100f * Time.fixedDeltaTime);
//			rb2d.AddForce (Vector2.right * moveSpeed * 1000f * Time.fixedDeltaTime);
		
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
		dead = true;
		GetComponent<Collider2D> ().isTrigger = true;
		rb2d.isKinematic = true;
		anim.SetTrigger ("Dead");

		GM.instance.GameOver ();
	}

	public void Move (float speed)
	{
		moveSpeed = speed;
	}

	public void Jump ()
	{
//		print ("jump");
		if (grounded)
			jump = true;
	}

	public void ReturnInTime ()
	{
		Time.timeScale = .5f;
		rb2d.isKinematic = true;
		returnInTime = true;
		anim.enabled = false;
		audioSource.pitch = .5f;

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies) {
			enemy.GetComponent<Enemy> ().ReturnInTime ();
		}
	}

	public void ReturnInTimeStop ()
	{
		Time.timeScale = 1;
		rb2d.isKinematic = false;
		returnInTime = false;
		anim.enabled = true;
		audioSource.pitch = 1f;

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies) {
			enemy.GetComponent<Enemy> ().ReturnInTimeStop ();
		}
	}

	public void Throw ()
	{
		if (Time.time > nextFire) {
			if (grounded) {
				anim.SetTrigger ("Throw");			
			} else {
				anim.SetTrigger ("JumpThrow");
			}
			nextFire = Time.time + fireRate;
			Instantiate (kunai, kunaiSpawn.position, kunaiSpawn.rotation);
		}
	}

	public void Attack ()
	{
		if (grounded) {
			anim.SetTrigger ("MeleeAttack");
			RaycastHit2D hit = Physics2D.Linecast (transform.position, attackCheck.position, 1 << LayerMask.NameToLayer ("Enemies"));
			if (hit.collider != null && hit.collider.CompareTag ("Enemy")) {
				hit.collider.gameObject.GetComponent<Enemy> ().TakeDamage (1);
			}
		} else {
			anim.SetTrigger ("JumpAttack");
			RaycastHit2D hit = Physics2D.Linecast (transform.position, jumpAttackCheck.position, 1 << LayerMask.NameToLayer ("Enemies"));
			if (hit.collider != null && hit.collider.CompareTag ("Enemy")) {
				hit.collider.gameObject.GetComponent<Enemy> ().TakeDamage (1);
			}
		}
	}

	public void TakeDamage (int damage)
	{
		health -= damage;
		healthSlider.value = health;
		
		aud.clip = soundNo;
		aud.Play ();

		if (health == 0) {
			die ();
		}
	}

	public void AddHealth (int heal)
	{
		health += heal;
		healthSlider.value = health;
	}

}
