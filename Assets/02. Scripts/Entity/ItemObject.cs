using UnityEngine;

public interface IInteractable
{
    public InteractionUIType GetUIType(); 
    public string GetItemName();
    public string GetItemDesc();
    public Vector3 GetPosition();
    public void OnInteract();
}

public enum InteractionUIType
{
    Screen,
    World
}

public class ItemObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;
    
    public InteractionUIType GetUIType()
    {
        return InteractionUIType.Screen;
    }

    public string GetItemName()
    {
        return itemData.ItemName;
    }
    
    public string GetItemDesc()
    {
        return itemData.ItemDesc;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void OnInteract()
    {
        UIManager.Instance.OnItemAdded?.Invoke(itemData);
        Destroy(gameObject);
    }
}
