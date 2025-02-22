using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingUI : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private SoundBar masterVolume;
    [SerializeField] private SoundBar bgmVolume;
    
    [Header("Mode")]
    [SerializeField] private Toggle fullScreen;
    [SerializeField] private Toggle windowed;
    
    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private List<Resolution> resolutionList;

    private void Awake()
    {
        resolutionDropdown.ClearOptions();
        ClassifyResolution();
        masterVolume.Slider.onValueChanged.AddListener((value) => GameManager.GetInstance.soundManager.SetMasterVolume(value));
        bgmVolume.Slider.onValueChanged.AddListener((value) => GameManager.GetInstance.soundManager.SetBGMVolume(value));
        resolutionDropdown.onValueChanged.AddListener((value)=>GameManager.GetInstance.ApplyResolution(resolutionList[resolutionDropdown.value].width, resolutionList[resolutionDropdown.value].height, fullScreen.isOn));
        fullScreen.onValueChanged.AddListener((value) => GameManager.GetInstance.ApplyResolution(resolutionList[resolutionDropdown.value].width, resolutionList[resolutionDropdown.value].height, value));
    }

    private void OnEnable()
    {
        Show();
    }

    private void Show()
    {
        GameSettingData data = DataCloud.gameSettingData;

        masterVolume.SetValue(data.masterVolume);
        bgmVolume.SetValue(data.bgmVolume);
        
        fullScreen.isOn = data.isFullScreen;
        windowed.isOn = !data.isFullScreen;
        
        int curIndex = resolutionList.FindIndex(res => res.width == data.resolution.x && res.height == data.resolution.y);
        resolutionDropdown.value = curIndex >= 0 ? curIndex : 0;
        resolutionDropdown.RefreshShownValue();
    }

    public void Save()
    {
        GameSettingData data = DataCloud.gameSettingData;
        
        data.masterVolume = masterVolume.GetValue();
        data.bgmVolume = bgmVolume.GetValue();
        
        data.isFullScreen = fullScreen.isOn;
        data.resolution = new Vector2Int(resolutionList[resolutionDropdown.value].width, resolutionList[resolutionDropdown.value].height);

        GameManager.GetInstance.ApplySetting();
        DataCloud.SaveGameSetting();
    }
    
    private void ClassifyResolution()
    {
        Resolution[] resolutions = Screen.resolutions;
        Dictionary<(int, int), Resolution> resolutionDict = new Dictionary<(int, int), Resolution>();

        foreach (Resolution res in resolutions)
        {
            var key = (res.width, res.height);

            // 동일 해상도가 이미 있으면, 가장 높은 refresh rate를 유지
            if (!resolutionDict.ContainsKey(key) || res.refreshRateRatio.value > resolutionDict[key].refreshRateRatio.value)
            {
                resolutionDict[key] = res;
            }
        }

        resolutionList = new List<Resolution>(resolutionDict.Values);
        
        List<string> options = new List<string>();
        foreach (var res in resolutionList)
        {
            options.Add($"{res.width}x{res.height}");
        }
        resolutionDropdown.AddOptions(options);
    }

    public void PlayClickSound()
    {
        GameManager.GetInstance.soundManager.PlaySFX("Reward_Arrow_Click");
    }

}

[System.Serializable]
public class GameSettingData
{
    public float masterVolume = 100.0f;
    public float bgmVolume = 100.0f;

    public bool isFullScreen = true;
    public Vector2Int resolution = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
}
