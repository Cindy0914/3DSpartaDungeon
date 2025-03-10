using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class StateMachine<T> : MonoBehaviour where T : MonoBehaviour
{
    public State state = State.None;

    [SerializeField] protected T actor;
    [SerializeField] protected Animator anim;

    protected virtual void Update()
    {
        StateUpdate();
    }

    #region MACHINE

    public void StateChange(State changeState)
    {
        switch (state)
        {
            case State.None:
                break;
            case State.Idle:
                Idle_Exit();
                break;
            case State.Walk:
                Walk_Exit();
                break;
            case State.Jump:
                Jump_Exit();
                break;
        }

        state = changeState;

        switch (state)
        {
            case State.None:
                break;
            case State.Idle:
                Idle_Enter();
                break;
            case State.Walk:
                Walk_Enter();
                break;
            case State.Jump:
                Jump_Enter();
                break;
        }
    }

    private void StateUpdate()
    {
        switch (state)
        {
            case State.None:
                break;
            case State.Idle:
                Idle_Update();
                break;
            case State.Walk:
                Walk_Update();
                break;
            case State.Jump:
                Jump_Update();
                break;
        }
    }

    #endregion

    #region STATE

    protected virtual void Idle_Enter() { }

    protected virtual void Idle_Update() { }

    protected virtual void Idle_Exit() { }

    protected virtual void Walk_Enter() { }

    protected virtual void Walk_Update() { }

    protected virtual void Walk_Exit() { }

    protected virtual void Jump_Enter() { }

    protected virtual void Jump_Update() { }

    protected virtual void Jump_Exit() { }

    protected virtual void Attack_Enter() { }

    protected virtual void Attack_Update() { }

    protected virtual void Attack_Exit() { }

    protected virtual void Hit_Enter() { }

    protected virtual void Hit_Update() { }

    protected virtual void Hit_Exit() { }

    protected virtual void Die_Enter() { }

    protected virtual void Die_Update() { }

    protected virtual void Die_Exit() { }

    #endregion
}

public enum State
{
    None,
    Idle,
    Walk,
    Jump,
}