using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private int lastDir = 0;
    private int curDir;
    [SerializeField] private float attackRange = 0.4f;
    [SerializeField] private float animationTime = 1f;

    private enum State
    {
        Patrol,
        Chase,
        Attacking,
        Reset,
        Idle
    }

    private State state;

    [SerializeField] private Transform endOfPlatformLeft;
    [SerializeField] private Transform endOfPlatformRight;
    [SerializeField] private Transform player;

    [SerializeField]
    private EnemyAttack enemyCombat;

    private Vector2 start;
    private Vector2 roamPosition;

    // For movement
    [SerializeField] private Animator anim;
    public MovementData data;
    public Rigidbody2D rb;
    public LayerMask groundLayer;
    private int facingDirection;

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

    void Update() {
        if(facingDirection > 0) {
            rb.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else {
            rb.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void FixedUpdate() {
        switch (state)
        {
            default:
            case State.Patrol:
                // transform.position = Vector2.MoveTowards(transform.position, roamPosition, speed * Time.deltaTime);
                
                // reached position
                if (Vector2.Distance(transform.position, roamPosition) <= 0.3f)
                {
                    Debug.Log("Entering coroutine");
                    StartCoroutine(StopMovement(0));
                    state = State.Idle;
                }
                else
                {
                    // movement animation here
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
                    // movement animation here
                    Move((new Vector2(player.position.x, transform.position.y) - rb.position).normalized);
                    // transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), speed * Time.deltaTime);
                }

                break;

            case State.Attacking:
                StartCoroutine(StopMovement(1));
                break;

            case State.Reset:
                
                //  transform.position = Vector2.MoveTowards(transform.position, start, speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, start) <= 0.3f)
                {
                    StartCoroutine(StopMovement(1));
                    state = State.Idle;
                }
                else
                {
                    // movement animation here
                    Move((start - rb.position).normalized);
                }

                break;

            case State.Idle:
                // idle animation
                anim.SetBool("Walking", false);

                FindTarget();
                break;
        }
        
    }

    private Vector2 ToRoamPosition()
    {

        Debug.Log("getting new position");
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
            return new Vector2(Random.Range(transform.position.x + 2f, endOfPlatformRight.position.x), transform.position.y);
        }
        else
        {
            lastDir = -1;
            return new Vector2(Random.Range(endOfPlatformLeft.position.x, transform.position.x - 2f), transform.position.y);
        }
        
    }

    private void FindTarget()
    {
        if (Mathf.Abs(transform.position.x - player.position.x) < 4f && Mathf.Abs(transform.position.y - player.position.y) < 0.5f)
        {
            state = State.Chase;
        }
    }

    private IEnumerator StopMovement(int type)
    {
        if (type == 0)
        {
            yield return new WaitForSeconds(2f);
            Debug.Log("new position");
            state = State.Patrol;
            roamPosition = ToRoamPosition();
        }
        else if (type == 1)
        {
            yield return new WaitForSeconds(animationTime);
            state = State.Chase;
        }
    }

    public bool isGrounded() {
		return rb.IsTouchingLayers(LayerMask.GetMask("Floor"));	
	}

    public void Move(Vector2 moveInput) {
        // Visuals
        if(moveInput.x > 0) {
            facingDirection = 1;
        }
        else {
            facingDirection = -1;
        }
        anim.SetBool("Walking", true);

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
