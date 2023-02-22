using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public PlayerMovementData data;
    public Rigidbody2D rb;
	public BoxCollider2D floorCollider;
	public BoxCollider2D wallColliderRight;
	public BoxCollider2D wallColliderLeft;
	private float lastOnGroundTime = 1;
	private float lastSinceJumpPress = 0;
	private float jumpingTime = 0;
	private bool isJumping = false;
	private bool wallJumping = false;
	private short wallJumpingDirection = 1;
	private Vector2 moveInput;
	public LayerMask groundLayer;

	private void Update()
	{	
		// Jumping with input buffering and coyote time
        lastOnGroundTime += Time.deltaTime;
		lastSinceJumpPress += Time.deltaTime;
		jumpingTime += Time.deltaTime;
		
		if(isGrounded()) {
			lastOnGroundTime = 0;
		}
		if(Input.GetButtonDown("Jump")) {
			lastSinceJumpPress = 0;
		}
		if(isJumping && ((!Input.GetButton("Jump")) || jumpingTime > data.jumpTime)) {
			isJumping = false;
			rb.gravityScale = data.gravityScale;
		}

		// For controlling acceleration while wall jumping
		if(wallJumping && jumpingTime > data.wallJumpTime) {
			wallJumping = false;
		}

		// Regular jump
		if(lastSinceJumpPress < data.jumpBuffer) {
			if(lastOnGroundTime < data.coyoteTime) {
				Jump();
				lastSinceJumpPress = data.jumpBuffer;
				lastOnGroundTime = data.coyoteTime;
			}
			// Wall jump
			else if(wallColliderRight.IsTouchingLayers(LayerMask.GetMask("Floor"))) {
				wallJumping = true;
				wallJumpingDirection = -1;
				Jump();
			}
			else if(wallColliderLeft.IsTouchingLayers(LayerMask.GetMask("Floor"))) {
				wallJumping = true;
				wallJumpingDirection = 1;
				Jump();
			}
		}
		moveInput.x = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
	{
		// Movement - force applied is calculated by runForce * the difference in velocity between the current and max
		if(lastOnGroundTime <= 0.1f) {
			rb.AddForce(new Vector2(moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
		}
		else if(!wallJumping) {
			rb.AddForce(new Vector2(data.airAccelMult * moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
		}

		// Reduce speed when past the limit and running on ground (bhopping will preserve momentum)
		if(Mathf.Abs(rb.velocity.x) > data.runMaxSpeed && lastOnGroundTime == 0) {
			rb.AddForce(new Vector2(-moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
		}

		// Decelerate rapidly when not holding down movement keys
		if(Mathf.Abs(moveInput.x) < 0.01f && !wallJumping) {
			rb.AddForce(new Vector2(-data.runDeccelForce * rb.velocity.x, 0));
		}
    }

	public void Jump() {
		isJumping = true;
		jumpingTime = 0;
		lastSinceJumpPress = data.jumpBuffer;
		rb.gravityScale = data.gravityScaleJumping;
		if(wallJumping) {
			rb.velocity = new Vector2(data.jumpVelocity * data.wallJumpMult * wallJumpingDirection, data.jumpVelocity);
		}
		else {
			rb.velocity = new Vector2(rb.velocity.x, data.jumpVelocity);
		}
	}

	public bool isGrounded() {
		return floorCollider.IsTouchingLayers(LayerMask.GetMask("Floor"));
	}
}
