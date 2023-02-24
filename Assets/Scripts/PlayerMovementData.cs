using UnityEngine;

[CreateAssetMenu(menuName = "PlayerMovementData", fileName = "ScriptableObjects/PlayerMovementData")]

public class PlayerMovementData : ScriptableObject
{
	[Header("Run")]
	public float runMaxSpeed = 15; // max running speed
	public float runForce = 7; // base force applied to player when running
	public float runDeccelForce = 14; // base force applied to stop the player
	[Range(0.01f, 1)] public float crouchedMult = 0.5f; // movement multiplier when crouched
	
	[Header("Jump")]
	public float jumpVelocity = 15; // velocity applied when jumping
	public float jumpTime = 0.2f; // maximum time player can hold jump
	public float wallJumpMult = 0.5f; // multiplier applied to jumpVelocity to determine wall jump strength
	[Range(0.01f, 1)] public float wallJumpAccelMult = 0.3f;
	public float wallJumpTime = 0.4f; // time before player regains horizontal movement control after wall jumpinh
	public float companionJumpBoostMult = 2;
	public float gravityScaleJumping = 1; // gravity scale when jumping
	public float gravityScale = 5; // gravity scale when falling
	public float coyoteTime = 0.1f; // time after leaving a platform when the player can still jump
	public float jumpBuffer = 0.05f; // time after pressing jump when the keypress still registers (in case the player is falling and touches the floor right after the player presses jump)
	[Range(0.01f, 1)] public float airAccelMult = 0.5f; // movement multiplier applied when airborne
	
	[Header("SFX")]
	public AudioClip jumpSFX;
    public AudioClip jumpBoostSFX;
	public AudioClip landSFX;
	public AudioClip walkSFX;
}
