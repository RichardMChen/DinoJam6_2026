using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private CharacterBase player1;
    private CharacterBase player2;
    
    //[SerializeField] private HealthBarController player1HealthBar;
    //[SerializeField] private HealthBarController player2HealthBar;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
