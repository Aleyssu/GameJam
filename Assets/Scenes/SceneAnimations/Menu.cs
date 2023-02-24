using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour, IPointerClickHandler
{
    public PauseMenu menu;
    void Awake()
    {
        menu = GameObject.Find("Menu").GetComponent<PauseMenu>();
    }
    // Update is called once per frame
    public void OnPointerClick(PointerEventData eventData)
    {
        Time.timeScale = 0f;
        menu.anim.SetBool("Paused", true);
    }
}
