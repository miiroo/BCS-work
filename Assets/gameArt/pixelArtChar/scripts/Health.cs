using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;

public class Health : Photon.PunBehaviour, IPunObservable
{
    public bool isRestarted;
    public GameObject logText;
    public int health = 100;
    bool isRed = false;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        //sync health
        if (stream.isWriting) {
            stream.SendNext(health);
        }
        else {
            health = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void TakeDamage(int dmg) {
        changeColor();
        health -= dmg;
        Invoke("changeColor", (float)0.2);
    }
     
    public void changeColor() {
        if (!isRed) {
            GetComponent<SpriteRenderer>().color = Color.red;
            isRed = true;
        }
        else {
            isRed = false;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void Respawn() {
       
        GetComponent<playerController>().enabled = false;
        if (PhotonNetwork.isMasterClient) {
            if (health > 0) GameObject.Find("HP1").GetComponent<BarManager>().addRound();
            transform.position = GameObject.Find("Respawn1").transform.position;
        }
        else {
            if (health > 0 ) GameObject.Find("HP2").GetComponent<BarManager>().addRound();
            transform.position = GameObject.Find("Respawn2").transform.position;
        }
        health = 100;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<playerController>().enabled = true;

    }

    // Start is called before the first frame update
    void Start()
    {
        isRestarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine) return;


        if (health <= 0) {
            GameObject a;
            a = GameObject.Find("GameManager");
            a.GetComponent<GameManager>().r();
        }

        
    }
}
