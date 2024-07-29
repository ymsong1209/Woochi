using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    
    [HeaderTooltip("GAME STATE", "Game State는 Inspector에서 수정 불가")]
    [SerializeField,ReadOnly] private GameState gameState;

    [Header("Library")]
    [SerializeField] private Library library;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        if(DataCloud.playerData == null)
            LoadData();
    }

    public void SaveData()
    {
        if (!DataCloud.dontSave)
        {
            DataCloud.playerData.hasSaveData = true;
            DataCloud.SavePlayerData();
        }
    }

    public void LoadData()
    {
        DataCloud.LoadPlayerData();
        library.Initialize();
    }

    public void DeleteData()
    {
        DataCloud.DeletePlayerData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ResetGame()
    {
        DataCloud.ResetPlayerData();
        LoadData();
    }

    #region Getter Setter
    public Library Library => library;
    #endregion
}
