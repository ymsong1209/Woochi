using UnityEngine;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private Button beginBtn;
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button deleteBtn;
    [SerializeField] private Button testBtn;

    [SerializeField] private GameObject testPanel;

    void Start()
    {
        #region Event
        beginBtn.onClick.AddListener(Begin);
        continueBtn.onClick.AddListener(Continue);
        deleteBtn.onClick.AddListener(() => GameManager.GetInstance.ResetData());
        testBtn.onClick.AddListener(Test);
        #endregion

        continueBtn.interactable = DataCloud.playerData.isProgressing;
    }

    public void Departure(bool isBegin)
    {
        // 진행중이던 게임 정보 삭제
        if (isBegin)
        {
            DataCloud.ResetPlayerData();
        }

        HelperUtilities.MoveScene(SceneType.Battle);
    }

    /// <summary>
    /// 처음부터 눌렀을 시
    /// </summary>
    private void Begin() => Departure(true);

    /// <summary>
    /// 계속하기 눌렀을 시
    /// </summary>
    private void Continue() => Departure(false);

    private void Test()
    {
        testPanel.SetActive(true);
    }
}
