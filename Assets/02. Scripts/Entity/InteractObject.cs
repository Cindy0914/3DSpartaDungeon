using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitsTree : MonoBehaviour, IInteractable
{
    [Header("Interact Info")]
    [SerializeField] private Vector3 uiOffset;
    [SerializeField] private string treeName;
    [SerializeField] private string treeDesc;

    [Header("Tree Info")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody[] fruits;
    private readonly string shakeTriggerHash = "Shake";

    public InteractionUIType GetUIType()
    {
        return InteractionUIType.World;
    }

    public string GetItemName()
    {
        return treeName;
    }
    public string GetItemDesc()
    {
        return treeDesc;
    }

    public Vector3 GetPosition()
    {
        return transform.position + uiOffset;
    }

    public void OnInteract()
    {
        animator.SetTrigger(shakeTriggerHash);

        for (int i = 0; i < fruits.Length; i++)
        {
            fruits[i].isKinematic = false;
        }
        
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
