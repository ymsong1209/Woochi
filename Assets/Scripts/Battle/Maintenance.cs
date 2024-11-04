using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 정비 클래스
/// </summary>
public class Maintenance : MonoBehaviour
{
    [SerializeField] private Button openMapBtn;     // 지도 열기
    [SerializeField] private GameObject blindObject;    // 섭선, 위치 이동만 클릭 가능하게
    [SerializeField] private Button skillScrollBtn;
    [SerializeField] private Button skillScrollBlind;
    [SerializeField] private GameObject skillScroll; //도술두루마리

    void Start()
    {
        openMapBtn.onClick.AddListener(EndMaintenance);
        
        skillScrollBtn.onClick.AddListener(()=>skillScrollBlind.gameObject.SetActive(true));
        skillScrollBtn.onClick.AddListener(() => skillScroll.SetActive(true));
        
        skillScrollBlind.onClick.AddListener(()=>skillScrollBlind.gameObject.SetActive(false));
        skillScrollBlind.onClick.AddListener(()=>skillScroll.gameObject.SetActive(false));
        skillScroll.SetActive(false);
        skillScrollBlind.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void StartMaintenance()
    {
        gameObject.SetActive(true);
        blindObject.SetActive(true);
        skillScrollBlind.gameObject.SetActive(false);
        skillScroll.SetActive(false);
        
        BattleManager.GetInstance.InitializeMaintenance();
    }

    public void EndMaintenance()
    {
        gameObject.SetActive(false);
        blindObject.SetActive(false);
        skillScrollBlind.gameObject.SetActive(false);
        skillScroll.SetActive(false);

        BattleManager.GetInstance.DisableColliderArrow();
        BattleManager.GetInstance.DisableDummy();

        BattleManager.GetInstance.Allies.SaveFormation();       // 아군 포메이션 변경 했을까봐 다시 저장
        GameManager.GetInstance.SaveData();

        MapManager.GetInstance.CompleteNode();
        GameManager.GetInstance.soundBGM.ToggleBattleMap(false);
    }
}
