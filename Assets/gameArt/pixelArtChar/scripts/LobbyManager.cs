using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class LobbyManager : Photon.PunBehaviour
{
    public Text pName;
    public Text lobbyText;
    public string playerName = "Player";
    // Start is called before the first frame update
    void Start()
    {
        playerName ="Player" + Random.Range(1, 100);
        if (pName.text != "") playerName = pName.text;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.playerName = playerName;
        PhotonNetwork.gameVersion = "1";
        PhotonNetwork.ConnectUsingSettings(PhotonNetwork.gameVersion);
    }
    
    public void ChangeNameF () {
        lobbyText.text += "\n";
        lobbyText.text += playerName + " changed his name to " + pName.text;
        playerName = pName.text;
        PhotonNetwork.playerName = pName.text;
    }


    public void CreateRoom() {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
        lobbyText.text += "\n" + playerName + " created a room";
    }


    public void JoinRoom() {
        PhotonNetwork.JoinRandomRoom();
        lobbyText.text += "\n" + playerName + " joined a room";
    }

    public override void OnJoinedRoom() {
        lobbyText.text += "\n";
        PhotonNetwork.LoadLevel("Room");
    }
    public override void OnConnectedToMaster() {
        lobbyText.text += "\n";
        lobbyText.text += playerName + " connected";
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
