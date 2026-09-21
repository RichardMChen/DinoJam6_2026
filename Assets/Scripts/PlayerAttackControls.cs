using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackControls : MonoBehaviour
{
    public Animator animator;
    public GameObject attackPoint;
    public float attackPointRadius;
    public LayerMask enemyLayer;
    public float attackDamage = 1.0f;
    public float attackCooldown = 0.5f; //The cooldown time between attacks
    private float attackCooldownTimer = 0f; //Timer to keep track of the cooldown duration
    public bool canAttack = false;
    public bool isAttacking = false;

    [Header("Sound")]
    [SerializeField] AudioSource audioSourceRef;
    [SerializeField] AudioClip attackSound;

    // Update is called once per frame
    void Update()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        else
        {
            canAttack = true;
        }
    }

    public void Attack()
    {
        if (!GameManager.instance.canFight)
        {
            return;
        }

        if (canAttack)
        {
            animator.SetBool("isAttacking", true);
            isAttacking = true;
            PlayAttackSound();
        }
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

    public void PlayAttackSound()
    {
        if (audioSourceRef && attackSound && !audioSourceRef.isPlaying)
        {
            audioSourceRef.PlayOneShot(attackSound, 0.5f);
        }
    }

    //private bool attackSoundPlaying = false;
    //IEnumerator PlayAttackSoundCo()
    //{
    //    if (attackSoundPlaying)
    //    {
    //        yield return null;
    //    }

    //    if (audioSourceRef && attackSound)
    //    {
    //        audioSourceRef.PlayOneShot(attackSound, 0.5f);
    //        attackSoundPlaying = true;
    //    }

    //    yield return new WaitForSeconds(3.5f);
    //    attackSoundPlaying = false;
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackPointRadius);
    }
}
