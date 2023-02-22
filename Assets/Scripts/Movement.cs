using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public MovementData data;
    public Rigidbody2D rb;
	private float lastOnGroundTime = 1;
	public LayerMask groundLayer;
	private float jumpingTime = 0;
	private bool isJumping = false;

	private void Update()
	{	
		// For coyote time
        lastOnGroundTime += Time.deltaTime;
		
		if(isGrounded()) {
			lastOnGroundTime = 0;
		}

        // Insert your code below
		
        // Jump function call example
    }

    public bool isGrounded() {
		return rb.IsTouchingLayers(groundLayer);	
	}

    public void Move(Vector2 moveInput) {
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

	public void Jump() {
		isJumping = true;
		jumpingTime = 0;
		rb.gravityScale = data.gravityScaleJumping;
		rb.velocity = new Vector2(rb.velocity.x, data.jumpVelocity);
	}
}
