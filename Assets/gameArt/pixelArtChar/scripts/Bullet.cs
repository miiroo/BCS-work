using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;


public class Bullet : Photon.PunBehaviour
{
    public GameObject logText2;
    public float maxDistance;
    public float shiftX;
    public float shiftY;
    public float speed = 10f;
    public Rigidbody2D rb;
    private float x;
    public bool animFinished;
    public bool staticSkill;
    public int dmg;

    private GameObject playerSender;
    private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        playerSender = GameObject.FindGameObjectWithTag("Player");
        logText2 = GameObject.Find("LogText2");
        photonView = GetComponent<PhotonView>();
        if (!photonView.isMine) return;
        if (!staticSkill) {
            rb.velocity = transform.right * speed;
            x = transform.position.x;
        }
    }


    private void Update() {
        if (!photonView.isMine) return;

        if (!staticSkill) {
            if (Mathf.Abs(transform.position.x - x) > maxDistance && photonView.isMine) {
                Debug.Log("Destroyed due to distance");
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else {
            if (animFinished && photonView.isMine) {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (photonView.isMine) {
             GameObject player = GameObject.FindWithTag("Player");

            if (!staticSkill) {
                if (collision.tag == "Player" && !collision.gameObject.GetComponent<PhotonView>().isMine) {
                    var enemyHealth = collision.gameObject.GetComponent<Health>();
                    enemyHealth.photonView.RPC("TakeDamage", PhotonTargets.All, dmg);
                    PhotonNetwork.Destroy(gameObject);
                }
            }

            else {
                if (collision.tag == "Player" && !collision.gameObject.GetComponent<PhotonView>().isMine) {
                    var enemyHealth = collision.gameObject.GetComponent<Health>();
                    enemyHealth.photonView.RPC("TakeDamage", PhotonTargets.All, dmg);
                }
            }
        }
    }
}
