using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

	public float bounds;
	public float speed;
	public bool dead;

	private bool facingRight = true;
	private int direction = 1; 
	private float startPositionX;
	// Use this for initialization
	void Start ()
	{
		startPositionX = transform.position.x;
		direction = Random.Range (0, 1) == 0 ? -1 : 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!dead)
			transform.position += new Vector3 (speed, 0, 0) * direction;

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
//		print (other.gameObject.tag);
		if (!other.gameObject.CompareTag ("Player"))
			return;

		other.gameObject.GetComponent<Player> ().dead = true;
		other.gameObject.GetComponent<Collider2D> ().isTrigger = true;
		other.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
		other.gameObject.GetComponent<Animator> ().SetTrigger ("Dead");
//		GM.instance.Reload ();
//		yield WaitForSeconds(5);
//		Application.LoadLevel (Application.loadedLevel);
	}
}
