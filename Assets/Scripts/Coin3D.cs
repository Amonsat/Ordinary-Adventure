using UnityEngine;
using System.Collections;

public class Coin3D : MonoBehaviour
{

	public float rotateSpeed = 2f;
	
	void Update ()
	{
		transform.Rotate (Vector3.forward * rotateSpeed);
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
//		print (other.tag);
		if (!other.gameObject.CompareTag ("Player"))
			return;
		GM.instance.Coins++;
		Destroy (gameObject);
	}
}
