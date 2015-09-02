using UnityEngine;
using System.Collections;

public class DestroyByCheck : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (!other.CompareTag ("Enemy"))
			return;

		other.GetComponent<Enemy> ().die ();
//		other.gameObject.GetComponent<Animator> ().SetTrigger ("Dead");
//		other.gameObject.GetComponent<Collider2D> ().enabled = false;
//		other.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
//		other.gameObject.GetComponent<Enemy> ().dead = true;
//		other.gameObject.transform.position += new Vector3 (0, -.2f, 0);
	}
}
