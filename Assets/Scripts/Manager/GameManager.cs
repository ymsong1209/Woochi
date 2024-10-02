using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    
    [HeaderTooltip("GAME STATE", "Game State는 Inspector에서 수정 불가")]
    [SerializeField,ReadOnly] private GameState gameState;

    [Header("Library")]
    [SerializeField] private Library library;

    [Header("Sound")]
    public SoundBGM soundBGM;

    [SerializeField] private bool useDebugCharms = false;
    [SerializeField] private List<int> charmIDs = new List<int>(5);
    [SerializeField] private bool useDebugSkills = false;
    [SerializeField] private int[] skillIDs = new int[5];

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
        ExitGame();
    }

    public void ResetGame()
    {
        DataCloud.ResetPlayerData();
        LoadData();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #region Getter Setter
    public Library Library => library;
    public bool UseDebugCharms => useDebugCharms;
    public List<int> Charms => charmIDs;
    public bool UseDebugSkills => useDebugSkills;
    public int[] Skills => skillIDs;

    #endregion
}
