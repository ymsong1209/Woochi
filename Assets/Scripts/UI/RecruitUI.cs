using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecruitUI : MonoBehaviour
{
    [SerializeField] private Button accept;
    public TextMeshProUGUI recruitText;
    
    private Action onAcceptComplete;
    
   
    void Start()
    {
        accept.onClick.AddListener(OnAcceptButtonClick);
    }
    
    private void OnAcceptButtonClick()
    {
        gameObject.SetActive(false);
        StopAllCoroutines();
        onAcceptComplete?.Invoke(); // 대기 완료 콜백 호출
    }

    //allylist에 아군을 추가
    public void Recruit(int allyid, Action onComplete)
    {
        //allies에 allyid가 없을 경우 추가
        if(DataCloud.playerData.battleData.allies.Contains(allyid)) return;
        
        DataCloud.playerData.battleData.allies.Add(allyid);
        BattleManager.GetInstance.InitializeAlly();
        BaseCharacter ally = GameManager.GetInstance.Library.GetCharacter(allyid).GetComponent<BaseCharacter>();
        recruitText.text = ally.Name + "이(가) 파티에 합류했습니다";
        gameObject.SetActive(true);
        
        onAcceptComplete = onComplete; // 완료 콜백 저장
        StartCoroutine(WaitForAccept());
    }
    
    // accept 버튼이 눌리기 전까지 대기하는 Coroutine
    private IEnumerator WaitForAccept()
    {
        
        while (gameObject.activeSelf)
        {
            yield return null; 
        }
        
    }
}
