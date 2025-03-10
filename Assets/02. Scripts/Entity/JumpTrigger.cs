using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float jumpPower;

    private readonly int JumpTriggerHash = Animator.StringToHash("Jump");
    private Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("JumpTrigger OnTriggerEnter");
            player = other.GetComponent<Player>();
            animator.SetTrigger(JumpTriggerHash);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("JumpTrigger OnTriggerExit");
            player = null;
        }
    }

    public void AnimEvent_Jump()
    {
        Debug.Log("JumpTrigger AnimEvent_Jump");
        if (!player) return;
        Debug.Log("JumpTrigger AnimEvent_Jump/Player");
        player.PlayerController.Jump(jumpPower);
    }
}