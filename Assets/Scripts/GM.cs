using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
	private int coins = 0;
	public Text coinsText;


	public int Coins {
		get { return coins;}
		set {
			coins = value;
			coinsText.text = "Coins: " + coins;
		}
	}

	public static GM instance = null;


	// Use this for initialization
	void Awake ()
	{
		if (instance == null)
			instance = this;
		else
			Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Reload ()
	{
		Application.LoadLevel (Application.loadedLevel);
//		StartCoroutine (WaitThenReload ());
	}

	IEnumerator WaitThenReload ()
	{
		yield return new WaitForSeconds (3);
		Application.LoadLevel (Application.loadedLevel);
	}


}
