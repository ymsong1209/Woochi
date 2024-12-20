using System;
using AK.Wwise;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private RTPC masterVolumeRTPC;
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        
    }

    private void Start()
    {
        masterVolumeRTPC.SetGlobalValue(0);    
    }

    public void PlaySFX(string sfxName)
    {
        AkSoundEngine.PostEvent(sfxName, gameObject);
    }
}
