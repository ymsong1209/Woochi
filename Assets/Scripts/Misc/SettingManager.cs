using System.Collections;
using UnityEngine;

public class SettingManager : SingletonMonobehaviour<SettingManager>
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        ApplySetting();
    }

    public void ApplySetting()
    {
        GameSettingData data = DataCloud.gameSettingData;
        StartCoroutine(SetResolution(data));
    }
    
    private IEnumerator SetResolution(GameSettingData data)
    {
        Screen.fullScreen = data.isFullScreen;
        Screen.SetResolution(data.resolution.x, data.resolution.y, data.isFullScreen);
        
        yield return null;
        
        float targetAspect = 16f / 9f;
        float windowAspect = (float)Screen.width / Screen.height;
        mainCamera.orthographicSize = 8 * (targetAspect / windowAspect);
    }
}
