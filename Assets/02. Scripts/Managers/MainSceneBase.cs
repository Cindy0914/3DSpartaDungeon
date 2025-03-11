using UnityEngine;

public class MainSceneBase : Singleton<MainSceneBase>
{
    [SerializeField] private Vector3 playerSpawnPoint;
    
    private Transform mainCamTr;
    private const string playerPrefabKey = "Player";
    public Player Player { get; private set; }

    public override void Awake()
    {
        base.Awake();
        SpawnPlayer();
        SetMainCamera();
    }

    private void Start()
    {
        UIManager.Instance.Init(Player);
    }

    private void SpawnPlayer()
    {
        var playerPrefab = Resources.Load<GameObject>(playerPrefabKey);
        GameObject playerObj = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
        Player = playerObj.GetComponent<Player>();
        Player.Init();
    }

    private void SetMainCamera()
    {
        var camContainer = Player.PlayerController.CamContainer;
        mainCamTr = Camera.main.transform;
        mainCamTr.SetParent(camContainer);
        mainCamTr.localPosition = Vector3.zero;
        mainCamTr.localRotation = Quaternion.identity;
    }
}
