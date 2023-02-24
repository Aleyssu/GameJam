using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGate : MonoBehaviour
{
    public bool playerAtExit = false;
    public bool companionAtExit = false;
    public Transition transition;
    public string nextLevel;
    private void Start()
    {
        transition = GameObject.Find("Transition").GetComponent<Transition>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerAtExit = true;
        }
        if (collision.gameObject.tag == "Companion")
        {
            companionAtExit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerAtExit = false;
        }
        if (collision.gameObject.tag == "Companion")
        {
            companionAtExit = false;
        }
    }
    private void Update()
    {
        if (playerAtExit && companionAtExit)
        {
            transition.next = nextLevel;
            transition.load = true;
        }
    }
}
