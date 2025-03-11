using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 버프를 관리하는 클래스
public class BuffController : MonoBehaviour
{
    private Player player;
    private Dictionary<int, Coroutine> buffCoroutines; // 현재 적용중인 버프 코루틴
    private Dictionary<int, Action> finishActions;     // 버프 종료시 실행할 액션

    public void Init()
    {
        player = GetComponent<Player>();
        buffCoroutines = new Dictionary<int, Coroutine>();
        finishActions = new Dictionary<int, Action>();
    }

    // 버프 적용 실행
    public void ExecuteBuff(BuffData data)
    {
        var type = (int)data.Type;
        // 이미 적용중인 같은 타입의 버프가 있다면 종료 액션 실행 후 코루틴 중지
        if (buffCoroutines.TryGetValue(type, out var coroutine))
        {
            StopCoroutine(coroutine);
            finishActions[type]?.Invoke();
            buffCoroutines.Remove(type);
            finishActions.Remove(type);
        }

        // 새로운 버프 적용 코루틴 실행
        buffCoroutines.Add(type, StartCoroutine(ApplyBuff(data)));
        finishActions.Add(type, () => EndBuff(data));
    }
    
    // 버프 효과 적용 코루틴
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