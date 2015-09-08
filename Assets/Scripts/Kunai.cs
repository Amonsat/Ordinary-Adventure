using UnityEngine;
using System.Collections;

public class Kunai : MonoBehaviour
{

	public float speed;

	private Player player;

	// Use this for initialization
	void Start ()
	{
//		GetComponent<Rigidbody> ().velocity = transform.forward * speed;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		speed *= player.facingRight ? 1 : -1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position += new Vector3 (speed, 0, 0) * Time.deltaTime;
		transform.Rotate (new Vector3 (0, 0, speed));
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.CompareTag ("Enemy"))
			Destroy (gameObject);
	}
}
