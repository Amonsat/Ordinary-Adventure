using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour
{
	void Update ()
	{
		transform.Rotate (Vector3.up * 2f);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (!other.gameObject.CompareTag ("Player"))
			return;

		other.gameObject.GetComponent<Player> ().AddHealth (1);
		Destroy (gameObject);
	}
}
