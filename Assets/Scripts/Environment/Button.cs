using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Door door;
    private Animator anim;
    private Collider2D collision;
    void Start()
    {
        anim = GetComponent<Animator>();
        collision = GetComponent<Collider2D>();
    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.name == "Player" || col.gameObject.name == "Companion")
        {
            anim.SetTrigger("Activate");
            door.open();
            Destroy(collision);
        }
    }
}
