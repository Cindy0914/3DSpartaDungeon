using UnityEngine;

public class PlayerStateMachine : StateMachine<Player>
{
    [SerializeField] private PlayerController playerController;
    private readonly int moveAnimHash = Animator.StringToHash("IsMove");
    private readonly int jumpAnimHash = Animator.StringToHash("IsJump");
    private const float groundCheckDelay = 0.1f;
    private float jumpTime;

    private void Start()
    {
        state = State.Idle;
        StateChange(state);
    }

    protected override void Idle_Enter()
    {
        anim.SetBool(moveAnimHash, false);
    }

    protected override void Idle_Update()
    {
        if (playerController.MoveInput != Vector3.zero)
        {
            StateChange(State.Walk);
        }
    }

    protected override void Idle_Exit() { }

    protected override void Walk_Enter()
    {
        anim.SetBool(moveAnimHash, true);
    }

    protected override void Walk_Update()
    {
        if (playerController.MoveInput == Vector3.zero)
        {
            StateChange(State.Idle);
        }
    }

    protected override void Walk_Exit() { }

    protected override void Jump_Enter()
    {
        anim.SetBool(jumpAnimHash, true);
        jumpTime = Time.time;
    }

    protected override void Jump_Update()
    {
        if (Time.time - jumpTime < groundCheckDelay) return;
        
        if (playerController.IsGround())
        {
            if (playerController.MoveInput != Vector3.zero)
            {
                StateChange(State.Walk);
            }
            else
            {
                StateChange(State.Idle);
            }
        }
    }

    protected override void Jump_Exit()
    {
        anim.SetBool(jumpAnimHash, false);
    }
}