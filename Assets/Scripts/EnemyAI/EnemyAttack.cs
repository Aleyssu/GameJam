using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCD = 2f;
    private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        if (canAttack)
        {
            // trigger attack animation here

            canAttack = false;
            StartCoroutine(DelayAttack());
        }

    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }
}
