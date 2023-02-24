using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCD = 1f;
    [SerializeField] private float attackRange = 0.25f;
    [SerializeField] private int attackDamage = 1;
    private bool canAttack = true;

    [SerializeField] private Transform attackPoint;
    private GameObject playerHealthSystem;
    public LayerMask hittableLayers;
    
    // Animation
    [SerializeField] private Animator anim; 

    // Start is called before the first frame update
    void Start()
    {
        playerHealthSystem = GameObject.FindGameObjectWithTag("Health");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        if (canAttack)
        {
            anim.SetTrigger("Attack");

            // anim.ResetTrigger();
            canAttack = false;
            StartCoroutine(DelayAttack());
        }

    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(0.45f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, hittableLayers);
        bool playerDamaged = false;
        foreach (Collider2D hit in hits)
        {
            Debug.Log(hit);
            if (hit.tag == "Player" && !playerDamaged)
            {
                playerHealthSystem.GetComponent<HeartsVisual>().healthSystem.Damage(attackDamage);
                playerDamaged = true;
            }
        }

        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
