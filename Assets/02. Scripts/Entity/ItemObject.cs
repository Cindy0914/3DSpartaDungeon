using UnityEngine;

public interface IInteractable
{
    public ItemData GetItemData();
    public string GetItemName();
    public string GetItemDesc();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;

    public ItemData GetItemData()
    {
        return itemData;
    }
    
    public string GetItemName()
    {
        return itemData.ItemName;
    }
    
    public string GetItemDesc()
    {
        return itemData.ItemDesc;
    }
}
