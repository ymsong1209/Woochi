using System.Collections;
using System.Collections.Generic;
using OneLine;
using UnityEngine;

public class BuffDescriptionUI : MonoBehaviour
{
    [SerializeField,OneLineWithHeader] private List<BuffKeyValue> buffDescriptions = new List<BuffKeyValue>();
    private Dictionary<BuffEffect, GameObject> buffDictionary = new Dictionary<BuffEffect, GameObject>();
    [SerializeField] private SkillDescriptionUI skillDescriptionUI;

    public void Activate(BaseSkill skill)
    {
        gameObject.SetActive(true);
        foreach(KeyValuePair<BuffEffect,GameObject> pair in buffDictionary)
        {
            pair.Value.SetActive(false);
        }
        foreach(GameObject buffobject in skill.BuffPrefabList)
        {
            BuffEffect effect = buffobject.GetComponent<BaseBuff>().BuffEffect;
            if (buffDictionary.ContainsKey(effect))
            {
                buffDictionary[effect].SetActive(true);
            }
        }
    }

    void Awake()
    {
        // List를 Dictionary로 변환
        foreach (BuffKeyValue kv in buffDescriptions)
        {
            if (!buffDictionary.ContainsKey(kv.key))
            {
                buffDictionary.Add(kv.key, kv.value);
            }
        }
    }
    void Start()
    {
        foreach(KeyValuePair<BuffEffect, GameObject> buffDescription in buffDictionary)
        {
            buffDescription.Value.SetActive(false);
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


[System.Serializable]
public class BuffKeyValue
{
    public BuffEffect key;
    public GameObject value;
}