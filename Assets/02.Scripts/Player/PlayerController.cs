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
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    private readonly Ray[] groundRays = new Ray[4];
    private Vector3 moveInput;

    [Header("Rotation")]
    [SerializeField] private Transform mesh;

    [SerializeField] private Transform camContainer;
    [SerializeField] private float rotSmoothTime;
    [SerializeField] private float camRotSpeed;
    private Camera mainCam;
    private Vector2 lookInput;
    private float rotSmoothVelocity;

    private bool canMove = false;
    public Transform CamContainer => camContainer;

    public void Init()
    {
        RaycastGround();
        // Cursor.lockState = CursorLockMode.Locked;
        mainCam = Camera.main;
        canMove = true;
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        if (moveInput != Vector3.zero)
            Move();
    }

    private void LateUpdate()
    {
        if (!canMove) return;
        CameraLook();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("InputActionPhase.Started");
            stateMachine.StateChange(State.Walk);
        }
        else if (context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector3>();
            Debug.Log($"Performed: {moveInput}");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("InputActionPhase.Canceled");
            moveInput = Vector3.zero;
            stateMachine.StateChange(State.Idle);
        }
    }

    private void Move()
    {
        // Mathf.Atan2(moveInput.x, moveInput.z) Input값을 기준으로 방향을 나타내는 각도를 반환
        // * Mathf.Rad2Deg 라디안 값을 각도로 변환
        // + mainCamTr.eulerAngles.y 카메라의 y축 각도를 더해 카메라 기준으로 회전하도록 조정
        float targetAngle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
        // 자연스러운 캐릭터의 회전을 위한 것으로 SmoothDampAngle을 통해 절차적 회전 각도를 구함 
        float angle = Mathf.SmoothDampAngle(mesh.eulerAngles.y, targetAngle, ref rotSmoothVelocity, rotSmoothTime);

        mesh.rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        float currentY = rigid.velocity.y;
        rigid.velocity = moveDirection.normalized * moveSpeed;
        rigid.velocity = new Vector3(rigid.velocity.x, currentY, rigid.velocity.z);
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
            rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            stateMachine.StateChange(State.Jump);
        }
    }

    private void RaycastGround()
    {
        groundRays[0] = new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down);
        groundRays[1] = new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down);
        groundRays[2] = new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down);
        groundRays[3] = new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down);
    }

    private bool IsGround()
    {
        for (int i = 0; i < groundRays.Length; i++)
        {
            if (Physics.Raycast(groundRays[i], 0.1f, groundLayer))
                return true;
        }

        return false;
    }
}