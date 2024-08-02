using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� Ŭ����
/// </summary>
public class Maintenance : MonoBehaviour
{
    [SerializeField] private Button openMapBtn;     // ���� ����
    [SerializeField] private GameObject blindObject;    // ����, ��ġ �̵��� Ŭ�� �����ϰ�

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
        BattleManager.GetInstance.Allies.BattleEnd();       // �Ʊ� �����̼� ���� ������� �ٽ� ����
        MapManager.GetInstance.CompleteNode();
    }
}
