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
    [SerializeField] private bool useDebugLogs = false; //로그 기록 여부

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        if(DataCloud.playerData == null)
            LoadData();
    }

    private void Start()
    {
        ApplySetting();
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
    
    public void ApplySetting()
    {
        GameSettingData data = DataCloud.gameSettingData;
        float masterVolume = data.masterVolume;
        float bgmVolume = data.bgmVolume;
        soundManager.SetVolume(masterVolume, bgmVolume);
        Screen.fullScreen = data.isFullScreen;
        Screen.SetResolution(data.resolution.x, data.resolution.y, data.isFullScreen);
    }

    #region Getter Setter
    public Library Library => library;
    public bool UseDebugCharms => useDebugCharms;
    public List<int> Charms => charmIDs;
    public bool UseDebugSkills => useDebugSkills;
    public int[] Skills => skillIDs;
    public bool UseDebugLogs => useDebugLogs;

    #endregion
}
