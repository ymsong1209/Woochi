using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� Ŭ����
/// </summary>
public class Maintenance : MonoBehaviour
{
    [SerializeField] private Button openMapBtn;     // ���� ����

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

        BattleManager.GetInstance.Allies.BattleEnd();       // �Ʊ� �����̼� ���� ������� �ٽ� ����
        MapManager.GetInstance.CompleteNode();
    }
}
