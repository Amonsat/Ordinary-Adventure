using UnityEngine;
using System.Collections;

public class Kunai : MonoBehaviour
{

	public float speed;

	private Player player;
	private Rigidbody2D rb2d;

	void Awake ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
	}

	// Use this for initialization
	void Start ()
	{
//		GetComponent<Rigidbody> ().velocity = transform.forward * speed;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		speed *= player.facingRight ? 1 : -1;
//		rb2d.AddForce (new Vector2 (speed * 1.5f, 0), ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position += new Vector3 (speed, 0, 0) * Time.deltaTime;
		transform.Rotate (new Vector3 (0, 0, speed));
	}

	void OnCollisionEnter2D (Collision2D other)
	{
//		print (other.gameObject.tag);
		if (other.gameObject.CompareTag ("Enemy") || other.gameObject.CompareTag ("Ground"))
			Destroy (gameObject);

	}
}
