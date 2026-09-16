using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackControls : MonoBehaviour
{
    public Animator animator;
    public GameObject attackPoint;
    public float attackPointRadius;
    public LayerMask enemyLayer;
    public float attackDamage = 1.0f;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        animator.SetBool("isAttacking", true);
    }

    public void AttackDamageCollision()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackPointRadius, enemyLayer);

        foreach (Collider2D enemyObj in enemy)
        {
            //Debug.Log("Hit enemy: " + enemyObj.name);
            //enemyObj.GetComponent<Health>().maxHealth -= attackDamage;
            //enemyObj.GetComponent<Health>().TakeDamage(attackDamage);
            if (!enemyObj.GetComponent<CharacterBase>().isDead)
            {
                enemyObj.GetComponent<IDamageInterface>().TakeDamage(attackDamage);
            }
        }
    }

    public void EndAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackPointRadius);
    }
}
