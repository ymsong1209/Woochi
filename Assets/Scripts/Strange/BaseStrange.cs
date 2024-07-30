using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStrange : MonoBehaviour
{
    [SerializeField] private StrangeType strangeType;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public virtual void Activate(MapNode node)
    {
        gameObject.SetActive(true);
    }
    
    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
        StrangeManager.GetInstance.StrangeBackground.SetActive(false);
        MapManager.GetInstance.CompleteNode();
    }

    public virtual void Initialize()
    {
        gameObject.SetActive(false);
    }
    
    
    #region Getter Setter

    public StrangeType StrangeType => strangeType;

    #endregion Getter Setter
}
