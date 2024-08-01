using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 정비 클래스
/// </summary>
public class Maintenance : MonoBehaviour
{
    [SerializeField] private Button openMapBtn;     // 지도 열기

    void Start()
    {
        openMapBtn.onClick.AddListener(EndMaintenance);
        gameObject.SetActive(false);
    }

    public void StartMaintenance()
    {
        DataCloud.isMaintenance = true;
        gameObject.SetActive(true);

        BattleManager.GetInstance.InitializeMaintenance();
    }

    public void EndMaintenance()
    {
        DataCloud.isMaintenance = false;
        gameObject.SetActive(false);

        BattleManager.GetInstance.Allies.BattleEnd();       // 아군 포메이션 변경 했을까봐 다시 저장
        MapManager.GetInstance.CompleteNode();
    }
}
