using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDestory : MonoBehaviour
{

    public static NoDestory Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
