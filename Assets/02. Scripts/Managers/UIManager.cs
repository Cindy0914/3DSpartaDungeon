using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private ConditionPanel healthPanel;
    [SerializeField] private ConditionPanel staminaPanel;
    
    public Action<float> OnPlayerHealthChanged;
    public Action<float> OnPlayerStaminaChanged;

    public void Init(Player player)
    {
        healthPanel.Init(player.PlayerCondition.MaxHealth);
        staminaPanel.Init(player.PlayerCondition.MaxStamina);
        
        OnPlayerHealthChanged += healthPanel.UpdateValue;
        OnPlayerStaminaChanged += staminaPanel.UpdateValue;
    }
}
