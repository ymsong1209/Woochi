using System;
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
    private BaseSkill           currentSelectedSkill;           //현재 선택된 스킬
    private int                 currentRound;                   //현재 몇 라운드인지

    #region 아군과 적군의 위치값
    /// <summary>
    /// 크기가 1인 아군 위치값 
    /// 0~3 : 1열~4열 
    /// </summary>
    [SerializeField] private Vector3[] allySinglePosition = new Vector3[4];
    /// <summary>
    /// 크기가 2인 아군 위치값
    /// 0 : 1~2열 사이
    /// 1 : 2~3열 사이
    /// 2 : 3~4열 사이
    /// </summary>
    [SerializeField] private Vector3[] allyMultiplePosition = new Vector3[3];
    /// <summary>
    /// 크기가 1인 적군 위치값 
    /// 0~3 : 1열~4열 
    /// </summary>
    [SerializeField] private Vector3[] enemySinglePosition = new Vector3[4];
    /// <summary>
    /// 크기가 2인 적군 위치값
    /// 0 : 1~2열 사이
    /// 1 : 2~3열 사이
    /// 2 : 3~4열 사이
    /// </summary>
    [SerializeField] private Vector3[] enemyMultiplePosition = new Vector3[3];
    #endregion


    /// <summary>
    /// 아군이랑 적군의 싸움 순서
    /// </summary>
    [SerializeField] private Queue<GameObject> combatQueue = new Queue<GameObject>();
    [SerializeField, ReadOnly] private GameObject[] allyFormation = new GameObject[4];
    [SerializeField, ReadOnly] private GameObject[] EnemyFormation = new GameObject[4];

    #region 이벤트
    /// <summary>
    /// 캐릭터 턴이 시작될 때 호출되는 이벤트(UI 업데이트 등)
    /// </summary>
    public Action<BaseCharacter> OnCharacterTurnStart;
    #endregion

    #region 부울 변수
    [Header("Boolean Variables")]
    [HideInInspector] public bool isSkillSelected = false;
    [HideInInspector] public bool isSkillExecuted = false;
    #endregion

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
        for(int i = 0;i<allyFormation.Length; ++i)
        {
            allyFormation[i] = null;
        }
        for(int i = 0;i<EnemyFormation.Length; ++i)
        {
            EnemyFormation[i] = null;
        }

        #region 아군과 적군 배치
        #region 적군 배치
        // DungeonInfo내에 있는 적들을 알맞은 곳에 배치
        int EnemyTotalSize = 0;
        for(int i = 0;i<dungeon.EnemyList.Count; ++i)
        {
            GameObject enemyGameObject = dungeon.EnemyList[i];
            BaseCharacter enemyCharacter = enemyGameObject.GetComponent<BaseCharacter>();

            //턴 소비 체크
            enemyCharacter.IsTurnUsed = false;
            //전투 시작시 적용되는 버프 적용
            enemyCharacter.ApplyBuff(BuffTiming.BattleStart);

            //전투 순서에 삽입
            combatQueue.Enqueue(enemyGameObject);
            EnemyFormation[i] = enemyGameObject;

            //위치값을 정해야하는 특수한 객체가 아니면 enemyPosition대로 정상 소환
            int enemySize = enemyCharacter.Size;
            if (enemyCharacter.IsSpawnSpecific == false)
            {
                //크기가 1인 적군
                if (enemySize == 1)
                {
                    Instantiate(enemyGameObject, enemySinglePosition[EnemyTotalSize], Quaternion.identity);
                    ++EnemyTotalSize;
                }
                //크기가 2인 적군
                else if (enemySize == 2) {
                    Instantiate(enemyGameObject, enemyMultiplePosition[EnemyTotalSize], Quaternion.identity);
                    EnemyTotalSize += 2;
                }
            }
            //위치를 정해줘야하는 특수한 객체
            else
            {
                //크기가 1인 적군
                if (enemySize == 1)
                {
                    Instantiate(enemyGameObject, enemyCharacter.SpawnLocation, enemyCharacter.SpawnRotation);
                    ++EnemyTotalSize;
                }
                //크기가 2인 적군
                else if (enemySize == 2)
                {
                    Instantiate(enemyGameObject, enemyCharacter.SpawnLocation, enemyCharacter.SpawnRotation);
                    EnemyTotalSize += 2;
                }
            }
        }
        if (EnemyTotalSize > 4) Debug.LogError("EnemyTotalSize over 4");
        #endregion 적군 배치
        #region 아군 배치
        int AllyTotalSize = 0;
        for (int i = 0; i < GameManager.GetInstance.Allies.Count; ++i)
        {
            GameObject allyGameObject = GameManager.GetInstance.Allies[i];
            BaseCharacter allyCharacter = allyGameObject.GetComponent<BaseCharacter>();

            //전투 순서에 삽입
            combatQueue.Enqueue(allyGameObject);
            allyFormation[i] = allyGameObject;

            //턴 소비 체크
            allyCharacter.IsTurnUsed = false;
            //전투 시작 시 적용되는 버프 적용
            allyCharacter.ApplyBuff(BuffTiming.BattleStart);

            //위치값을 정해야하는 특수한 객체가 아니면 allyPosition대로 정상 소환
            int allySize = allyCharacter.Size;
            if (allyCharacter.IsSpawnSpecific == false)
            {
                //크기가 1인 아군
                if (allySize == 1)
                {
                    Instantiate(allyGameObject, allySinglePosition[AllyTotalSize], Quaternion.identity);
                    ++AllyTotalSize;
                }
                //크기가 2인 아군
                else if (allySize == 2)
                {
                    Instantiate(allyGameObject, allyMultiplePosition[AllyTotalSize], Quaternion.identity);
                    AllyTotalSize += 2;
                }
            }
            //위치를 정해줘야하는 특수한 객체
            else
            {
                //크기가 1인 아군
                if (allySize == 1)
                {
                    Instantiate(allyGameObject, allyCharacter.SpawnLocation, allyCharacter.SpawnRotation);
                    ++AllyTotalSize;
                }
                //크기가 2인 아군
                else if (allySize == 2)
                {
                    Instantiate(allyGameObject, allyCharacter.SpawnLocation, allyCharacter.SpawnRotation);
                    AllyTotalSize += 2;
                }
            }
        }
        #endregion 아군 배치
        #endregion 아군과 적군 배치

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

            character.ApplyBuff(BuffTiming.RoundStart);

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
        //캐릭터를 속도순으로 정렬하면서 모두 전투에 참여할 수 있또록 변경
        ReordercombatQueue(true);
        CharacterTurn();
    }

    /// <summary>
    /// combatQueue를 다시 속도순으로 정렬, ResetTurnUsed를 true로 하면 모든 캐릭터가 턴을 다시 쓸 수 있음
    /// </summary>
    void ReordercombatQueue(bool _ResetTurnUsed = false)
    {
        List<GameObject> allCharacters = new List<GameObject>();

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
            if (_ResetTurnUsed)
            {
                character.GetComponent<BaseCharacter>().IsTurnUsed = false;
            }
            combatQueue.Enqueue(character);
        }
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
            #region 이전 턴에 쓰인 변수 초기화
            isSkillSelected = false;
            isSkillExecuted = false;
            currentSelectedSkill = null;
            #endregion
            currentCharacterGameObject = combatQueue.Dequeue();
            BaseCharacter currentCharacter = currentCharacterGameObject.GetComponent<BaseCharacter>();

            if (currentCharacter.IsDead || currentCharacter.IsTurnUsed)
            {
                processedCharacters.Add(currentCharacterGameObject);
                continue;
            }

            // 자신의 차례가 됐을 때 버프 적용
            if (currentCharacter.ApplyBuff(BuffTiming.TurnStart))
            {
                // 현재 턴의 캐릭터에 맞는 UI 업데이트
                OnCharacterTurnStart?.Invoke(currentCharacter);

                // 스킬이 선택되고 실행될 때까지 대기
                while(!isSkillSelected && !isSkillExecuted)
                {
                    yield return null;
                }

                // 스킬 사용 완료 후 턴 사용 처리
                currentCharacter.IsTurnUsed = true;
            };

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

            yield return null;
        }
        //ProcessedCharacter에 있는 캐릭터들 다시 characterQueue에 삽입
        foreach(GameObject characters in processedCharacters)
        {
            combatQueue.Enqueue(characters);
        }

        //모든 캐릭터의 턴이 끝났을 때 실행
        PostRound();
    }

    /// <summary>
    /// UI에서 스킬 선택 시 호출되는 메서드
    /// </summary>
    /// <param name="_selectedSkill">선택된 스킬 정보</param>
    public void SkillSelected(BaseSkill _selectedSkill)
    {
        Debug.Log("Skill selected: " + _selectedSkill.Name);
        // 사용할 스킬 저장
        currentSelectedSkill = _selectedSkill;  
        isSkillSelected = true;

        // TODO : 범위 기반으로 스킬 대상 지정하는 코드 
        // 스킬의 적용 대상을 결정해서 클릭할 수 있게 하기

        // 적군의 0번째 캐릭터에게 스킬을 쓴다고 가정
        BaseCharacter temporaryCaster = _selectedSkill.SkillOwner;
        BaseCharacter temporaryEnemy = EnemyFormation[0].GetComponent<BaseCharacter>();

        StartCoroutine(ExecuteSkill(temporaryCaster, temporaryEnemy));
    }

    // 스킬 실행 로직 구현
    IEnumerator ExecuteSkill(BaseCharacter _caster, BaseCharacter _receiver)
    {
        Debug.Log(currentSelectedSkill.Name + " is executed by " + _caster.name + " on " + _receiver.name);
        isSkillExecuted = true;

        yield return new WaitForSeconds(1f); // 예시로 1초 대기
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
            character.ApplyBuff(BuffTiming.RoundEnd);

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
                Destroy(curchar.gameObject);
            }
        }
        combatQueue.Clear();
        GameManager.GetInstance.SelectRoom();
    }
}
