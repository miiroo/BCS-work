using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dumping = 1f;
    public Vector2 offset = new Vector2(2f, 1f);
    public bool isLeft;
    private Transform player;
    private int lastX;

    //camera's limits
    [SerializeField]
    float leftLimit;
    [SerializeField]
    float rightLimit;
    [SerializeField]
    float bottomLimit;
    [SerializeField]
    float upperLimit;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        // FindPlayer(isLeft);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 240;
        Screen.SetResolution(1920, 1080, false, 240);
    }



    // Update is called once per frame
    void Update()
    {
               
        if (player) {
            int currentX = Mathf.RoundToInt(player.position.x);
            if (currentX > lastX) isLeft = true;
            else if (currentX < lastX) isLeft = false;
            lastX = Mathf.RoundToInt(player.position.x);

            Vector3 target;
            if (isLeft) {
                target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
            }
            else target = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);

            Vector3 currentPostion = Vector3.Lerp(transform.position, target, dumping * Time.deltaTime);
            transform.position = currentPostion;
        }


        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
            Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
            transform.position.z
            );

    }



    public void FindPlayer(bool playerIsLeft) {
       
            player = GameObject.FindGameObjectWithTag("Player").transform;
            lastX = Mathf.RoundToInt(player.position.x);
            if (playerIsLeft) {
                transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
            }
            else {
                transform.position = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
            }
  
    }
}
