using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    private Player player;
    private Dictionary<int, Coroutine> buffCoroutines;
    private Dictionary<int, Action> finishActions;

    public void Init()
    {
        player = GetComponent<Player>();
        buffCoroutines = new Dictionary<int, Coroutine>();
        finishActions = new Dictionary<int, Action>();
    }

    public void ExecuteBuff(BuffData data)
    {
        var type = (int)data.Type;
        if (buffCoroutines.TryGetValue(type, out var coroutine))
        {
            StopCoroutine(coroutine);
            finishActions[type]?.Invoke();
            buffCoroutines.Remove(type);
            finishActions.Remove(type);
        }

        buffCoroutines.Add(type, StartCoroutine(ApplyBuff(data)));
        finishActions.Add(type, () => EndBuff(data));
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
        
        var type = (int)data.Type;
        finishActions[type]?.Invoke();
        finishActions.Remove(type);
        buffCoroutines.Remove(type);
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