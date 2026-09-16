using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Slider healthBarRef;
    public Health ownerHealth;
    
    public void UpdateValue(float newValue)
    {
        healthBarRef.value = newValue;
    }
}
