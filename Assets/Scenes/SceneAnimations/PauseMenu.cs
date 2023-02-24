using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Animator anim;
    public Transition transition;
    bool hasResume;

    void Start()
    {
        anim = GetComponent<Animator>();
        hasResume = false;
    }
    void Update()
    {
        if (hasResume)
        {
            hasResume = false;
            Time.timeScale = 1.0f;
        }
    }
    public void OnResume()
    {
        hasResume = true;
        anim.SetBool("Paused", false);
    }

    public void OnQuit()
    {
        transition.next = "Main Menu";
        transition.load = true;
    }
}
