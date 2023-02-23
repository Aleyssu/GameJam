using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private int lastDir = 0;
    private int curDir;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackRange = 1.2f;

    private enum State
    {
        Patrol,
        Chase,
        Attacking,
        Reset,
        Idle
    }

    private State state;

    [SerializeField]
    private Transform endOfPlatformLeft;
    [SerializeField]
    private Transform endOfPlatformRight;
    [SerializeField]
    private Transform player;

    private EnemyAttack enemyCombat;

    private Vector2 start;
    private Vector2 roamPosition;

    // For movement

    public MovementData data;
    public Rigidbody2D rb;
    public LayerMask groundLayer;

    private void Awake()
    {
        state = State.Patrol;
    }
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
        roamPosition = ToRoamPosition();
    }

    void FixedUpdate() {
        switch (state)
        {
            default:
            case State.Patrol:
                // transform.position = Vector2.MoveTowards(transform.position, roamPosition, speed * Time.deltaTime);
                
                // reached position
                if (Vector2.Distance(transform.position, roamPosition) <= 0.1f)
                {
                    StartCoroutine(StopMovement());
                    state = State.Idle;
                }
                else
                {
                    Move((roamPosition - rb.position).normalized);
                }

                FindTarget();
                break;

            case State.Chase:
                if (player.position.x > endOfPlatformRight.position.x || player.position.x < endOfPlatformLeft.position.x)
                {
                    state = State.Reset;
                }
                else if (Vector2.Distance(transform.position, player.position) < attackRange)
                {
                    enemyCombat.Attack();
                    state = State.Attacking;
                }
                else
                {
                    Move((new Vector2(player.position.x, transform.position.y) - rb.position).normalized);
                    // transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), speed * Time.deltaTime);
                }

                break;

            case State.Attacking:
                break;

            case State.Reset:
                
                //  transform.position = Vector2.MoveTowards(transform.position, start, speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, start) <= 0.1f)
                {
                    StartCoroutine(StopMovement());
                    state = State.Idle;
                }
                else
                {
                    Move((start - rb.position).normalized);
                }

                break;

            case State.Idle:
                FindTarget();
                break;

        }
        
    }

    private Vector2 ToRoamPosition()
    {
        // random x direction;
        if (lastDir == 0)
        {
            if (Random.Range(-1f, 1f) > 0)
            {
                curDir = 1;
            }
            else
            {
                curDir = -1;
            }    
        }
        else if (lastDir == 1)
        {
            curDir = -1;
        }
        else
        {
            curDir = 1;
        }

        if (curDir == 1)
        {
            lastDir = 1;
            return new Vector2(curDir * Random.Range(0, endOfPlatformRight.position.x - transform.position.x), transform.position.y);
        }
        else
        {
            lastDir = -1;
            return new Vector2(curDir * Random.Range(0, transform.position.x - endOfPlatformLeft.position.x), transform.position.y);
        }
        
    }

    private void FindTarget()
    {
        if (Mathf.Abs(transform.position.x - player.position.x) < 4f && Mathf.Abs(transform.position.y - player.position.y) < 0.5f)
        {
            state = State.Chase;
        }
    }

    private IEnumerator StopMovement()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("new position");
        state = State.Patrol;
        roamPosition = ToRoamPosition();
    }

    public bool isGrounded() {
		return rb.IsTouchingLayers(LayerMask.GetMask("Floor"));	
	}

    public void Move(Vector2 moveInput) {
        // Movement - force applied is calculated by runForce * the difference in velocity between the current and max
		if(Mathf.Abs(rb.velocity.x) < data.runMaxSpeed || (rb.velocity.x * moveInput.x) < 0) {
			if(isGrounded()) {
				rb.AddForce(new Vector2(moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
			}
			else {
				rb.AddForce(new Vector2(data.airAccelMult * moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
			}
		}

		// Reduce speed when past the limit and running on ground (bhopping will preserve momentum)
		if(Mathf.Abs(rb.velocity.x) > data.runMaxSpeed && isGrounded()) {
			rb.AddForce(new Vector2(-moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
		}

		// Decelerate rapidly when not holding down movement keys
		if(Mathf.Abs(moveInput.x) < 0.01f) {
			rb.AddForce(new Vector2(-data.runDeccelForce * rb.velocity.x, 0));
		}
    }

}
