using UnityEngine;

[CreateAssetMenu(menuName = "PlayerMovementData", fileName = "ScriptableObjects/PlayerMovementData")]

public class PlayerMovementData : ScriptableObject
{
	[Header("Run")]
	public float runMaxSpeed; // max running speed
	public float runForce; // base force applied to player when running
	public float runDeccelForce; // base force applied to stop the player
	[Header("Jump")]
	public float jumpVelocity; // velocity applied when jumping
	public float coyoteTime = 0.1f; // time after leaving a platform when the player can still jump
	public float jumpBuffer = 0.1f; // time after pressing jump when the keypress still registers (in case the player is falling and touches the floor right after the player presses jump)
	[Range(0.01f, 1)] public float airAccelMult; // movement multiplier applied when airborne
}
