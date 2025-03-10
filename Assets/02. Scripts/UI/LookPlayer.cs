using UnityEngine;

public class LookPlayer : MonoBehaviour
{
    private Transform playerTr;
    private Transform tr;

    private void Start()
    {
        playerTr = MainSceneBase.Instance.Player.transform;
        tr = transform;
    }

    private void Update()
    {
        Vector3 targetPos = new Vector3(playerTr.position.x, tr.position.y, playerTr.position.z);
        tr.LookAt(targetPos);
        
        tr.Rotate(0, 180f, 0);
    }
}
