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
        gameObject.SetActive(true);
    }

    public void EndMaintenance()
    {
        gameObject.SetActive(false);

        BattleManager.GetInstance.Allies.BattleEnd();       // �Ʊ� �����̼� ���� ������� �ٽ� ����
        MapManager.GetInstance.CompleteNode();
    }
}
