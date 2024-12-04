using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackManager : SingletonMonobehaviour<BackManager>
{
    private Stack<Popup> popupStack = new Stack<Popup>();

    [SerializeField] private GamePause pause;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    
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
        Clear();
    }
    
    public void Push(Popup popup)
    {
        popupStack.Push(popup);
    }
    
    public void Pop()
    {
        if (popupStack.Count > 0)
        {
            popupStack.Pop();
        }
    }

    private void Back()
    {
        if (DataCloud.IsFocusing) return;
        
        if (popupStack.Count > 0)
        {
            Popup popup = popupStack.Pop();
            popup.Activate(false);
        }
        else
        {
            pause.Pause();
        }
    }
    
    private void Clear()
    {
        popupStack.Clear();
    }
}
