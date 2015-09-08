using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{

	public float bounds;
	public float speed;
	public bool dead;
	public AudioClip soundOh;

	private bool facingRight = true;
	private int direction = 1; 
	private float startPositionX;
	private SpriteRenderer spriteRenderer;
	private Rigidbody2D rb2d;
	private Animator anim;

	private LimitedSizeStack<Vector3> positions = new LimitedSizeStack<Vector3> ();
	private LimitedSizeStack<Sprite> sprites = new LimitedSizeStack<Sprite> ();
	private bool returnInTime = false;


	void Awake ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
	}
	// Use this for initialization
	void Start ()
	{
		startPositionX = transform.position.x;
		direction = Random.Range (0, 1) == 0 ? -1 : 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void FixedUpdate ()
	{
		Vector3 oldPosition = transform.position;

		if (!dead) {
			transform.position += new Vector3 (speed, 0, 0) * direction * Time.deltaTime;
		
			if (transform.position.x > startPositionX + bounds) {
				direction = -1;
			} else if (transform.position.x < startPositionX - bounds) {
				direction = 1;
			}
		
			if (direction > 0 && !facingRight) {
				Flip ();
			} else if (direction < 0 && facingRight) {
				Flip ();
			}

			if (returnInTime && positions.Count > 0) {
				transform.position = positions.Pop ();
				spriteRenderer.sprite = sprites.Pop ();
			}

			if (!returnInTime && transform.position != oldPosition) {
				positions.Push (transform.position);
				sprites.Push (spriteRenderer.sprite);
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

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.CompareTag ("Kunai"))
			die ();

		if (!other.gameObject.CompareTag ("Player"))
			return;
		other.gameObject.GetComponent<Player> ().die ();
	}

	public void die ()
	{
		AudioSource.PlayClipAtPoint (soundOh, transform.position);
		GetComponent<Animator> ().SetTrigger ("Dead");
		GetComponent<Collider2D> ().enabled = false;
		GetComponent<Rigidbody2D> ().isKinematic = true;
		GetComponent<Enemy> ().dead = true;
		transform.position += new Vector3 (0, -.2f, 0);
	}

	public void ReturnInTime ()
	{
		rb2d.isKinematic = true;
		returnInTime = true;
		anim.enabled = false;
	}
	
	public void ReturnInTimeStop ()
	{
		rb2d.isKinematic = false;
		returnInTime = false;
		anim.enabled = true;
	}
}
