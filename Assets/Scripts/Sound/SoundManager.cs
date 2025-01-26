using System;
using AK.Wwise;
using UnityEngine;
using Event = AK.Wwise.Event;

public enum BGMState
{
    None,
    Title,
    Map,
    Battle,
    Reward,
    End
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private RTPC masterVolumeRTPC;
    [SerializeField] private RTPC bgmVolumeRTPC;
    [SerializeField] private Event[] bgmEvents;

    public void PlayBGM(BGMState bgmState)
    {
        bgmEvents[(int) bgmState].Post(gameObject);
    }
    
    public void PlaySFX(string sfxName)
    {
        AkSoundEngine.PostEvent(sfxName, gameObject);
    }

    public void StopAllSound()
    {
        AkSoundEngine.StopAll();
    }
    public void SetMasterVolume(float masterVolume)
    {
        masterVolumeRTPC.SetGlobalValue(masterVolume);
    }
    
    public void SetBGMVolume(float bgmVolume)
    {
        bgmVolumeRTPC.SetGlobalValue(bgmVolume);
    }
}
