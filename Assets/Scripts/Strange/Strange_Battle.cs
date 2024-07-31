using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class Strange_Battle : BaseStrange
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button battleReadyBtn;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private MapNode node;
    
    // Start is called before the first frame update
    void Start()
    {
        continueBtn.onClick.AddListener(() => Deactivate());
        battleReadyBtn.onClick.AddListener(() => BattleSelect());
    }


    public override void Initialize()
    {
        base.Initialize();
        continueBtn.gameObject.SetActive(false);
        battleReadyBtn.gameObject.SetActive(true);
        descriptionText.text = "갑자기 주변에서 사나운 요기가 느껴진다.\n 전투 준비를 하자.";
    }

    public override void Activate(MapNode _node)
    {
        base.Activate(_node);
        node = _node;
    }

    public override void Deactivate()
    {
        gameObject.SetActive(false);
        StrangeManager.GetInstance.StrangeBackground.SetActive(false);
        BattleManager.GetInstance.InitializeBattle(node.Node.enemyIDs, node.Node.abnormalID);
    }
    private void BattleSelect()
    {
        //노드에 무엇이 배치될지는 Mapgenerator에서 이미 결정됨.
        if (node.Node.enemyIDs[0] < 5000)
        {
            descriptionText.text = "이정도는 물리쳐야 도사다. 해 보자!";
        }
        else
        {
            descriptionText.text = "조금 버거워보이는 적이다. 조심하자.";
        }
        battleReadyBtn.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(true);
    }
}
