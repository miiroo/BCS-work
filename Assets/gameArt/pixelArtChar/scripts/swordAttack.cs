using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;

public class swordAttack : UnityEngine.MonoBehaviour
{

    public GameObject logText2;

    // Start is called before the first frame update
    void Start()
    {
        logText2 = GameObject.Find("LogText2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            var enemyHealth = collision.gameObject.GetComponent<Health>();
            enemyHealth.TakeDamage(12);
        }

    }
}
