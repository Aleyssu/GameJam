using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private int lastDir = 0;
    private float curDir;
    [SerializeField] private float speed = 1f;

    [SerializeField]
    private Transform endOfPlatformLeft;
    [SerializeField]
    private Transform endOfPlatformRight;
    [SerializeField]
    private Transform player;

    private Vector2 start;
    private Vector2 roamPosition;

    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
        roamPosition = ToRoamPosition();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, roamPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, roamPosition) <= 0.2f)
        {
            roamPosition = ToRoamPosition();
        }
    }

    private Vector2 ToRoamPosition()
    {
        // random x direction;
        if (lastDir == 0)
        {
            curDir = Random.Range(-1f, 1f);
        }
        else if (lastDir == 1)
        {
            curDir = -1f;
        }
        else
        {
            curDir = 1f;
        }

        Vector2 randomDir = new Vector2(curDir, 0).normalized;

        if (curDir >= 0)
        {
            lastDir = -1;
            return start + randomDir * Random.Range(2f, Vector2.Distance(transform.position, endOfPlatformRight.position));
        }
        else
        {
            lastDir = 1;
            return start + randomDir * Random.Range(2f, Vector2.Distance(transform.position, endOfPlatformLeft.position));
        }
        
    }

    private void FindTarget()
    {
        // to be implemented
    }

}
