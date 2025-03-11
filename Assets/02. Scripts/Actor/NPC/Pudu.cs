using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Pudu : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;

    [Header("State")]
    [SerializeField] private NPCState currentState;

    [Header("Agent Setting")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minActionDelay;
    [SerializeField] private float maxActionDelay;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    private readonly int moveAnimHash = Animator.StringToHash("IsMove");
    private readonly int eatAnimHash = Animator.StringToHash("IsEat");
    private int loadArea;

    private void Start()
    {
        navMeshAgent.speed = moveSpeed;
        StartCoroutine(DoRandomAction());
    }

    private void Update()
    {
        if (currentState != NPCState.Move) return;
        
        if (navMeshAgent.remainingDistance <= 0.1f)
            ChangeState(NPCState.Idle);
        if (!navMeshAgent.hasPath)
        {
            navMeshAgent.SetDestination(GetRandomPosition());
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator DoRandomAction()
    {
        while (true)
        {
            ChangeState(GetNewState());
            yield return new WaitForSeconds(Random.Range(minActionDelay, maxActionDelay));
        }
    }

    private NPCState GetNewState()
    {
        var newState = 0;
        
        do 
        {
            newState = Random.Range(0, 3);
        }
        while (newState == (int)NPCState.Eat && IsLoadArea());

        return (NPCState)newState;
    }

    private void ChangeState(NPCState newState)
    {
        Debug.Log("Change State: " + newState);
        currentState = newState;

        switch (currentState)
        {
            case NPCState.Idle:
                Idle();
                break;
            case NPCState.Move:
                Move();
                break;
            case NPCState.Eat:
                Eat();
                break;
        }
    }

    private void Idle()
    {
        animator.SetBool(moveAnimHash, false);
        animator.SetBool(eatAnimHash, false);
        navMeshAgent.isStopped = true;
    }

    private void Move()
    {
        animator.SetBool(moveAnimHash, true);
        animator.SetBool(eatAnimHash, false);
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(GetRandomPosition());
    }

    private void Eat()
    {
        animator.SetBool(moveAnimHash, false);
        animator.SetBool(eatAnimHash, true);
        navMeshAgent.isStopped = true;
    }

    private bool IsLoadArea()
    {
        NavMesh.SamplePosition(transform.position, out var hit, 1.0f, NavMesh.AllAreas);
        return hit.mask == loadArea;
    }

    private Vector3 GetRandomPosition()
    {
        var searchDistance = Random.Range(minDistance, maxDistance);
        var randomPos = Random.insideUnitSphere * searchDistance;
        NavMesh.SamplePosition(transform.position + randomPos, out var hit, maxDistance, NavMesh.AllAreas);
        return hit.position;
    }
}

public enum NPCState
{
    Idle,
    Move,
    Eat,
}