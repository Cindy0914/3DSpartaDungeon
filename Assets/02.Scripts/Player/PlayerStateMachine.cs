using UnityEngine;

public class PlayerStateMachine : StateMachine<Player>
{
    private readonly int moveAnimHash = Animator.StringToHash("IsMove");
    private readonly int jumpAnimHash = Animator.StringToHash("Jump");

    private void Start()
    {
        state = State.Idle;
        StateChange(state);
    }

    protected override void Idle_Enter()
    {
    }

    protected override void Idle_Update()
    {
    }

    protected override void Idle_Exit()
    {
    }

    protected override void Walk_Enter()
    {
        anim.SetBool(moveAnimHash, true);
    }

    protected override void Walk_Update()
    {
    }

    protected override void Walk_Exit()
    {
        anim.SetBool(moveAnimHash, false);
    }

    protected override void Jump_Enter()
    {
        anim.SetTrigger(jumpAnimHash);
    }

    protected override void Jump_Update()
    {
    }

    protected override void Jump_Exit()
    {
    }
}
