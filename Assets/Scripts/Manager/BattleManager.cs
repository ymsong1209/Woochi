using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.TextCore.Text;

public class BattleManager : SingletonMonobehaviour<BattleManager>
{
   
    private BattleState         CurState;
    public GameObject           currentCharacterGameObject;     //현재 누구 차례인지
    private BaseSkill           currentSelectedSkill;           //현재 선택된 스킬
    private int                 currentRound;                   //현재 몇 라운드인지

    [SerializeField] private GameObject skillTriggerSelector;

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
    [SerializeField, ReadOnly] private GameObject[] enemyFormation = new GameObject[4];

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
        skillTriggerSelector = GetComponentInChildren<SkillTriggerSelector>()?.gameObject;
        skillTriggerSelector.SetActive(false);
    }

    /// <summary>
    /// DungeonInfoSO 정보를 받아와서 아군과 적군 위치값 설정
    /// </summary>
    public void InitializeBattle(DungeonInfoSO dungeon)
    {
        CurState = BattleState.Initialization;
        if (dungeon == null) { Debug.LogError("Null Dungeon"); return; }

        skillTriggerSelector.SetActive(true);

        currentRound = 0;
        combatQueue.Clear();
        for(int i = 0;i<allyFormation.Length; ++i)
        {
            allyFormation[i] = null;
        }
        for(int i = 0;i<enemyFormation.Length; ++i)
        {
            enemyFormation[i] = null;
        }

        #region 아군과 적군 배치
        #region 적군 배치
        // DungeonInfo내에 있는 적들을 알맞은 곳에 배치
        int EnemyTotalSize = 0;
        for(int i = 0;i<dungeon.EnemyList.Count; ++i)
        {
            GameObject enemyPrefab = dungeon.EnemyList[i];

            GameObject enemyGameObject = Instantiate(enemyPrefab);
            BaseCharacter enemyCharacter = enemyGameObject.GetComponent<BaseCharacter>();
            enemyCharacter.Initialize();
            //턴 소비 체크
            enemyCharacter.IsTurnUsed = false;
            //전투 시작시 적용되는 버프 적용
            enemyCharacter.ApplyBuff(BuffTiming.BattleStart);

            //전투 순서에 삽입
            combatQueue.Enqueue(enemyGameObject);
            enemyFormation[EnemyTotalSize] = enemyGameObject;

            //위치값을 정해야하는 특수한 객체가 아니면 enemyPosition대로 정상 소환
            int enemySize = enemyCharacter.Size;
            if (enemyCharacter.IsSpawnSpecific == false)
            {
                //크기가 1인 적군
                if (enemySize == 1)
                {
                    enemyGameObject.transform.position = enemySinglePosition[EnemyTotalSize];
                    ++EnemyTotalSize;
                }
                //크기가 2인 적군
                else if (enemySize == 2) {
                    enemyGameObject.transform.position = enemyMultiplePosition[EnemyTotalSize];
                    enemyFormation[EnemyTotalSize + 1] = enemyGameObject;
                    EnemyTotalSize += 2;
                }
            }
            //위치를 정해줘야하는 특수한 객체
            else
            {
                //크기가 1인 적군
                if (enemySize == 1)
                {
                    enemyGameObject.transform.position = enemyCharacter.SpawnLocation;
                    
                    ++EnemyTotalSize;
                }
                //크기가 2인 적군
                else if (enemySize == 2)
                {
                    enemyGameObject.transform.position = enemyCharacter.SpawnLocation;
                    enemyFormation[EnemyTotalSize + 1] = enemyGameObject;
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
            GameObject allyPrefab = GameManager.GetInstance.Allies[i];
            GameObject allyGameObject = Instantiate(allyPrefab);
            BaseCharacter allyCharacter = allyGameObject.GetComponent<BaseCharacter>();

            allyCharacter.Initialize();
            allyCharacter.IsAlly = true;

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
                    allyGameObject.transform.position = allySinglePosition[AllyTotalSize];
                    ++AllyTotalSize;
                }
                //크기가 2인 아군
                else if (allySize == 2)
                {
                    allyGameObject.transform.position = allyMultiplePosition[AllyTotalSize];
                    AllyTotalSize += 2;
                }
            }
            //위치를 정해줘야하는 특수한 객체
            else
            {
                //크기가 1인 아군
                if (allySize == 1)
                {
                    allyGameObject.transform.position = allyCharacter.SpawnLocation;
                    ++AllyTotalSize;
                }
                //크기가 2인 아군
                else if (allySize == 2)
                {
                    allyGameObject.transform.position = allyCharacter.SpawnLocation;
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
        //캐릭터를 속도순으로 정렬하면서 모두 전투에 참여할 수 있도록 변경
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
        Debug.Log("CurState : CharacterTurn");
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

                // 닼던처럼 한다면 현재 턴이 적이라면 UI를 업데이트 하지 않는 식으로 하는게 좋을 것 같음
                if (currentCharacter.IsAlly)
                {
                    //캐릭터의 스킬에 변경점이 있는지 확인
                    currentCharacter.CheckSkillsOnTurnStart();
                }
                // TODO : 현재 턴이 적일 시 AI로 행동 결정(임시 코드)
                if(!currentCharacter.IsAlly)
                    StartCoroutine(EnemyAction(currentCharacter));

                // 스킬이 선택되고 실행될 때까지 대기
                while(!isSkillSelected || !isSkillExecuted)
                {
                    yield return null;
                }
                
            };

            // 자신 차례가 지난 후 턴 사용 처리
            currentCharacter.IsTurnUsed = true;

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
    public void SkillSelected(BaseSkill _selectedSkill)
    {
        // 사용할 스킬 저장
        currentSelectedSkill = _selectedSkill;
        isSkillSelected = true;
    }

    public void ExecuteSelectedSkill(int _index = -1)
    {
        // 적군의 0번째 캐릭터에게 스킬을 쓴다고 가정
        if (currentSelectedSkill == null) return;
        //단일공격 스킬일 경우 index에 들어온 적만 공격
        if(currentSelectedSkill.SkillTargetType == SkillTargetType.Singular)
        {
            AttackSingular(_index);
        }
        else if(currentSelectedSkill.SkillTargetType == SkillTargetType.Multiple)
        {
            AttackMultiple(_index);
        }
    }

    /// <summary>
    /// Index의 위치에 따라 적 한명에게 공격한다.
    /// </summary>
    private void AttackSingular(int _index)
    {
        BaseCharacter Caster = null;
        List<BaseCharacter> Enemies = new List<BaseCharacter>();

        if (CurrentSelectedSkill == null) return;
        Caster = currentSelectedSkill.SkillOwner;


        //index<4인경우는 적이 아군을 공격한 경우
        if (_index < 4)
        {
           Enemies.Add(allyFormation[_index].GetComponent<BaseCharacter>());
        }
        //4<index<8인 경우는 아군이 적을 공격한 경우
        else if (_index < 8)
        {
            Enemies.Add(enemyFormation[_index - 4].GetComponent<BaseCharacter>());
        }
       
        if (Caster != null && Enemies.Count > 0)
        {
            StartCoroutine(ExecuteSkill(Caster, Enemies));
        }
        else
        {
            Debug.LogError("Caster or Enemy not assigned!");
        }
    }

    private void AttackMultiple(int _index)
    {
        BaseCharacter Caster = null;
        List<BaseCharacter> Enemies = new List<BaseCharacter>();
        if (currentSelectedSkill == null) return;
        Caster = currentSelectedSkill.SkillOwner;

        //Index 여부와 상관없이 selectedSkill의 범위 내의 모든 공격 가능한 몹들 공격
        for (int i = 0;i<currentSelectedSkill.SkillRadius.Length; i++)
        {
            //i<4일 경우 적이 아군을 공격
            if (i < 4 && currentSelectedSkill.SkillRadius[i])
            {
                Enemies.Add(allyFormation[i].GetComponent<BaseCharacter>());
            }
            else if (4 <= i && i < 8 && currentSelectedSkill.SkillRadius[i])
            {
                BaseCharacter enemy = enemyFormation[i - 4].GetComponent<BaseCharacter>();
                //적의 Size가 2인 경우
                if(enemy.Size == 2)
                {
                    // 이미 Enemies 리스트에 동일한 GameObject를 참조하는 BaseCharacter가 없는 경우에만 추가
                    if (!Enemies.Any(e => e.gameObject == enemy.gameObject))
                    {
                        Enemies.Add(enemy);
                    }
                }
                else
                {
                    // Size가 1인 적은 그냥 추가
                    Enemies.Add(enemy);
                }

            }
        }

        if (Caster != null && Enemies.Count > 0)
        {
            StartCoroutine(ExecuteSkill(Caster, Enemies));
        }
        else
        {
            Debug.LogError("Caster or Enemy not assigned!");
        }
    }

    // 스킬 실행 로직 구현
    IEnumerator ExecuteSkill(BaseCharacter _caster, List<BaseCharacter> _receivers)
    {
        foreach(BaseCharacter receiver in _receivers)
        {
            currentSelectedSkill.ApplySkill(receiver);
            Debug.Log(currentSelectedSkill.Name + " is executed by " + _caster.name + " on " + receiver.name);
        }
        
        isSkillExecuted = true;

        yield return new WaitForSeconds(1f); // 예시로 1초 대기
    }

    /// <summary>
    /// Enemy 임시 행동
    /// </summary>
    IEnumerator EnemyAction(BaseCharacter _enemy)
    {
        Debug.Log(_enemy.name + "가 행동합니다");
        yield return new WaitForSeconds(3f); // 예시로 3초 대기 후 스킬 실행 가정
        isSkillSelected = true;
        isSkillExecuted = true;
    }
    
    public void TurnOver()
    {
        isSkillSelected = true;
        isSkillExecuted = true;
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

    #region Getter Setter

    public GameObject[] AllyFormation
    {
        get { return allyFormation; }
        private set { allyFormation = value; } 
    }

    public GameObject[] EnemyFormation
    {
        get { return enemyFormation; }
        private set { enemyFormation = value; }
    }
    public BaseSkill CurrentSelectedSkill => currentSelectedSkill;
    #endregion
}
