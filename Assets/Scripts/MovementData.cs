using UnityEngine;

[CreateAssetMenu(menuName = "MovementData", fileName = "ScriptableObjects/MovementData")]

public class MovementData : ScriptableObject
{
	[Header("Run")]
	public float runMaxSpeed = 15; // max running speed
	public float runForce = 7; // base force applied to player when running
	public float runDeccelForce = 14; // base force applied to stop the player
	[Header("Jump")]
	public float jumpVelocity = 15; // velocity applied when jumping
	public float coyoteTime = 0.1f; // time after leaving a platform when the player can still jump
	public float jumpBuffer = 0.05f; // time after pressing jump when the keypress still registers (in case the player is falling and touches the floor right after the player presses jump)
	[Range(0.01f, 1)] public float airAccelMult = 0.5f; // movement multiplier applied when airborne
}
