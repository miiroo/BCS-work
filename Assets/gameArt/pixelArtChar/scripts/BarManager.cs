using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class BarManager : Photon.PunBehaviour {

    public GameObject controlledPlayer;
    public Transform bar;
    public int maxValue = 100;
    public Vector3 newScale;
    public int rounds;
    public GameObject round1;
    public GameObject round2;
    public int colorr; //1-white, 0 - black
    // Start is called before the first frame update


    void Start() {
        colorr = 0;
        newScale = new Vector3(1, 1, 1);
        bar.localScale = new Vector3(1, 1, 1);
        //   Debug.Log(bar.name.ToString());
        rounds = 0;
    }

   

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext(newScale);
            stream.SendNext(colorr);
        }
        else {
            newScale = (Vector3)stream.ReceiveNext();
            colorr = (int)stream.ReceiveNext();
        }
    }

    void Update() {
        bar.localScale = newScale;

        if (colorr == 1) round1.GetComponent<Image>().color = Color.white;
        else round1.GetComponent<Image>().color = Color.black;
        
        if (!photonView.isMine) {
          
            return;
        }


        if (controlledPlayer != null) {
       
            if ((float)controlledPlayer.GetComponent<Health>().health / (float)maxValue < 0) newScale = new Vector3(0, (float)1.0, (float)1.0);
            else
                newScale = new Vector3((float)controlledPlayer.GetComponent<Health>().health / (float)maxValue, (float)1.0, (float)1.0);
        }

    }

    public void addRound() {
        rounds++;
        GameObject gm = GameObject.Find("GameManager");
        if (rounds == 2) {
            if (PhotonNetwork.isMasterClient) {
                Debug.Log("IM MASTER " + gameObject.GetInstanceID().ToString());
                gm.GetComponent<GameManager>().WinPlayer1();
            }
            else {
                gm.GetComponent<GameManager>().WinPlayer2();
                Debug.Log("IM NOT MASTER " + gameObject.GetInstanceID().ToString());
            }
        }
        else {
            round1.GetComponent<Image>().color = Color.white;
            colorr = 1;
        }
    }

    public void FindPlayer() {
        controlledPlayer = GameObject.FindGameObjectWithTag("Player");
        newScale = new Vector3((float)controlledPlayer.GetComponent<Health>().health / (float)maxValue, (float)1.0, (float)1.0);
        photonView.TransferOwnership(controlledPlayer.GetComponent<PhotonView>().ownerId);
    }

    // Update is called once per frame

}