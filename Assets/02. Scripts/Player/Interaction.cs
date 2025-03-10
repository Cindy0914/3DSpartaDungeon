using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float checkRate = 0.05f;
    [SerializeField] private float maxCheckRadius;
    [SerializeField] private Vector3 checkOffset;
    [SerializeField] private LayerMask layerMask;
    
    private GameObject currentInteractObj;
    private Item currentInteractable;
    private Transform meshTr;
    private Collider[] hitColliders;
    private float lastCheckTime;
    
    public Item CurrentInteractable => currentInteractable;

    public void Init(Transform meshTransform)
    {
        meshTr = meshTransform;
        hitColliders = new Collider[1];
    }
    
    private void Update()
    {
        if (!(Time.time - lastCheckTime > checkRate) || !meshTr) return;
        
        lastCheckTime = Time.time;
        var origin = meshTr.position + meshTr.forward * checkOffset.z + meshTr.up * checkOffset.y;
        var result = Physics.OverlapSphereNonAlloc(origin, maxCheckRadius, hitColliders, layerMask);
        
        if (result > 0)
        {
            var hit = hitColliders[0];
            if (hit.gameObject == currentInteractObj) return;
            
            currentInteractObj = hit.gameObject;
            currentInteractable = currentInteractObj.GetComponent<Item>();
            var itemName = currentInteractable.GetItemName();
            var itemDesc = currentInteractable.GetItemDesc();
            UIManager.Instance.SetInteraction(itemName, itemDesc);
        }
        else
        {
            currentInteractObj = null;
            currentInteractable = null;
            UIManager.Instance.InactiveInteraction();
        }
    }
    
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started || currentInteractable == null) return;

        // 오브젝트 파괴 및 인벤토리에 추가
        Destroy(currentInteractObj);
        var itemData = currentInteractable.GetItemData();
        UIManager.Instance.OnItemAdded?.Invoke(itemData);
        
        // 상호작용 종료
        currentInteractObj = null;
        currentInteractable = null;
        UIManager.Instance.InactiveInteraction();
    }
}
