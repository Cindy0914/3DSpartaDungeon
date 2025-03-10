using TMPro;
using UnityEngine;

public class InteractionPanel : MonoBehaviour
{
    [Header("Screen UI")]
    [SerializeField] private GameObject screenUI;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    
    [Header("World UI")]
    [SerializeField] private RectTransform worldUI;
    [SerializeField] private TextMeshProUGUI interactionText;

    public void HideInteractionUi()
    {
        screenUI.SetActive(false);
        worldUI.gameObject.SetActive(false);
    }
    
    public void SetScreenUI(string itemName, string desc)
    {
        screenUI.SetActive(true);
        nameText.text = itemName;
        descText.text = desc;
    }

    public void SetWorldUI(string desc, Vector3 target)
    {
        worldUI.gameObject.SetActive(true);
        interactionText.text = desc;
        worldUI.position = target;
    }
}
