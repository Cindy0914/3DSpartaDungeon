using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxStamina;
    private float currentHealth;
    private float currentStamina;
    
    public float MaxHealth => maxHealth;
    public float MaxStamina => maxStamina;

    public void Init()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }
    
    public void AddHealth(float value)
    {
        currentHealth = Mathf.Min(currentHealth + value, maxHealth);
        UIManager.Instance.OnPlayerHealthChanged?.Invoke(currentHealth);
    }
    
    public void ReduceHealth(float value)
    {
        currentHealth = Mathf.Max(currentHealth - value, 0);
        UIManager.Instance.OnPlayerHealthChanged?.Invoke(currentHealth);
    }
    
    public void AddStamina(float value)
    {
        currentStamina = Mathf.Min(currentStamina + value, maxStamina);
        UIManager.Instance.OnPlayerStaminaChanged?.Invoke(currentStamina);
    }
    
    public void ReduceStamina(float value)
    {
        currentStamina = Mathf.Max(currentStamina - value, 0);
        UIManager.Instance.OnPlayerStaminaChanged?.Invoke(currentStamina);
    }
}