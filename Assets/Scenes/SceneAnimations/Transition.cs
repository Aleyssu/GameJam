using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public Animator animator;
    public bool load;
    public string next;
    public float transitionDelayTime = 1.0f;
    void Awake()
    {
        animator = GetComponent<Animator>();
        load = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (load == true)
        {
            LoadLevel();
        }
    }
    public void LoadLevel()
    {
        StartCoroutine(DelayLoadLevel(next));
    }
    IEnumerator DelayLoadLevel(string sceneName)
    {
        animator.SetTrigger("Load");
        Time.timeScale = 1f;
        yield return new WaitForSeconds(transitionDelayTime);
        SceneManager.LoadScene(sceneName);
    }
}