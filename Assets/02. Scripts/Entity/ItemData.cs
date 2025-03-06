using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public string ItemName;
    public string ItemDesc;
    public Sprite ItemIcon;
    public GameObject ItemPrefab;
    
    [Header("Item Type")]
    public Consumable[] Consumables;
    
    [Header("Stacking")]
    public bool IsStackable;
    public int MaxStack;
}

[Serializable]
public class Consumable
{
    public ConsumableType Type;
    public int Value;
}

public enum ConsumableType
{
    Health,
    Stamina,
}
