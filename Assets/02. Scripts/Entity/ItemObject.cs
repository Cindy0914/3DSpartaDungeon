using UnityEngine;

public interface IInteractable
{
    public void OnInteract();
    public string GetItemName();
    public string GetItemDesc();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;

    public void OnInteract()
    {
        // 인벤토리에 추가
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
