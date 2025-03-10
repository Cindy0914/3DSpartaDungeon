using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Camera uiCamera;
    [SerializeField] private ConditionPanel healthPanel;
    [SerializeField] private ConditionPanel staminaPanel;
    [SerializeField] private InteractionPanel interactionPanel;
    [SerializeField] private Inventory inventory;
    [SerializeField] private BuffPanel buffPanel;
    
    public Action<float> OnPlayerHealthChanged;
    public Action<float> OnPlayerStaminaChanged;
    public Action<ItemData> OnItemAdded;
    public Action<BuffType, float> OnBuffProgressChanged;
    
    public bool IsInventoryActive => inventory.gameObject.activeInHierarchy;

    public void Init(Player player)
    {
        healthPanel.Init(player.PlayerCondition.MaxHealth);
        staminaPanel.Init(player.PlayerCondition.MaxStamina);
        interactionPanel.HideInteractionUi();
        inventory.Init();
        buffPanel.Init();
        
        OnPlayerHealthChanged += healthPanel.UpdateValue;
        OnPlayerStaminaChanged += staminaPanel.UpdateValue;
        OnItemAdded += inventory.AddItem;
        OnBuffProgressChanged += buffPanel.UpdateBuffProgress;
    }
    
    public void ActiveInteractScreenUI(string itemName, string desc)
    {
        interactionPanel.SetScreenUI(itemName, desc);
    }
    
    public void ActiveInteractWorldUI(string desc, Vector3 position)
    {
        interactionPanel.SetWorldUI(desc, position);
    }
    
    public void InactiveInteraction()
    {
        interactionPanel.HideInteractionUi();
    }
    
    public void OnInputInventory()
    {
        if (IsInventoryActive)
            inventory.InactiveInventory();
        else
            inventory.gameObject.SetActive(true);
    }
}
