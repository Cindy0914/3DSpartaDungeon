using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxStamina;
    [SerializeField] private float passiveStaminaRecovery;
    [SerializeField] private float runningStaminaCost;
    private float currentHealth;
    private float currentStamina;
    private bool isRunning;
    
    public Action OnStaminaEmpty; // 스태미나가 0이 됐을 때 실행 할 이벤트
    public float MaxHealth => maxHealth;
    public float MaxStamina => maxStamina;

    public void Init()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        OnStaminaEmpty += () => isRunning = false;
    }

    private void Update()
    {
        if (isRunning)
            ReduceStamina(runningStaminaCost * Time.deltaTime);
        else
        {
            if (currentStamina < maxStamina)
                AddStamina(passiveStaminaRecovery * Time.deltaTime);
        }
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
        
        if (IsStaminaEmpty())
            OnStaminaEmpty?.Invoke();
    }
    
    public void SetRunning(bool value)
    {
        isRunning = value;
    }
    
    public bool IsStaminaEmpty()
    {
        return currentStamina <= 0;
    }
}