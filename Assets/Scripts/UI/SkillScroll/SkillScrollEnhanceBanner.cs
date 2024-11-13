using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillScrollEnhanceBanner : MonoBehaviour
{
    [SerializeField] private bool isAlphaBlending;
    [SerializeField] private bool showBanner;

    [SerializeField] private Image bannerImg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (isActive)
        // {
        //     //
        // }
    }

    public void Reset()
    {
        
    }
    
    public Image BannerImg
    {
        get => bannerImg;
        set => bannerImg = value;
    }
    public bool IsAlphaBlending
    {
        get => isAlphaBlending;
        set => isAlphaBlending = value;
    }
}
