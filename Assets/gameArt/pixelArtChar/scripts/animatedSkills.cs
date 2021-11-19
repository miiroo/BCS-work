using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatedSkills : MonoBehaviour
{

    public float speed = 10f;
    public Rigidbody2D rb;
    private float x;
    public Animator animator;
    // Start is called before the first frame update
    void Start() {
        rb.velocity = transform.right * speed;
        x = transform.position.x;
    }


    private void Update() {
        if (Mathf.Abs(transform.position.x - x) > 10) {
            Debug.Log("Destroyed due to distance");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision.name);
        Destroy(gameObject);
    }
}
