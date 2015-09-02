using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
	private int coins = 0;
	public Text coinsText;
	public GameObject gameoverui;
	public GameObject gamestartui;


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

		gamestartui.SetActive (true);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.Escape)) {
			Exit ();
		}
	}

	public void Reload ()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	public void GameOver ()
	{
		gameoverui.SetActive (true);
	}

	public void GameStart ()
	{
		gamestartui.SetActive (false);
	}

	public void Exit ()
	{
		Application.Quit ();
	}
}
