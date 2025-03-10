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
            player = other.GetComponent<Player>();
            animator.SetTrigger(JumpTriggerHash);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }

    public void AnimEvent_Jump()
    {
        if (player == null) return;
        player.PlayerController.Jump(jumpPower);
    }
}