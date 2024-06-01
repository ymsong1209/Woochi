using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoochiActionButton : MonoBehaviour
{
    [SerializeField] private WoochiButtonList owner;
    [SerializeField] private GameObject activeImage;
    // Start is called before the first frame update
    void Start()
    {
        activeImage.SetActive(false);
    }

    public virtual void Initialize()
    {
        activeImage.SetActive(false);
    }
    
    public virtual void Activate()
    {
       
    }

    public virtual void Deactivate()
    {
        activeImage.SetActive(false);
    }

    public void Highlight()
    {
        activeImage.SetActive(true);
    }
    public void DeHighlight()
    {
        activeImage.SetActive(false);
    }
}
