using UnityEngine;
using System.Collections;

public class ItemKunai : MonoBehaviour {

    void Update()
    {
        var z = (Mathf.PingPong(Time.time, 2f) -1f) * 35;        
        transform.rotation = Quaternion.Euler(0,0,z);
//        print(z);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        //        other.gameObject.GetComponent<Player>().AddHealth(1);
        other.gameObject.GetComponent<Player>().AddKunai();
        Destroy(gameObject);
    }
}
