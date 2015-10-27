using UnityEngine;
using System.Collections;

public class ShowIfMobile : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        #if UNITY_STANDALONE
	        gameObject.SetActive(false);
        #endif
    }
}
