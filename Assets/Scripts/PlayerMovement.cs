using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public PlayerMovementData data;
    public Rigidbody2D rb;
	public BoxCollider2D mainBodyCollider;
	public BoxCollider2D floorCollider;
	public BoxCollider2D wallColliderRight;
	public BoxCollider2D wallColliderLeft;
	public Animator anim;
	private float lastOnGroundTime = 1;
	private float lastSinceJumpPress = 0;
	private float jumpingTime = 0;
	private bool isJumping = false;
	private bool wallJumping = false;
	private int wallJumpingDirection = 1;
	private int facingDirection = 1;
	private float jumpMult = 1;
	private float movementMult = 1;
	
	private Vector2 moveInput;
	public LayerMask groundLayer;
	
	// Crouching
	private bool isCrouching = false;
	private Vector2 originalHitbox;
	private Vector2 crouchedHitbox;

	// SFX
	public AudioSource srcWalk;
	public AudioSource srcJump;
	public AudioSource srcLand;

	public void Awake() {
		srcWalk.clip = data.walkSFX;
		srcJump.clip = data.jumpSFX;
		srcLand.clip = data.landSFX;

		originalHitbox = mainBodyCollider.size;
		crouchedHitbox = new Vector2(mainBodyCollider.size.x, mainBodyCollider.size.y * data.crouchedHitboxMult);
	}

	private void Update()
	{	
		// Jumping with input buffering and coyote time
        lastOnGroundTime += Time.deltaTime;
		lastSinceJumpPress += Time.deltaTime;
		jumpingTime += Time.deltaTime;

		// Checking if player on ground
		if(isGrounded()) {
			if(anim.GetBool("InAir")) {
				srcLand.Play();
				anim.SetBool("InAir", false);
			}
			lastOnGroundTime = 0;
		}
		else
		{
            anim.SetBool("InAir", true);
        }

		// Crouching
		if(Input.GetAxisRaw("Vertical") < 0) {
			anim.SetBool("Crouched", true);
			isCrouching = true;
			mainBodyCollider.size = crouchedHitbox;
			movementMult = data.crouchedMovementMult;
		}
		else {
			anim.SetBool("Crouched", false);
			isCrouching = false;
			mainBodyCollider.size = originalHitbox;
			movementMult = 1;
		}

		// Check for jump input
		if(Input.GetButtonDown("Jump")) {
			lastSinceJumpPress = 0;
        }
		// Increase gravity so player falls when releasing jump
		if(isJumping && ((!Input.GetButton("Jump")) || jumpingTime > data.jumpTime)) {
			isJumping = false;
			anim.ResetTrigger("Jump");
			rb.gravityScale = data.gravityScale;
		}

		// For controlling acceleration while wall jumping
		if(wallJumping && jumpingTime > data.wallJumpTime) {
			wallJumping = false;
        }

		// Regular jump
		if(lastSinceJumpPress < data.jumpBuffer) {
			if (lastOnGroundTime < data.coyoteTime)
			{
				Jump();
				lastSinceJumpPress = data.jumpBuffer;
				lastOnGroundTime = data.coyoteTime;
			}
			// Wall jump
			else if (wallColliderRight.IsTouchingLayers(LayerMask.GetMask("Floor")) || wallColliderRight.IsTouchingLayers(LayerMask.GetMask("Companion")))
			{
				anim.ResetTrigger("Jump");
				wallJumping = true;
				wallJumpingDirection = -1 * facingDirection;
				Jump();
			}
			else if (wallColliderLeft.IsTouchingLayers(LayerMask.GetMask("Floor")) || wallColliderLeft.IsTouchingLayers(LayerMask.GetMask("Companion")))
			{
				anim.ResetTrigger("Jump");
				wallJumping = true;
				wallJumpingDirection = 1 * facingDirection;
				Jump();
			}
        }
		moveInput.x = Input.GetAxisRaw("Horizontal");
		// Ignore jumping until jump animations for player are released
		if (moveInput.x != 0 )
		{
			anim.SetBool("isRunning", true);
		}
		else
		{
            anim.SetBool("isRunning", false);
        }
    }

    private void FixedUpdate()
	{
		// Movement - force applied is calculated by runForce * the difference in velocity between the current and max
		if(Mathf.Abs(moveInput.x) > 0.1f && (Mathf.Abs(rb.velocity.x) < data.runMaxSpeed || (rb.velocity.x * moveInput.x) < 0)) {
			// Air accel multiplier
			if(lastOnGroundTime <= 0.1f) {
				rb.AddForce(new Vector2(moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
			}
			// Air accel multiplier immediately following a walljump
			else if(wallJumping) {
				rb.AddForce(new Vector2(data.wallJumpAccelMult * moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
			}
			// Walking on land
			else {
				rb.AddForce(new Vector2(movementMult * data.airAccelMult * moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
			}
		}
		// Flipping facing direction
		if(moveInput.x > 0 && !wallJumping) {
			facingDirection = 1;
			rb.transform.localRotation = Quaternion.Euler(0, 0, 0);
		} 
		else if(moveInput.x < 0 && !wallJumping) {
			facingDirection = -1;
			rb.transform.localRotation = Quaternion.Euler(0, 180, 0);
		}

		// Reduce speed when past the limit and running on ground (bhopping will preserve momentum)
		if(Mathf.Abs(rb.velocity.x) > data.runMaxSpeed && lastOnGroundTime == 0) {
			Debug.Log(new Vector2(-moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
			rb.AddForce(new Vector2(-moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
		}

		// Decelerate rapidly when not holding down movement keys
		if(Mathf.Abs(moveInput.x) < 0.01f && !wallJumping) {
			rb.AddForce(new Vector2(-data.runDeccelForce * rb.velocity.x, 0));
		}
    }

	public void Jump() {
		anim.SetTrigger("Jump");
		isJumping = true;
		jumpingTime = 0;
		lastSinceJumpPress = data.jumpBuffer;
		rb.gravityScale = data.gravityScaleJumping;
		if(rb.IsTouchingLayers(LayerMask.GetMask("Companion"))) {
			jumpMult = data.companionJumpBoostMult;
			srcJump.clip = data.jumpBoostSFX;
		}
		else {
			jumpMult = 1;
			srcJump.clip = data.jumpSFX;
		}
		srcJump.Play();

		if(wallJumping) {
			rb.velocity = new Vector2(data.jumpVelocity * data.wallJumpMult * wallJumpingDirection * jumpMult, data.jumpVelocity * jumpMult);
			facingDirection = wallJumpingDirection;
			if(facingDirection > 0) {
				rb.transform.localRotation = Quaternion.Euler(0, 0, 0);
			}
			else {
				rb.transform.localRotation = Quaternion.Euler(0, 180, 0);
			}
		}
		else {
			rb.velocity = new Vector2(rb.velocity.x, data.jumpVelocity * jumpMult);
		}
	}

	public bool isGrounded() {
		return floorCollider.IsTouchingLayers(LayerMask.GetMask("Floor")) || floorCollider.IsTouchingLayers(LayerMask.GetMask("Companion"));
	}

	public void walkSFX() {
		srcWalk.Play();
	}
}
