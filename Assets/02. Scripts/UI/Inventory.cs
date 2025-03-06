using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Slot")]
    [SerializeField] private Transform slotPanel;
    
    [Header("Item Info")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private GameObject healthImage;
    [SerializeField] private GameObject staminaImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI staminaText;
    
    [Header("Button")]
    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;

    private ItemSlot[] slots;
    private ItemData selectedItem;
    private Dictionary<int, ItemSlot> existSlotDict;

    public void Init()
    {
        gameObject.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        existSlotDict = new();
        for (int i = 0; i < slotPanel.childCount; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].Init();
        }
        
        ClearItemInfo();
        useButton.onClick.AddListener(UseItem);
        dropButton.onClick.AddListener(DropItem);
    }

    public void AddItem(ItemData itemData)
    {
        if (existSlotDict.ContainsKey(itemData.ItemID))
        {
            existSlotDict[itemData.ItemID].AddStack();
        }
        else
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsEmpty())
                {
                    slots[i].SetItem(itemData);
                    existSlotDict.Add(itemData.ItemID, slots[i]);
                    slots[i].SelectButton.onClick.AddListener(() => OnItemSlotClick(itemData));
                    return;
                }
            }
            
            var player = MainSceneBase.Instance.Player;
            var pos = player.DropPoint.position;
            var interactData = player.Interaction.CurrentInteractable.GetItemData();
            Instantiate(interactData.ItemPrefab, pos, Quaternion.identity);
        }
    }

    private void OnItemSlotClick(ItemData data)
    {
        selectedItem = data;
        nameText.text = data.ItemName;
        descText.text = data.ItemDesc;
        useButton.gameObject.SetActive(true);
        dropButton.gameObject.SetActive(true);

        for (int i = 0; i < data.Consumables.Length; i++)
        {
            switch (data.Consumables[i].Type)
            {
                case ConsumableType.Health:
                    healthImage.SetActive(true);
                    healthText.text = $"체력 {data.Consumables[i].Value}";
                    break;
                case ConsumableType.Stamina:
                    staminaImage.SetActive(true);
                    staminaText.text = $"기력 {data.Consumables[i].Value}";
                    break;
            }
        }
    }

    private void ClearItemInfo()
    {
        nameText.text = "";
        descText.text = "";
        healthImage.SetActive(false);
        staminaImage.SetActive(false);
        useButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }
    
    public void UseItem()
    {
        if (!selectedItem) return;

        var playerCondition = MainSceneBase.Instance.Player.PlayerCondition;
        for (int i = 0; i < selectedItem.Consumables.Length; i++)
        {
            switch (selectedItem.Consumables[i].Type)
            {
                case ConsumableType.Health:
                    playerCondition.AddHealth(selectedItem.Consumables[i].Value);
                    break;
                case ConsumableType.Stamina:
                    playerCondition.AddStamina(selectedItem.Consumables[i].Value);
                    break;
            }
        }

        if (existSlotDict[selectedItem.ItemID].RemoveConsumeItem() != 0) return;
        RemoveItem();
    }

    public void DropItem()
    {
        if (!selectedItem) return;
        
        var pos = MainSceneBase.Instance.Player.DropPoint.position;
        Instantiate(selectedItem.ItemPrefab, pos, Quaternion.identity);
        
        if (existSlotDict[selectedItem.ItemID].RemoveConsumeItem() != 0) return;
        RemoveItem();
    }

    private void RemoveItem()
    {
        existSlotDict.Remove(selectedItem.ItemID);
        selectedItem = null;
        ClearItemInfo();
    }
    
    public void InactiveInventory()
    {
        gameObject.SetActive(false);
        ClearItemInfo();
    }
}
