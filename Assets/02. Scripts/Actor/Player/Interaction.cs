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
    private IInteractable currentInteractable;
    private Transform meshTr;
    private Collider[] hitColliders;
    private float lastCheckTime;
    
    public void Init(Transform meshTransform)
    {
        meshTr = meshTransform;
        hitColliders = new Collider[1];
    }
    
    private void Update()
    {
        if (!(Time.time - lastCheckTime > checkRate) || !meshTr) return;
        
        lastCheckTime = Time.time;
        // 플레이어 앞의 임의의 위치에 SphereCast를 통해 상호작용 가능한 오브젝트를 체크
        var origin = meshTr.position + meshTr.forward * checkOffset.z + meshTr.up * checkOffset.y;
        var result = Physics.OverlapSphereNonAlloc(origin, maxCheckRadius, hitColliders, layerMask);
        
        // 상호작용 가능한 오브젝트가 있을 경우
        if (result > 0)
        {
            var hit = hitColliders[0];
            if (hit.gameObject == currentInteractObj) return;
            
            currentInteractObj = hit.gameObject;
            currentInteractable = currentInteractObj.GetComponent<IInteractable>();
            var itemName = currentInteractable.GetItemName();
            var itemDesc = currentInteractable.GetItemDesc();
            
            // 상호작용 가능한 오브젝트의 UI 타입에 따라 UI 활성화
            if (currentInteractable.GetUIType() == InteractionUIType.Screen)
                UIManager.Instance.ActiveInteractScreenUI(itemName, itemDesc);
            else
            {
                var uiPos = currentInteractable.GetPosition();
                UIManager.Instance.ActiveInteractWorldUI(itemDesc, uiPos);
            }
        }
        // 상호작용 가능한 오브젝트가 없을 경우
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

        // 상호작용 가능한 오브젝트의 OnInteract 함수 호출
        currentInteractable.OnInteract();
        currentInteractObj = null;
        currentInteractable = null;
        UIManager.Instance.InactiveInteraction();
    }
}
