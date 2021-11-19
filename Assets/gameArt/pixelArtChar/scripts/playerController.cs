using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class playerController : Photon.PunBehaviour
{
    //0 - sword, 1 - bow, 2 - wand
    public int weapon = 0;
    public int skill = -1;
    private string idleAn = "playerIdle";
    private string runAn = "playerRun";
    private string attAn = "playerAttack";
    public GameObject logText;
    public GameObject logText2;
    public GameObject bullet;
    public Transform firePoint;
    public string prefabName;
    private bool swordAutoAttack = false;


    private float maxSpeed = 10;
    public float speed;
    public float jumpForce;
    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public int extraJumps;
    public bool animFinished = true;
    public bool attackAnimation = false;


    private string currAnim = "idleAnim";
    private Vector3 pos;
    private PhotonView photonViewer;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;
    private SpriteRenderer spr;



    

    public int dmgGet = 0;
    public int dmgGive = 0;

    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animFinished = true;
        attackAnimation = false;
        logText = GameObject.Find("LogText");
        logText2 = GameObject.Find("LogText2");
        photonViewer = GetComponent<PhotonView>();
        PhotonNetwork.sendRate = 59;
        PhotonNetwork.sendRateOnSerialize = 59;

        if (PhotonNetwork.isMasterClient) {
            weapon = PlayerPrefs.GetInt("weapon1");
          //  gameObject.tag = "player1";
        }
        else {
            weapon = PlayerPrefs.GetInt("weapon2");
          //  gameObject.tag = "player2";
        }

        OnWeapon();

    }

    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            if (rb!=null)
            pos = rb.position;
            
            stream.Serialize(ref pos);
            stream.SendNext(currAnim);
        }
        else {
            pos = Vector3.zero;
            stream.Serialize(ref pos);
            currAnim = (string)stream.ReceiveNext();      
        }
    }



    void FixedUpdate()
    {
        if (!photonViewer.isMine) {
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (isGrounded == true)
        {
           // print("we are on the ground");
            extraJumps = 0;
        }
        /*
        if (Input.GetKey("1")) {
            weapon = 0;
            OnWeapon();
        }
        if (Input.GetKey("2")) {
            weapon = 1;
            OnWeapon();
        }
        if (Input.GetKey("3")) {
            weapon = 2;
            OnWeapon();
        }
        */

        
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!photonViewer.isMine) {
            rb.position = pos;
            animator.Play(currAnim, 0);
            return;
        }

        float moveX = Input.GetAxis("Horizontal");
            // movement {
            if ((Input.GetKey("a") || Input.GetKey("d")) && !Input.GetKey("space") && animFinished) {
                attackAnimation = false;
                if (!isGrounded) {
                    animator.Play("playerJump");
                    currAnim = "playerJump";
                }
                Run(moveX);
            }

            if (Input.GetKeyDown("w") && extraJumps > 0 && !Input.GetKey("space") && animFinished) {
                attackAnimation = false;
                animator.PlayInFixedTime("playerJump");
                currAnim = "playerJump";
                Jump();
            }
            else if (Input.GetKeyDown("w") && extraJumps == 0 && isGrounded == true && !Input.GetKey("space") && animFinished) {
                attackAnimation = false;
                animator.Play(idleAn);
                currAnim = idleAn;
                Jump();
            }
            //  } movement end

            // attack
            
            if (Input.GetKey("space") || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)
            && !Input.GetKey("d") && !Input.GetKey("a") && !Input.GetKeyDown("w")) 
            {
                if (isGrounded) {
                    animator.Play(attAn);
                    currAnim = attAn;
                    attackAnimation = true;
                    if (Input.GetKey(KeyCode.UpArrow)) {
                        skill = 1;
                    }
                    if (Input.GetKey(KeyCode.DownArrow)) {
                        skill = 2;
                    }
                    if (Input.GetKey(KeyCode.LeftArrow)) {
                        skill = 3;
                    }
                    if (Input.GetKey(KeyCode.RightArrow)) {
                        skill = 4;
                    }
                    if (Input.GetKey("space")) {
                        skill = 0;
                    }

                if (animFinished && attackAnimation) {
                    Shoot(skill, weapon);
                }
            }
            }
            // attack end

            // animation 
            if ( !Input.GetKey("d") && !Input.GetKey("a") && !Input.GetKeyDown("w") && !Input.GetKey("space") && !Input.GetKey(KeyCode.LeftShift)) {
                if (!attackAnimation) {
                    if (isGrounded) { 
                        animator.Play(idleAn);
                        currAnim = idleAn;
                    }
                    else { 
                        animator.Play("playerJump");
                        currAnim = "playerJump";
                    }
                }
                else if (animFinished) {
                    skill = -1;
                    attackAnimation = false;
                    if (isGrounded) { 
                        animator.Play(idleAn);
                        currAnim = idleAn;
                    }
                    else { 
                        animator.Play("playerJump");
                        currAnim = "playerJump";
                    }
                }
            }
            // animation end

    }


    void Run(float moveX)
    {
        
        if (moveX > 0)
        {
            if (isGrounded) {
                animator.Play(runAn);
                currAnim = runAn;
            }
           // print("Speed: " + rb.velocity.x);
            if (transform.localRotation.y != 0)
            {
                transform.Rotate(0f, -180f, 0f);
            }

            if (moveX * speed < maxSpeed)
            {
                var dir = new Vector2(moveX * speed, rb.velocity.y);
                rb.velocity = dir;
            }
            else
            {
                var dir = new Vector2(maxSpeed, rb.velocity.y);
                rb.velocity = dir;
            }
        }
        else
        {
            if (moveX < 0)
            {
                if (isGrounded) {
                    animator.Play(runAn);
                    currAnim = runAn;
                }
             //   print("Speed: " + rb.velocity.x);
                if (transform.localRotation.y == 0)
                {
                    transform.Rotate(0f, 180f, 0f);
                }

                if (moveX * speed > -maxSpeed)
                {
                    var dir = new Vector2(moveX * speed, rb.velocity.y);
                    rb.velocity = dir;
                }
                else
                {
                    var dir = new Vector2(-maxSpeed, rb.velocity.y);
                    rb.velocity = dir;
                }

            }
        }
    }

    void OnWeapon() {
        switch (weapon) {
            case 0:
                idleAn = "playerIdle";
                runAn = "playerRun";
                attAn = "playerAttack";
                break; 
            case 1:
                idleAn = "playerIdleBow";
                runAn = "playerRunBow";
                attAn = "playerAttackBow";
                break;
            case 2:
                idleAn = "playerIdleWand";
                runAn = "playerRunWand";
                attAn = "playerAttackWand";
                break;
            default: break;
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        extraJumps--;

    }


    void Shoot(int skillN, int weaponN) {
        if (weaponN == 0) { //sword
            swordAutoAttack = false;
            if (skillN == 0) swordAutoAttack = true;
            if (skillN == 1) prefabName = "Prefabs/swordAttack1"; //up
            if (skillN == 2) prefabName = "Prefabs/swordieSkill4"; //down
            if (skillN == 3) prefabName = "Prefabs/swordSkill2"; //left
            if (skillN == 4) prefabName = "Prefabs/swordSkill3"; //right
        }
        if (weaponN == 1) { // bow
            swordAutoAttack = false;
            if (skillN == 0) prefabName = "Prefabs/arrow";
            if (skillN == 1) prefabName = "Prefabs/arrowSkill1"; //up
            if (skillN == 2) prefabName = "Prefabs/sprt"; //down
            if (skillN == 3) prefabName = "Prefabs/archerSkill3"; //left
            if (skillN == 4) prefabName = "Prefabs/archerSkill4"; //right
        }
        if (weaponN == 2) { // wand
            swordAutoAttack = false;
            if (skillN == 0) prefabName = "Prefabs/fireBall";
            if (skillN == 1) prefabName = "Prefabs/wandSkill1"; //up
            if (skillN == 2) prefabName = "Prefabs/wandSkill2"; //down
            if (skillN == 3) prefabName = "Prefabs/wandSkill3"; //left
            if (skillN == 4) prefabName = "Prefabs/wandSkill4"; //right
        }
        if (!swordAutoAttack) {
            bullet = Resources.Load<GameObject>(prefabName);
            Vector3 newPosition = firePoint.position;
            newPosition.z = 0;
            if (transform.rotation.y == 0) newPosition.x += bullet.GetComponent<Bullet>().shiftX;
            else newPosition.x -= bullet.GetComponent<Bullet>().shiftX;

            newPosition.y -= bullet.GetComponent<Bullet>().shiftY;

            newPosition.z = 0;

            PhotonNetwork.Instantiate(prefabName, newPosition, transform.rotation, 0);
        }

    }



    public void OnTriggerEnter2D(Collider2D collision) {

    }

}
