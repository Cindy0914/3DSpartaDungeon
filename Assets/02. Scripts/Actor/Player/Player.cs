using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerCondition playerCondition;
    [SerializeField] private BuffController buffController;
    [SerializeField] private Interaction interaction;
    [SerializeField] private Transform dropPoint;
    
    public PlayerController PlayerController => playerController;
    public PlayerCondition PlayerCondition => playerCondition;
    public BuffController BuffController => buffController;
    public Interaction Interaction => interaction;
    public Transform DropPoint => dropPoint;
    
    public void Init()
    {
        playerController.Init();
        playerCondition.Init();
        buffController.Init();
        interaction.Init(playerController.MeshTr);
    }
}
