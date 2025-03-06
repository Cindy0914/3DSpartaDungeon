using TMPro;
using UnityEngine;

public class InteractionPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descText;
    
    public void SetInteraction(string itemName, string desc)
    {
        nameText.text = itemName;
        descText.text = desc;
    }
}
