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
        GameManager.GetInstance.soundManager.PlaySFX("Main_Title");
    }

    public void Departure(bool isBegin)
    {
        if (isBegin)
        {
            GameManager.GetInstance.ResetGame();
        }

        HelperUtilities.MoveScene(SceneType.Battle);
    }

    /// <summary>
    /// ó������ ������ ��
    /// </summary>
    private void Begin() => Departure(true);

    /// <summary>
    /// ����ϱ� ������ ��
    /// </summary>
    private void Continue() => Departure(false);

}
