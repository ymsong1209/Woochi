using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIcon : MonoBehaviour
{
    [SerializeField] private BuffType buffType;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private Canvas canvas;
    
    private GameObject tooltipInstance;

    public void Activate()
    {
        
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
    }

    public void CheckChildBuffs(BaseBuff _buff)
    {
        // Check if buffType is not statstrengthen or statweaken
        if (buffType != BuffType.StatStrengthen && buffType != BuffType.StatWeaken)
        {
            //부모 자식 연결 해제
            _buff.transform.parent = null;
            DeActivate();
        }
        else
        {
            // Check if _buff is statstrengthen or statweaken
            if (_buff.BuffType == BuffType.StatStrengthen || _buff.BuffType == BuffType.StatWeaken)
            {
                // Find a child with the same name as _buff
                Transform child = transform.Find(_buff.name);
                if (child != null)
                {
                    child.transform.parent = null;

                    // If there are no children left, deactivate
                    if (transform.childCount == 0)
                    {
                        DeActivate();
                    }
                }
            }
        }
    }

    public void OnMouseEnter()
    {
        if (tooltipInstance == null)
        {
            tooltipInstance = Instantiate(tooltipPrefab, canvas.transform);
            //tooltipInstance.GetComponentInChildren<Text>().text = buffType.ToString(); // BuffType의 설명을 텍스트로 설정
        }
        
        tooltipInstance.SetActive(true);
        tooltipInstance.transform.position = Input.mousePosition; // 팝업 위치를 마우스 위치로 설정
    }

    public void OnMouseExit()
    {
        if (tooltipInstance != null)
        {
            tooltipInstance.SetActive(false);
        }
    }
    
    #region Getter Setter
    public BuffType BuffType => buffType;
    // public BuffType SkillOwner
    // {
    //     get { return skillOwner; }
    //     set { skillOwner = value; }
    // }
    #endregion
}
