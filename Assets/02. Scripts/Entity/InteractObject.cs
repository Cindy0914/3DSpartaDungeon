using UnityEngine;

public interface IInteractObject
{
    public string GetInteractionInfo();
    public void Interact();
}

public class FruitsTree : MonoBehaviour, IInteractObject
{
    private string interactionInfo;
    
    
    public string GetInteractionInfo()
    {
        
    }

    public void Interact()
    {
        
    }
}
