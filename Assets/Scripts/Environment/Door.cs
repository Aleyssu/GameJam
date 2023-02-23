using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;
    private Collider2D collision;
    private void Start()
    {
        anim = GetComponent<Animator>();
        collision = GetComponent<Collider2D>();
    }
    public void open()
    {
        anim.SetTrigger("Activate");
        Destroy(collision);
    }
}
