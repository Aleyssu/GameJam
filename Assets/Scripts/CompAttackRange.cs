using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CompAttackRange : MonoBehaviour
{
    private bool canAttack = true;
    [SerializeField] private float attackCD = 1f;
    [SerializeField] private float attackRange = 0.25f;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask hittableLayers;

    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource src;

    public Companion companion;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            float objDist = col.transform.position.x - gameObject.transform.position.x;
            if (objDist < 0)
            {
                companion.Attack(Vector2.left);
            }
            else
            {
                companion.Attack(Vector2.right);
            }

            companion.target = col.gameObject;
            Attack();
        }
    }

    private void Attack()
    {
        if (canAttack)
        {
            Debug.Log("?");
            companion.isAttacking = true;
            anim.SetTrigger("Attack");

            canAttack = false;
            StartCoroutine(DelayAttack());
        }

    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(0.3f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, hittableLayers);
        foreach (Collider2D hit in hits)
        {
            Debug.Log("hitting enemy");
            hit.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(attackCD);
        canAttack = true;
        companion.isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void attackSFX() {
        src.Play();
    }
}
