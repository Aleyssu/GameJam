using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public List<GameObject> objectStanding = new List<GameObject>();

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Entity")
        {
            objectStanding.Add(col.gameObject);
        }
    }

    private void OnCollisionExit(Collision col)
    {
        for (int i = 0; i < objectStanding.Count; i++)
        {
            if (col.gameObject == objectStanding[i])
            {
                objectStanding.RemoveAt(i);
            }
        }
    }
}
