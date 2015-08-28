using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
	public float rotateSpeed = 2f;

	void Update ()
	{
		transform.Rotate (Vector2.up * rotateSpeed);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (!other.gameObject.CompareTag ("Player"))
			return;
		GM.instance.Coins++;
		Destroy (gameObject);
	}
}
