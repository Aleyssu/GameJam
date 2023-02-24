using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class Command : MonoBehaviour
{
    public TextMeshProUGUI mText;
    public Image mImage;
    void Awake()
    {
        mText = GetComponentInChildren<TextMeshProUGUI>();
        mImage = GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            if (Time.timeScale > 0)
            {
                mText.SetText("Stay");
                mImage.color = Color.green;
            }
        } 
        else if (Input.GetKey(KeyCode.N))
        {
            if (Time.timeScale > 0)
            {
                mText.SetText("Follow");
                mImage.color = Color.blue;
            }
        }
        else if (Input.GetKey(KeyCode.M))
        {
            if (Time.timeScale > 0)
            {
                mText.SetText("Hunt");
                mImage.color = Color.red;
            }
        }

    }
}
