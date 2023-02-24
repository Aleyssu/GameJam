using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGate : MonoBehaviour
{
    private bool playerAtExit = false;
    private bool companionAtExit = false;
    public Transition transition;
    public string nextLevel;
    private void Start()
    {
        transition = GameObject.Find("Transition").GetComponent<Transition>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerAtExit = true;
        }
        else
        {
            playerAtExit = false;
        }
        if (collision.gameObject.tag == "Companion")
        {
            companionAtExit = true;
        }
        else
        {
            companionAtExit = false;
        }
        if (playerAtExit && companionAtExit)
        {
            transition.next = nextLevel;
            transition.load = true;
        }
    }
}
