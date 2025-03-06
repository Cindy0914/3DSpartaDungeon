using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerCondition playerCondition;
    [SerializeField] private Interaction interaction;
    
    public PlayerController PlayerController => playerController;
    public PlayerCondition PlayerCondition => playerCondition;
    
    public void Init()
    {
        playerController.Init();
        playerCondition.Init();
        interaction.Init(playerController.MeshTr);
    }

    public void OnGUI()
    {
        // 폰트 사이즈 크게 변경
        GUI.skin.label.fontSize = 30;
        GUI.skin.button.fontSize = 30;
        
        if (GUI.Button(new Rect(10, 10, 200, 50), "Damage"))
        {
            playerCondition.ReduceHealth(10f);
        }
        
        if (GUI.Button(new Rect(10, 70, 200, 50), "Heal"))
        {
            playerCondition.AddHealth(10f);
        }
        
        if (GUI.Button(new Rect(10, 130, 200, 50), "Recover Stamina"))
        {
            playerCondition.AddStamina(10f);
        }
        
        if (GUI.Button(new Rect(10, 190, 200, 50), "Use Stamina"))
        {
            playerCondition.ReduceStamina(10f);
        }
    }
}
