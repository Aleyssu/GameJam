using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public List<GameObject> objectStanding = new List<GameObject>();

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            objectStanding.Add(col.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
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
