using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public enum AIMode
{
    Hunt,
    Stay,
    Follow,
}

public class Companion : MonoBehaviour
{
    public MovementData data;
    public Rigidbody2D rb;
    public GameObject player;
    public Platform platform;
    public GameObject target;
    public Animator anim;
    private AIMode mode = AIMode.Follow; // Mostly used for UI
    public float followOffset = 2.5f;
    private float jumpdistance = 5f;
    private float lastOnGroundTime;
    public LayerMask groundLayer;
    public BoxCollider2D floorCollider;


    private void MoveTowards()
    {
        // The y axis is ignored for convience sake
        float dist = target.transform.position.x - gameObject.transform.position.x; // positive if target is to the right and negative if target is to the left
        if (target != null)
        {
            if (dist > followOffset)
            {
                Move(Vector2.right);
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (dist < -followOffset)
            {
                Move(Vector2.left);
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    private void DetectEnemy()
    {
        if (platform.objectStanding.Count != 0)
        {
            GameObject closestEnemy = null;
            float dist = Mathf.Infinity;
            for (int i = 0; i < platform.objectStanding.Count; i++)
            {
                float objDist = Mathf.Abs(platform.objectStanding[i].transform.position.x - gameObject.transform.position.x);
                if (dist > objDist)
                {
                    closestEnemy = platform.objectStanding[i];
                    dist = objDist;
                }
            }
            if (closestEnemy != null)
            {
                target = closestEnemy;
            }
            else
            {
                target = null;
            }
        }
    }
    public bool isGrounded()
    {
        return floorCollider.IsTouchingLayers(LayerMask.GetMask("Floor"));	
    }

    public void Move(Vector2 moveInput)
    {
        // Movement - force applied is calculated by runForce * the difference in velocity between the current and max
        if(Mathf.Abs(rb.velocity.x) < data.runMaxSpeed || (rb.velocity.x * moveInput.x) < 0) {
            if (lastOnGroundTime > 0.1f)
            {
                rb.AddForce(new Vector2(data.airAccelMult * moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
            }
            else
            {
                anim.SetBool("Walking", true);
                rb.AddForce(new Vector2(moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
            }
        }

        // Reduce speed when past the limit and running on ground (bhopping will preserve momentum)
        if (Mathf.Abs(rb.velocity.x) > data.runMaxSpeed && lastOnGroundTime == 0)
        {
            rb.AddForce(new Vector2(-moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
        }

        // Decelerate rapidly when not holding down movement keys
        if (Mathf.Abs(moveInput.x) < 0.01f)
        {
            rb.AddForce(new Vector2(-data.runDeccelForce * rb.velocity.x, 0));
        }
    }

    public void Jump()
    {
        if (lastOnGroundTime < data.coyoteTime)
        {
            rb.velocity = new Vector2(rb.velocity.x, data.jumpVelocity);
            lastOnGroundTime = data.coyoteTime;
        }
    }

    void OnStay(InputValue value)
    {
        mode = AIMode.Stay;
        target = gameObject;
    }

    void OnFollow(InputValue value)
    {
        mode = AIMode.Follow;
        target = player;
    }

    void OnHunt(InputValue value)
    {
        mode = AIMode.Hunt;
        if (platform != null)
        {
            DetectEnemy();
        }
    }

    public void Attack(Vector2 dir)
    {
        anim.SetTrigger("Attack");
        // Make sure the Companion is facing the right way
        if ((GetComponent<SpriteRenderer>().flipY && dir == Vector2.right) || (!GetComponent<SpriteRenderer>().flipY && dir == Vector2.left))
        {
            GetComponent<SpriteRenderer>().flipY ^= true;
        }
    }

    void Start()
    {
        // GameObject player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        // target = player;
        lastOnGroundTime = data.coyoteTime;
    }

    void Update()
    {
        // Play Animations
        // Check if on air or not
        if (rb.velocity.x == 0)
        {
            anim.SetBool("Walking", false);
        }
        if (isGrounded() == true)
        {
            anim.SetBool("InAir", false);
        }
        else
        {
            anim.SetBool("InAir", true);
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            // Move towards target when possible
            MoveTowards();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Platform>() == true)
        {
            platform = col.gameObject.GetComponent<Platform>();
            if (mode == AIMode.Hunt)
            {
                DetectEnemy();
            }
        }
    }
}