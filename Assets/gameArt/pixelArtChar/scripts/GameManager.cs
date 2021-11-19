using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;
using UnityEngine.UI;
public class GameManager : Photon.PunBehaviour
{

    public GameObject cam;
    public GameObject resp1;
    public GameObject resp2;
    public GameObject logText;
  //  private GameObject player;

    public GameObject winText;
    public GameObject loseText;
    public GameObject hp1;
    public GameObject hp2;

    public PhotonView photonViewer;
    private float startTime = 180;
    private float currentTime;
    
    // Start is called before the first frame update
    void Start()
    {
        winText.active = false;
        loseText.active = false;
        photonViewer = GetComponent<PhotonView>();
        currentTime = startTime;
        if (PhotonNetwork.isMasterClient) {
            PhotonNetwork.Instantiate("Prefabs/GameChar", resp1.transform.position, Quaternion.identity, 0);
            hp1.GetComponent<BarManager>().FindPlayer();
 
        }
        else {
            PhotonNetwork.Instantiate("Prefabs/GameChar", resp2.transform.position, Quaternion.identity, 0);
            hp2.GetComponent<BarManager>().FindPlayer();
        }
        cam.GetComponent<CameraController>().FindPlayer(false);
        logText = GameObject.Find("Timer");


    }

    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext(currentTime);
        }
        else {
            currentTime = (float)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update() {
        string showingTime;
        if (!photonViewer.isMine) {
             showingTime = Mathf.Round(currentTime).ToString();
            logText.GetComponent<Text>().text = showingTime;
            return;
        }
        showingTime = Mathf.Round(currentTime).ToString();
        logText.GetComponent<Text>().text = showingTime;
        if (currentTime > 0) {
            currentTime -= 1*Time.deltaTime;
        }
        else {
            PhotonNetwork.LoadLevel(1);
        }
    }

    [PunRPC]
    public void Resp() {
        GameObject a;
        a = GameObject.FindGameObjectWithTag("Player");
        a.GetComponent<Health>().Respawn();
    }
    [PunRPC]
    public void resetTime() {
        currentTime = startTime;
    }

    public void r() {
        photonViewer.RPC("Resp", PhotonTargets.All);
        photonViewer.RPC("resetTime", PhotonTargets.All);
    }

    public void WinPlayer1() {
        showWin();
        photonViewer.RPC("showLose", PhotonTargets.Others);
        Invoke("Leave", 5);
    }

    public void WinPlayer2() {
        photonViewer.RPC("showLose", PhotonTargets.MasterClient); 
        showWin();
        Invoke("Leave", 5);
    }

    [PunRPC]
    public void showLose() {
        loseText.active = true;
    }
    
    [PunRPC]
    public void showWin() {
        winText.active = true;
    }

    public void Leave() {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() {
        //when current player leave room
        PhotonNetwork.LoadLevel("Lobby");
    }


    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) { 
        PhotonNetwork.LeaveRoom();
    }
}
