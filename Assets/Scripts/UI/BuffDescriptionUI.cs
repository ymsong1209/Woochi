using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDescriptionUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> buffDescriptions;
    [SerializeField] private SkillDescriptionUI skillDescriptionUI;

    public void Activate(BaseSkill skill)
    {
        gameObject.SetActive(true);
    }
    void Start()
    {
        foreach(GameObject buffDescription in buffDescriptions)
        {
            buffDescription.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!skillDescriptionUI) return;
        //skillDescription의 panelRt의 위치를 고려해 buffDescriptionUI를 skillDescription의 오른쪽에 위치시킴.
        RectTransform skillPanelRt = skillDescriptionUI.PanelRt;
        Vector2 skillPanelSize = skillPanelRt.sizeDelta;
        
        RectTransform buffPanelRt = GetComponent<RectTransform>();
        float horizontalOffset = 30f;
        Vector3 buffUIPosition = new Vector3(
            skillPanelRt.transform.position.x + skillPanelSize.x / 2 + buffPanelRt.sizeDelta.x / 2 + horizontalOffset,
            skillPanelRt.transform.position.y - skillPanelSize.y / 2 + buffPanelRt.sizeDelta.y / 2, 
            skillPanelRt.transform.position.z
        );
        transform.position = buffUIPosition;
    }
    
    public SkillDescriptionUI SkillDescriptionUI
    {
        set => skillDescriptionUI = value;
    }
}
