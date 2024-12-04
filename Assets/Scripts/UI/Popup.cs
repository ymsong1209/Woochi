using System;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject back;

    public void Show()
    {
        Activate(true);
        BackManager.GetInstance.Push(this);
    }

    public void Close()
    {
        Activate(false);
        BackManager.GetInstance.Pop();
    }

    public void Activate(bool active)
    {
        popup.SetActive(active);
        back.SetActive(active);
    }
}
