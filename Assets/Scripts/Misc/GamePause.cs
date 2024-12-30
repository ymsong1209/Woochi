using System;
using UnityEngine;
using UnityEngine.UI;
using Event = AK.Wwise.Event;

public class GamePause : MonoBehaviour
{
    [SerializeField] private Button setting;
    [SerializeField] private Button exit;
    [SerializeField] private Button exitYes;

    [Header("Popup")] 
    [SerializeField] private Popup pausePopup;
    [SerializeField] private Popup settingPopup;
    [SerializeField] private Popup exitPopup;
    
    [Header("Sound")]
    [SerializeField] private Event pauseSound;
    
    private void Awake()
    {
        setting?.onClick.AddListener(Setting);
        exit?.onClick.AddListener(Exit);
        exitYes?.onClick.AddListener(() => HelperUtilities.MoveScene(SceneType.Title));
    }

    public void Pause()
    {
        pausePopup.Show();
        Time.timeScale = 0;
        pauseSound.Post(gameObject);
    }
    
    private void OnDisable()
    {
        Time.timeScale = 1;
        pauseSound.Post(gameObject);
    }

    private void Setting()
    {
        settingPopup.Show();
    }
    
    private void Exit()
    {
        exitPopup.Show();
    }
}
