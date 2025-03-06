using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private ConditionPanel healthPanel;
    [SerializeField] private ConditionPanel staminaPanel;
    [SerializeField] private InteractionPanel interactionPanel;
    
    public Action<float> OnPlayerHealthChanged;
    public Action<float> OnPlayerStaminaChanged;

    public void Init(Player player)
    {
        healthPanel.Init(player.PlayerCondition.MaxHealth);
        staminaPanel.Init(player.PlayerCondition.MaxStamina);
        interactionPanel.gameObject.SetActive(false);
        
        OnPlayerHealthChanged += healthPanel.UpdateValue;
        OnPlayerStaminaChanged += staminaPanel.UpdateValue;
    }
    
    public void SetInteraction(string itemName, string desc)
    {
        interactionPanel.SetInteraction(itemName, desc);
        interactionPanel.gameObject.SetActive(true);
    }
    
    public void InactiveInteraction()
    {
        interactionPanel.gameObject.SetActive(false);
    }
}
