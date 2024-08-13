using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 정비 클래스
/// </summary>
public class Maintenance : MonoBehaviour
{
    [SerializeField] private Button openMapBtn;     // 지도 열기
    [SerializeField] private GameObject blindObject;    // 섭선, 위치 이동만 클릭 가능하게

    void Start()
    {
        openMapBtn.onClick.AddListener(EndMaintenance);
        gameObject.SetActive(false);
    }

    public void StartMaintenance()
    {
        gameObject.SetActive(true);
        blindObject.SetActive(true);

        BattleManager.GetInstance.InitializeMaintenance();
    }

    public void EndMaintenance()
    {
        gameObject.SetActive(false);
        blindObject.SetActive(false);

        BattleManager.GetInstance.DisableColliderArrow();
        BattleManager.GetInstance.DisableDummy();

        BattleManager.GetInstance.Allies.BattleEnd();       // 아군 포메이션 변경 했을까봐 다시 저장
        MapManager.GetInstance.CompleteNode();
    }
}
