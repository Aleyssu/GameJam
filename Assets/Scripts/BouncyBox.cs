using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBox : MonoBehaviour
{
    public Rigidbody2D rb;
    
    private float moveDir;
    // Start is called before the first frame update
    private UnityEngine.Collider2D[] arr;
    public float maxSpeed = 10;
    public float moveForce = 1;
    public float jumpForce = 10;
    public float rotationalForce = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = Input.GetAxis("Horizontal");
        rb.AddForce(new Vector2((maxSpeed - Mathf.Abs(rb.velocity.x)) * moveForce * moveDir, 0));

        // Layer 3 is the floor
        if(Input.GetButtonDown("Jump") && rb.IsTouchingLayers(LayerMask.GetMask("Floor"))) {
            rb.AddForce(new Vector2(0, jumpForce));
            if(moveDir == 0) {
                rb.AddTorque(rotationalForce * (Random.Range(1, 4) - 2));
            }
            else {
                rb.AddTorque(-rotationalForce * moveDir);
                Debug.Log(moveDir);
            }
        }
    }
}
