using System;
using UnityEngine;
using UnityEngine.UI;

public class BuffPanel : MonoBehaviour
{
    [SerializeField] private Image speedBuffImage;
    [SerializeField] private Image jumpBuffImage;

    public void Init()
    {
        speedBuffImage.gameObject.SetActive(false);
        jumpBuffImage.gameObject.SetActive(false);
    }
    
    public void UpdateBuffProgress(BuffType type, float value)
    {
        switch (type)
        {
            case BuffType.SpeedUp:
                ActiveBuffImage(speedBuffImage);
                speedBuffImage.fillAmount = value;
                break;
            case BuffType.JumpUp:
                ActiveBuffImage(jumpBuffImage);
                jumpBuffImage.fillAmount = value;
                break;
            case BuffType.None:
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        return;
        
        void ActiveBuffImage(Image image)
        {
            if (!image.gameObject.activeInHierarchy)
                image.gameObject.SetActive(true);
        }
    }
}
