using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CompAttackRange : MonoBehaviour
{
    public Companion companion;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            float objDist = col.transform.position.x - gameObject.transform.position.x;
            if (objDist < 0)
            {
                companion.Attack(Vector2.left);
            }
            else
            {
                companion.Attack(Vector2.right);
            }
        }
    }
}
