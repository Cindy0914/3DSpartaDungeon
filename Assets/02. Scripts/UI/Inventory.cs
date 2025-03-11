using System;
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

    [Header("Consumable Info")]
    [SerializeField] private GameObject healthImage;
    [SerializeField] private GameObject staminaImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI staminaText;

    [Header("Buff Info")]
    [SerializeField] private GameObject speedImage;
    [SerializeField] private GameObject jumpImage;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI jumpText;

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
        // 아이템이 슬롯에 이미 존재하는 경우
        if (existSlotDict.ContainsKey(itemData.ItemID))
        {
            // 스택이 가능하다면 스택을 추가
            if (!existSlotDict[itemData.ItemID].TryAddStack())
            {
                existSlotDict.Remove(itemData.ItemID);
                FindNewSlot();
            }
        }
        else
        {
            FindNewSlot();
        }
        return;
        
        // 새로운 슬롯을 찾는 함수
        void FindNewSlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (!slots[i].IsEmpty()) continue;
                
                slots[i].SetItem(itemData);
                existSlotDict.Add(itemData.ItemID, slots[i]);
                slots[i].SelectButton.onClick.AddListener(() => OnItemSlotClick(itemData));
                return;
            }

            var player = MainSceneBase.Instance.Player;
            var pos = player.DropPoint.position;
            Instantiate(itemData.ItemPrefab, pos, Quaternion.identity);
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
            var consumable = data.Consumables[i];
            switch (consumable.Type)
            {
                case ConsumableType.Health:
                    healthImage.SetActive(true);
                    healthText.text = $"체력 {consumable.Value.ToString()}";
                    break;
                case ConsumableType.Stamina:
                    staminaImage.SetActive(true);
                    staminaText.text = $"기력 {consumable.Value.ToString()}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(consumable.Type), consumable.Type, null);
            }
        }

        for (int i = 0; i < data.Buffs.Length; i++)
        {
            var buff = data.Buffs[i];
            switch (buff.Type)
            {
                case BuffType.SpeedUp:
                    speedImage.SetActive(true);
                    speedText.text = $"속도 {buff.Value.ToString()}";
                    break;
                case BuffType.JumpUp:
                    jumpImage.SetActive(true);
                    jumpText.text = $"점프력 {(buff.Value / 10).ToString()}";
                    break;
                case BuffType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(buff.Type), buff.Type, null);
            }
        }
    }

    private void ClearItemInfo()
    {
        nameText.text = "";
        descText.text = "";
        healthImage.SetActive(false);
        staminaImage.SetActive(false);
        speedImage.SetActive(false);
        jumpImage.SetActive(false);
        useButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }

    private void UseItem()
    {
        if (!selectedItem) return;

        var playerCondition = MainSceneBase.Instance.Player.PlayerCondition;
        for (int i = 0; i < selectedItem.Consumables.Length; i++)
        {
            var consumable = selectedItem.Consumables[i];
            switch (selectedItem.Consumables[i].Type)
            {
                case ConsumableType.Health:
                    playerCondition.AddHealth(consumable.Value);
                    break;
                case ConsumableType.Stamina:
                    playerCondition.AddStamina(consumable.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(consumable.Type), consumable.Type, null);
            }
        }

        for (int i = 0; i < selectedItem.Buffs.Length; i++)
        {
            var buff = selectedItem.Buffs[i];
            var buffController = MainSceneBase.Instance.Player.BuffController;
            buffController.ExecuteBuff(buff);
        }

        if (existSlotDict[selectedItem.ItemID].TakeOneItem() != 0) return;
        RemoveItem();
    }

    private void DropItem()
    {
        if (!selectedItem) return;

        var pos = MainSceneBase.Instance.Player.DropPoint.position;
        Instantiate(selectedItem.ItemPrefab, pos, Quaternion.identity);

        if (existSlotDict[selectedItem.ItemID].TakeOneItem() != 0) return;
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