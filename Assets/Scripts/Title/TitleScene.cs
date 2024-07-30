using UnityEngine;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private Button beginBtn;
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button deleteBtn;

    void Start()
    {
        #region Event
        beginBtn.onClick.AddListener(Begin);
        continueBtn.onClick.AddListener(Continue);
        exitBtn.onClick.AddListener(() => GameManager.GetInstance.ExitGame());
        deleteBtn.onClick.AddListener(() => GameManager.GetInstance.DeleteData());
        #endregion

        continueBtn.interactable = DataCloud.playerData.hasSaveData;
    }

    public void Departure(bool isBegin)
    {
        // 진행중이던 게임 정보 삭제
        if (isBegin)
        {
            GameManager.GetInstance.ResetGame();
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

}
