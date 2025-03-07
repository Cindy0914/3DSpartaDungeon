using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    private Player player;
    private Dictionary<BuffType, Coroutine> buffCoroutines;
    private Dictionary<BuffType, Action> finishActions;

    public void Init()
    {
        player = GetComponent<Player>();
        buffCoroutines = new Dictionary<BuffType, Coroutine>();
        finishActions = new Dictionary<BuffType, Action>();
    }

    public void ExecuteBuff(BuffData data)
    {
        if (buffCoroutines.TryGetValue(data.Type, out var coroutine))
        {
            StopCoroutine(coroutine);
            finishActions[data.Type]?.Invoke();
            buffCoroutines.Remove(data.Type);
            finishActions.Remove(data.Type);
        }

        buffCoroutines.Add(data.Type, StartCoroutine(ApplyBuff(data)));
        finishActions.Add(data.Type, () => EndBuff(data));
    }
    
    private IEnumerator ApplyBuff(BuffData data)
    {
        BuffEffect(data);
        
        var elapsedTime = data.Duration;
        while (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            UIManager.Instance.OnBuffProgressChanged?.Invoke(data.Type, elapsedTime / data.Duration);
            yield return null;
        }
        
        finishActions[data.Type]?.Invoke();
        finishActions.Remove(data.Type);
        buffCoroutines.Remove(data.Type);
    }

    private void BuffEffect(BuffData data)
    {
        switch (data.Type)
        {
            case BuffType.SpeedUp:
                player.PlayerController.SetMoveSpeed(data.Value);
                break;
            case BuffType.JumpUp:
                player.PlayerController.SetJumpPower(data.Value);
                break;
            case BuffType.None:
            default:
                throw new ArgumentOutOfRangeException(nameof(data), data.Type, null);
        }
    }

    private void EndBuff(BuffData data)
    {
        switch (data.Type)
        {
            case BuffType.SpeedUp:
                player.PlayerController.SetMoveSpeed(-data.Value);
                break;
            case BuffType.JumpUp:
                player.PlayerController.SetJumpPower(-data.Value);
                break;
            case BuffType.None:
            default:
                throw new ArgumentOutOfRangeException(nameof(data), data.Type, null);
        }
    }
}