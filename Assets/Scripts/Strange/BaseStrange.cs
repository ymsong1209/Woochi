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

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Activate()
    {
        
    }
    
    public virtual void Deactivate()
    {
        
    }

    public virtual void Initialize()
    {
        
    }

    protected virtual void OnEnable()
    {
        throw new NotImplementedException();
    }
    
    #region Getter Setter

    public StrangeType StrangeType => strangeType;

    #endregion Getter Setter
}
