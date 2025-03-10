using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private PlayerStateMachine stateMachine;

    [Header("Movement")]
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float addRunSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    private Vector3 moveInput;
    private Vector3 currentVelocity;
    private Vector3 prevVelocity;

    [Header("Rotation")]
    [SerializeField] private Transform meshTr;
    [SerializeField] private Transform camContainer;
    [SerializeField] private float rotSmoothTime;
    [SerializeField] private float camRotSpeed;
    private Camera mainCam;
    private Vector2 lookInput;
    private float rotSmoothVelocity;
    
    private bool canMove = false;
    private bool isRunning = false;

    public Transform CamContainer => camContainer;
    public Transform MeshTr => meshTr;
    public Vector3 MoveInput => moveInput;

    public void Init()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainCam = Camera.main;
        canMove = true;
        
        var playerCondition = MainSceneBase.Instance.Player.PlayerCondition;
        playerCondition.OnStaminaEmpty += OnCancelRun;
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        
        if (moveInput != Vector3.zero)
            Move();
        else
            rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
    }

    private void LateUpdate()
    {
        if (!canMove) return;
        CameraLook();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector3>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector3.zero;
        }
    }

    private void Move()
    {
        // Mathf.Atan2(moveInput.x, moveInput.z) Input값을 기준으로 방향을 나타내는 각도를 반환
        // * Mathf.Rad2Deg 라디안 값을 각도로 변환
        // + mainCamTr.eulerAngles.y 카메라의 y축 각도를 더해 카메라 기준으로 회전하도록 조정
        float targetAngle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
        // 자연스러운 캐릭터의 회전을 위한 것으로 SmoothDampAngle을 통해 절차적 회전 각도를 구함 
        float angle = Mathf.SmoothDampAngle(meshTr.eulerAngles.y, targetAngle, ref rotSmoothVelocity, rotSmoothTime);

        meshTr.rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        float currentY = rigid.velocity.y;
        rigid.velocity = moveDirection.normalized * moveSpeed;
        rigid.velocity = new Vector3(rigid.velocity.x, currentY, rigid.velocity.z);
    }

    public void SetMoveSpeed(float value)
    {
        moveSpeed += value;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void CameraLook()
    {
        mainCam.transform.RotateAround(transform.position, Vector3.up, lookInput.x * camRotSpeed);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround())
        {
            Jump(jumpPower);
        }
    }
    
    public void Jump(float power)
    {
        rigid.AddForce(Vector2.up * power, ForceMode.Impulse);
        stateMachine.StateChange(State.Jump);
    }

    public void SetJumpPower(float value)
    {
        jumpPower += value;
    }
    
    public void OnRun(InputAction.CallbackContext context)
    {
        var playerCondition = MainSceneBase.Instance.Player.PlayerCondition;
        if (context.phase == InputActionPhase.Started)
        {
            isRunning = true;
            playerCondition.SetRunning(true);
            SetMoveSpeed(addRunSpeed);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            if (!isRunning) return;
            playerCondition.SetRunning(false);
            SetMoveSpeed(-addRunSpeed);
        }
    }
    
    private void OnCancelRun()
    {
        isRunning = false;
        SetMoveSpeed(-addRunSpeed);
    }

    public bool IsGround()
    {
        Ray[] rays = 
        {
            new(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };
        
        for (int i = 0; i < rays.Length; i++)
        {
            if (!Physics.Raycast(rays[i], 0.05f, groundLayer))
            {
                return false;
            }
        }

        return true;
    }
    
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            UIManager.Instance.OnInputInventory();
            Cursor.lockState = UIManager.Instance.IsInventoryActive ? CursorLockMode.None : CursorLockMode.Locked;
            Time.timeScale = UIManager.Instance.IsInventoryActive ? 0f : 1f;
            canMove = !UIManager.Instance.IsInventoryActive;
        }
    }
}