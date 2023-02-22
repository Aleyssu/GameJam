using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public MovementData data;
    public Rigidbody2D rb;
	private float lastOnGroundTime = 1;
	private float lastSinceJumpPress = 0;
	private Vector2 moveInput;
	public LayerMask groundLayer;

	private void Update()
	{	
		// Jumping with input buffering and coyote time
        lastOnGroundTime += Time.deltaTime;
		lastSinceJumpPress += Time.deltaTime;
		
		if(isGrounded()) {
			lastOnGroundTime = 0;
		}
		if(Input.GetButtonDown("Jump")) {
			lastSinceJumpPress = 0;
		}

		if(lastOnGroundTime < data.coyoteTime && lastSinceJumpPress < data.jumpBuffer) {
			rb.velocity = new Vector2(rb.velocity.x, data.jumpVelocity);
			lastSinceJumpPress = data.jumpBuffer;
			lastOnGroundTime = data.coyoteTime;
        }
		moveInput.x = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
	{
		// Movement - force applied is calculated by runForce * the difference in velocity between the current and max
		if(lastOnGroundTime > 0.1f) {
			rb.AddForce(new Vector2(data.airAccelMult * moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
		}
		else {
			rb.AddForce(new Vector2(moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
        }

		// Reduce speed when past the limit and running on ground (bhopping will preserve momentum)
		if(Mathf.Abs(rb.velocity.x) > data.runMaxSpeed && lastOnGroundTime == 0) {
			rb.AddForce(new Vector2(-moveInput.x * data.runForce * Mathf.Abs(data.runMaxSpeed * moveInput.x - rb.velocity.x), 0));
		}

		// Decelerate rapidly when not holding down movement keys
		if(Mathf.Abs(moveInput.x) < 0.01f) {
			rb.AddForce(new Vector2(-data.runDeccelForce * rb.velocity.x, 0));
		}
    }

	public bool isGrounded() {
		return rb.IsTouchingLayers(groundLayer);	
	}
}
