using UnityEngine;

[CreateAssetMenu(fileName = "BouncyBoxScriptableObject", menuName = "ScriptableObjects/BouncyBoxScriptableObject")]
public class BouncyBoxScriptableObject : ScriptableObject
{
    public float maxSpeed = 10;
    public float moveForce = 5;
    public float jumpForce = 1300;
    public float rotationalForce = 30;
}
