using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionPanel : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private float maxValue;
    
    public void Init(float maxValue)
    {
        this.maxValue = maxValue;
    }
    
    public void UpdateValue(float value)
    {
        fillImage.fillAmount = value / maxValue;
    }
}
