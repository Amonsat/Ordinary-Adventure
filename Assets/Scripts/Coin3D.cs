using UnityEngine;
using System.Collections;

public class Coin3D : MonoBehaviour
{

	public float rotateSpeed = 2f;
	public AudioClip soundPickUp;

	void Update ()
	{
		transform.Rotate (Vector3.forward * rotateSpeed);
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (!other.gameObject.CompareTag ("Player"))
			return;

		AudioSource.PlayClipAtPoint (soundPickUp, transform.position);
		GM.instance.Coins++;
		Destroy (gameObject);
	}
}
