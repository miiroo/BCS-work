using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RoomManager : Photon.PunBehaviour {
    // Start is called before the first frame update



    private PhotonView photonViewer;
    public Text player1name;
    public Text player2name;
    public GameObject tipRdy;


    void Start() {


        GameObject.Find("BowButton").GetComponentInChildren<Text>().color = Color.white;
        GameObject.Find("SwordButton").GetComponentInChildren<Text>().color = Color.white;
        GameObject.Find("WandButton").GetComponentInChildren<Text>().color = Color.white;

        GameObject buttonRdy;
        tipRdy.active = false;
        player1name.text = "";
        player2name.text = "";

        photonViewer = GetComponent<PhotonView>();
        if (PhotonNetwork.isMasterClient) {
            player1name.text = PhotonNetwork.player.NickName;
            buttonRdy = GameObject.Find("READY");
            buttonRdy.active = false;
        }
        else {
            buttonRdy = GameObject.Find("START");
            buttonRdy.active = false;
            player2name.text = PhotonNetwork.player.NickName;

        }
        PlayerPrefs.SetInt("weapon1", 1);
        PlayerPrefs.SetInt("weapon2", 1);
    }


    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        player2name.text = newPlayer.NickName;

    }


    public void chooseWeapon() {
        //0 - sword, 1 - bow, 2 - wand
        var pressedWeapon = EventSystem.current.currentSelectedGameObject;
        if (pressedWeapon != null) {
            pressedWeapon.GetComponentInChildren<Text>().color = Color.green;
            if (pressedWeapon.name == "BowButton") {
                if (PhotonNetwork.isMasterClient)
                    PlayerPrefs.SetInt("weapon1", 1);
                else PlayerPrefs.SetInt("weapon2", 1); 

                GameObject.Find("SwordButton").GetComponentInChildren<Text>().color = Color.white;
                GameObject.Find("WandButton").GetComponentInChildren<Text>().color = Color.white;
            }
            if (pressedWeapon.name == "SwordButton") {
                if (PhotonNetwork.isMasterClient)
                    PlayerPrefs.SetInt("weapon1", 0);
                else PlayerPrefs.SetInt("weapon2", 0);

                GameObject.Find("BowButton").GetComponentInChildren<Text>().color = Color.white;
                GameObject.Find("WandButton").GetComponentInChildren<Text>().color = Color.white;
            }
            if (pressedWeapon.name == "WandButton") {
                if (PhotonNetwork.isMasterClient)
                    PlayerPrefs.SetInt("weapon1", 2);
                else PlayerPrefs.SetInt("weapon2", 2);

                GameObject.Find("SwordButton").GetComponentInChildren<Text>().color = Color.white;
                GameObject.Find("BowButton").GetComponentInChildren<Text>().color = Color.white;
            }
        }
    
    
    }

    [PunRPC]
    public void addTip() {
        tipRdy.active = !tipRdy.active;
    }

    public void Ready() {
        photonViewer.RPC("addTip", PhotonTargets.All);
    }

    public void Leave() {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() {
        //when current player leave room
        PhotonNetwork.LoadLevel("Lobby");
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        //add win due to enemy leave
        PhotonNetwork.LeaveRoom();
        // PhotonNetwork.LoadLevel("Lobby");
    }


    public void StartGame() {
        if (tipRdy.active) {
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext(player1name.text);
        }
        else {
            player1name.text = (string)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
