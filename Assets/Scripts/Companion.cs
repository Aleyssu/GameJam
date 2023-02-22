using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public enum AIMode
{
    Hunt,
    Stay,
    Follow,
}

public class Companion : MonoBehaviour
{
    GameObject player;
    Platform platform;
    GameObject target;
    public AIMode mode; // Mostly used for UI
    float followOffset;
    float speed;
    bool isAttacking;


    private float MoveTowards()
    {
        // The y axis is ignored for convience sake
        float dist = target.transform.position.x - gameObject.transform.position.x; // positive if target is to the right and negative if target is to the left
        float newPosition = gameObject.transform.position.x;
        if (target != null)
        {
            if (dist > followOffset)
            {
                newPosition += speed;
            }
            else if (dist < -followOffset)
            {
                newPosition -= speed;
            }
        }
        return newPosition;
    }

    private void DetectEnemy()
    {
        if (platform.objectStanding.Count != 0)
        {
            GameObject closestEnemy = platform.objectStanding[0];
            float dist = Mathf.Abs(closestEnemy.transform.position.x - gameObject.transform.position.x);
            for (int i = 1; i < platform.objectStanding.Count; i++)
            {
                float objDist = Mathf.Abs(platform.objectStanding[i].transform.position.x - gameObject.transform.position.x);
                if (dist > objDist)
                {
                    closestEnemy = platform.objectStanding[i];
                    dist = objDist;
                }
            }
            target = closestEnemy;
        }
    }

    private void Attack()
    {
        // Call the attack animation
    }

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        target = player;
    }

    void Update()
    {
        if (isAttacking == false)
        {
            // Move towards target and when input is set, change mode
            MoveTowards();
        }
        // Check for any nearby enemies and see if there are any enemies to swing at
        GameObject detectEnemies;
        bool enemyInRange = false;
        // Insert enemyinRange script
        if (enemyInRange == true)
        {
            isAttacking = true;
            Attack();
        }
        // Priority: Stay > Follow > Hunt (in the event multiple button are pressed for some reason)
        if (false)
        {
            mode = AIMode.Stay;
            target = gameObject;
        }
        else if (false)
        {
            mode = AIMode.Follow;
            target = player;
        }
        else if (false)
        {
            mode = AIMode.Hunt;
            if (platform != null)
            {
                DetectEnemy();
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Platform>() == true)
        {
            platform = gameObject.GetComponent<Platform>();
            if (mode == AIMode.Hunt)
            {
                DetectEnemy();
            }
        }
    }

    private void OnCollisionExit(Collision col)
    {
        platform = null;
        if (mode == AIMode.Hunt)
        {
            target = null;
        }
    }
}