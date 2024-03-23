using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.TextCore.Text;

public class BattleManager : SingletonMonobehaviour<BattleManager>
{
   
    private BattleState         CurState;
    private GameObject          currentCharacterGameObject;     //현재 누구 차례인지
    private int                 currentRound;                   //현재 몇 라운드인지

    #region 아군과 적군의 위치값
    [SerializeField] private Transform[] allyPosition = new Transform[7];
    [SerializeField] private Transform[] enemyPosition = new Transform[7];
    #endregion


    /// <summary>
    /// 아군이랑 적군의 싸움 순서
    /// </summary>
    [SerializeField] private Queue<GameObject> combatQueue = new Queue<GameObject>();
    [SerializeField, ReadOnly] private List<GameObject> allyFormation = new List<GameObject>();
    [SerializeField, ReadOnly] private List<GameObject> EnemyFormation = new List<GameObject>();

    private void Start()
    {
        CurState = BattleState.IDLE;
    }

    /// <summary>
    /// DungeonInfoSO 정보를 받아와서 아군과 적군 위치값 설정
    /// </summary>
    public void InitializeBattle(DungeonInfoSO dungeon)
    {
        CurState = BattleState.Initialization;
        if (dungeon == null) { Debug.LogError("Null Dungeon"); return; }

        currentRound = 0;
        combatQueue.Clear();
        allyFormation.Clear();
        EnemyFormation.Clear();

        #region 아군과 적군 배치
        // DungeonInfo내에 있는 적들을 알맞은 곳에 배치
        int EnemyTotalSize = 0;
        for(int i = 0;i<dungeon.EnemyList.Count; ++i)
        {
            GameObject enemyGameObject = dungeon.EnemyList[i];
            combatQueue.Enqueue(enemyGameObject);
            EnemyFormation.Add(enemyGameObject);
            if(enemyGameObject.GetComponent<BaseCharacter>().SpawnLocation != new Vector3(0, 0, 0))
            {
                Instantiate(enemyGameObject, enemyPosition[EnemyTotalSize]);
            }
        }
        foreach (GameObject enemyGameObject in dungeon.EnemyList)
        {
            combatQueue.Enqueue(enemyGameObject);
            //Todo : 캐릭터를 알맞은 곳에 배치
            Transform trans = enemyGameObject.transform;
            Instantiate(enemyGameObject, trans);
        }
       
        foreach(GameObject allyGameObject in GameManager.GetInstance.Allies)
        { 
            combatQueue.Enqueue(allyGameObject);
            //Todo : 아군 캐릭터 알맞게 배치
            Transform trans = allyGameObject.transform;   //임시 코드
            Instantiate(allyGameObject, trans);           //임시 코드
        }

        #endregion

        #region PreRound 상태로 넘어감
        PreRound();
        #endregion

    }

    /// <summary>
    /// 캐릭터들의 버프 정리
    /// </summary>
    void PreRound()
    {
        CurState = BattleState.PreRound;
        ++currentRound;
        CheckPreBuffs();
        //버프로 인한 캐릭터 사망 확인
        if (CheckVictory(combatQueue))
        {
            PostBattle(true);
        }
        else if (CheckDefeat(combatQueue))
        {
            PostBattle(false);
        }
        DetermineOrder();
    }

    /// <summary>
    /// 라운드가 시작할때 적용되는 버프 정리
    /// </summary>
    void CheckPreBuffs()
    {
        int characterCount = combatQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue에서 항목을 제거
            GameObject characterGameObject = combatQueue.Dequeue();
            BaseCharacter character = characterGameObject.GetComponent<BaseCharacter>();

            foreach (BaseBuff buff in character.activeBuffs)
            {
                buff.ApplyRoundStartBuff();
                //죽음 체크
                character.CheckDead();
            }

            // 수정된 character를 Queue의 뒤쪽에 다시 추가.
            combatQueue.Enqueue(characterGameObject);
        }

    }

    /// <summary>
    /// 캐릭터들을 속도순으로 정렬
    /// </summary>
    void DetermineOrder()
    {
        CurState = BattleState.DetermineOrder;
        SortBattleOrder();
        CharacterTurn();
    }

    /// <summary>
    /// 캐릭터들을 속도순으로 정렬
    /// </summary>
    void SortBattleOrder()
    {
        List<GameObject> SortList = new List<GameObject>();

        // Queue에 있는 요소 하나씩 꺼내서 List에 집어넣음
        foreach (GameObject character in combatQueue)
        {
            SortList.Add(character);
        }

        // List 내부의 characters를 Speed 속성을 기준으로 내림차순 정렬
        SortList.Sort((character1, character2) => character2.GetComponent<BaseCharacter>().Speed.CompareTo(character1.GetComponent<BaseCharacter>().Speed));

        // Queue를 비우고
        combatQueue.Clear();

        // 정렬된 List 내부의 characters를 다시 combatQueue에 집어넣음
        foreach (GameObject character in SortList)
        {
            combatQueue.Enqueue(character);
        }
    }

    /// <summary>
    /// 캐릭터들의 행동 시작
    /// </summary>
    void CharacterTurn()
    {
        CurState = BattleState.CharacterTurn;
        //캐릭터별로 행동
        StartCoroutine(HandleCharacterTurns());
       
    }

    IEnumerator HandleCharacterTurns()
    {
        List<GameObject> processedCharacters = new List<GameObject>();

        while (combatQueue.Count > 0)
        {
            currentCharacterGameObject = combatQueue.Dequeue();
            BaseCharacter currentCharacter = currentCharacterGameObject.GetComponent<BaseCharacter>();

            if (currentCharacter.IsDead)
            {
                processedCharacters.Add(currentCharacterGameObject);
                continue;
            }

            // 차례가 됐을 때 버프 적용
            currentCharacter.ApplyTurnStartBuffs();

            yield return StartCoroutine(WaitForSkillSelection(currentCharacter));

            // 스킬 사용으로 인한 속도 변경 처리
            ReordercombatQueue(processedCharacters);

            processedCharacters.Add(currentCharacterGameObject);

            // 승리 조건 체크
            if (CheckVictory(processedCharacters) && CheckVictory(combatQueue))
            {
                PostBattle(true);
                yield break;
            }
            //패배 조건 체크
            else if (CheckDefeat(processedCharacters) && CheckDefeat(combatQueue))
            {
                PostBattle(false);
                yield break;
            }


            yield return new WaitForSeconds(1f); // 스킬 애니메이션 등을 위한 대기 시간
        }

        //모든 캐릭터의 턴이 끝났을 때 실행
        PostRound();
    }

    /// <summary>
    /// 임시로 만든 함수. 대량 수정 필요
    /// </summary>
    IEnumerator WaitForSkillSelection(BaseCharacter character)
    {
        bool skillSelected = false;
       
        BaseSkill selectedSkill = null;
        int selectedSkillIndex = -1;

        // 스킬 선택을 위한 임시 UI 메시지
        Debug.Log("Press 1 or 2 to select a skill for " + character.name);

        while (!skillSelected)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // 여기서는 예시로 첫 번째 스킬을 선택했다고 가정.
                Debug.Log("Skill 1 selected");
                selectedSkill = character.skills[0];
                selectedSkillIndex = 0;
                skillSelected = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // 두 번째 스킬이 선택되었다고 가정.
                Debug.Log("Skill 2 selected");
                selectedSkill = character.skills[1];
                selectedSkillIndex = 1;
                skillSelected = true;
            }

            yield return null; // 다음 프레임까지 대기
        }

        // 선택된 스킬을 사용하는 로직
        if (selectedSkill != null && selectedSkillIndex != -1)
        {
            // 적군의 0번째 캐릭터에게 스킬을 쓴다고 가정
            BaseCharacter temporaryEnemy = EnemyFormation[0].GetComponent<BaseCharacter>();
            BaseCharacter temporaryCaster = allyFormation[0].GetComponent<BaseCharacter>();

            yield return StartCoroutine(ExecuteSkill(selectedSkillIndex, temporaryCaster, temporaryEnemy));
        }
    }

    // 스킬 실행 로직 구현
    IEnumerator ExecuteSkill(int _skillindex, BaseCharacter _caster, BaseCharacter _receiver)
    {
        BaseSkill skill = _caster.skills[_skillindex];

        yield return new WaitForSeconds(1f); // 예시로 1초 대기
    }

    void ReordercombatQueue(List<GameObject> processedCharacters)
    {
        List<GameObject> allCharacters = new List<GameObject>(processedCharacters);

        // combatQueue에 남아 있는 캐릭터를 모두 allCharacters 리스트에 추가
        while (combatQueue.Count > 0)
        {
            allCharacters.Add(combatQueue.Dequeue());
        }

        // allCharacters 리스트를 속도에 따라 재정렬
        allCharacters.Sort((character1, character2) => character2.GetComponent<BaseCharacter>().Speed.CompareTo(character1.GetComponent<BaseCharacter>().Speed));

        // 재정렬된 리스트를 바탕으로 combatQueue 재구성
        combatQueue.Clear();
        foreach (GameObject character in allCharacters)
        {
            combatQueue.Enqueue(character);
        }
    }


    /// <summary>
    /// 라운드가 끝날때 적용되는 버프 실행 후, 승리 조건 체크
    /// </summary>
    void PostRound()
    {
        CurState = BattleState.PostRound;
        CheckPostBuffs();
        //적군이 모두 죽으면 PostBattle로 넘어감. 아닐시 다시 PreRound로 돌아감
        if(CheckVictory(combatQueue))
        {
            PostBattle(true);
        }
        else if (CheckDefeat(combatQueue))
        {
            PostBattle(false);
        }
        {
            PreRound();
        }
    }

    void CheckPostBuffs()
    {
        int characterCount = combatQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue에서 항목을 제거
            GameObject characterGameObject = combatQueue.Dequeue();
            BaseCharacter character = characterGameObject.GetComponent<BaseCharacter>();

            foreach (BaseBuff buff in character.activeBuffs)
            {
                buff.ApplyRoundEndBuff();
                //죽음 체크
                character.CheckDead();
            }

            // 수정된 character를 Queue의 뒤쪽에 다시 추가합니다.
            combatQueue.Enqueue(characterGameObject);
        }
    }

    /// <summary>
    /// 적군이 모두 죽었는지 확인
    /// </summary>
    bool CheckVictory(IEnumerable<GameObject> characters)
    {

        foreach (GameObject characterGameObject in characters)
        {
            BaseCharacter character = characterGameObject.GetComponent<BaseCharacter>();
            if (!character.IsAlly && !character.IsDead)
            {
                return false; // 살아있는 적군이 있으므로 승리하지 않음
            }
        }
        return true;
    }

    /// <summary>
    /// 아군이 모두 죽었는지 확인
    /// </summary>
    bool CheckDefeat(IEnumerable<GameObject> characters)
    {
        foreach (GameObject characterGameObject in characters)
        {
            BaseCharacter character = characterGameObject.GetComponent<BaseCharacter>();
            if (character.IsAlly && !character.IsDead)
            {
                return false; // 살아있는 아군이 있으므로 패배하지 않음
            }
        }
        return true;
    }

    /// <summary>
    /// 보상 정산 후, 전투 종료
    /// </summary>
    void PostBattle(bool _victory)
    {
        //승리시
        if (_victory)
        {
            //승리 화면 뜬 후 보상 정산
        }
        else
        {
            //패배 화면 뜨기
        }
        //적군인 경우 삭제
        while (combatQueue.Count > 0)
        {
            BaseCharacter curchar = combatQueue.Dequeue().GetComponent<BaseCharacter>();
            if(curchar.IsAlly == false)
            {
                curchar.Destroy();
            }
        }
        combatQueue.Clear();
    }
}
