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
        Debug.Log("OnTriggerEnter");
        
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            animator.SetTrigger(JumpTriggerHash);
        }
    }
    
    public void AnimEvent_Jump()
    {
        player.PlayerController.Jump(jumpPower);
        player = null;
    }
}