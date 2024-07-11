using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    
    [HeaderTooltip("GAME STATE", "Game State는 Inspector에서 수정 불가")]
    [SerializeField,ReadOnly] private GameState gameState;

    [SerializeField] private BaseCharm[] charmList = new BaseCharm[5];

    [Header("Library")]
    [SerializeField] private Library charcterLibrary;
    [SerializeField] private Library abnormalLibrary;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        if(DataCloud.playerData == null)
            LoadData();
    }

    void Start()
    {
        SelectRoom();
    }

    public void SelectRoom()
    {
        gameState = GameState.SELECTROOM;
        Debug.Log("GameState : SelectRoom");
    }
    
    public void RemoveCharm(BaseCharm charm)
    {
        for(int i = 0;i<charmList.Length;++i)
        {
            if(charmList[i] == charm)
            {
                charmList[i] = null;
                break;
            }
        }
    }

    public void SaveData()
    {
        DataCloud.SavePlayerData();
    }

    public void LoadData()
    {
        DataCloud.LoadPlayerData();
    }

    public void ResetData()
    {
        DataCloud.ResetPlayerData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    #region Getter Setter
    public BaseCharm[] CharmList => charmList;

    public Library CharcterLibrary => charcterLibrary;
    public Library AbnormalLibrary => abnormalLibrary;
    #endregion
}
