using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    [HeaderTooltip("GAME STATE", "Game State는 Inspector에서 수정 불가")]
    [SerializeField,ReadOnly] private GameState gameState;

    [Header("Library")]
    [SerializeField] private Library library;

    [Header("Sound")]
    public SoundManager soundManager;

    [Header("Debug")]
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
        DataCloud.playerData.hasSaveData = true;
        DataCloud.SavePlayerData();
    }

    public void LoadData()
    {
        DataCloud.LoadPlayerData();
        DataCloud.LoadGameSetting();
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
    
    public void GoTitle()
    {
        HelperUtilities.MoveScene(SceneType.Title);
    }
    
    #region Getter Setter
    public Library Library => library;
    public bool UseDebugCharms => useDebugCharms;
    public List<int> Charms => charmIDs;
    public bool UseDebugSkills => useDebugSkills;
    public int[] Skills => skillIDs;

    #endregion
}
