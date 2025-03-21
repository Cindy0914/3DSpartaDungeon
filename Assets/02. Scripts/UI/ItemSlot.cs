using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Button selectButton;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI stackText;

    private ItemData itemData;
    private int stack;
    
    public bool IsEmpty() => itemData == null;
    public Button SelectButton => selectButton;

    public void Init()
    {
        iconImage.sprite = null;
        stackText.text = "";
    }

    public void SetItem(ItemData data, int stackValue = 1)
    {
        itemData = data;
        iconImage.sprite = itemData.ItemIcon;
        stack += stackValue;
        
        stackText.text = stack > 1 ? stack.ToString() : "";
    }

    public bool TryAddStack(int stackValue = 1)
    {
        if (!itemData.IsStackable) return false;
        if (itemData.MaxStack < stack + stackValue) return false;
            
        stack += stackValue;
        stackText.text = stack.ToString();
        return true;
    }
    
    public int TakeOneItem()
    {
        stack--;
        if (stack == 0)
        {
            ClearSlot();
            return stack;
        }

        stackText.text = stack.ToString();
        return stack;
    }

    private void ClearSlot()
    {
        itemData = null;
        iconImage.sprite = null;
        stackText.text = "";
        selectButton.onClick.RemoveAllListeners();
    }
}
