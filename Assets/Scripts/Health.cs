using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            //gameObject.GetComponent<CharacterBase>().animatorRef.SetBool("isDead", true); //Replace with interface later
            gameObject.GetComponent<IDamageInterface>().Dead();
        }
    }

    public void AdjustHealth(float amount)
    {
        currentHealth += amount;
        //gameObject.GetComponent<CharacterBase>().animatorRef.SetTrigger("attacked"); //Replace with interface later
    }
}
