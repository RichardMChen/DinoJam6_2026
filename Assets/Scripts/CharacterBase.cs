using UnityEngine;

public class CharacterBase : MonoBehaviour, IDamageInterface
{
    public Health healthScript;
    public HealthBarController hpBarControllerRef;
    public Animator animatorRef;
    public bool isDead { get; set; } = false;

    public void TakeDamage(float damage)
    {
        animatorRef.SetTrigger("attacked"); //Replace with interface later
        healthScript.AdjustHealth(-damage);
        if (hpBarControllerRef)
        {
            hpBarControllerRef.UpdateValue(healthScript.currentHealth / healthScript.maxHealth);
        }
    }

    public void Dead()
    {
        animatorRef.SetBool("isDead", true);
        isDead = true;
    }
}
