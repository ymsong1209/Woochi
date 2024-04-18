using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : SingletonMonobehaviour<BattleManager>
{
   
    private BattleState         CurState;
    public  BaseCharacter       currentCharacter;               //현재 누구 차례인지
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
    [SerializeField] private Queue<BaseCharacter> combatQueue = new Queue<BaseCharacter>();
    [SerializeField, ReadOnly] private BaseCharacter[] allyFormation = new BaseCharacter[4];
    [SerializeField, ReadOnly] private BaseCharacter[] enemyFormation = new BaseCharacter[4];

    #region 이벤트
    /// <summary>
    /// 캐릭터 턴이 시작될 때 호출되는 이벤트(UI 업데이트 등)
    /// </summary>
    public Action<BaseCharacter, bool> OnCharacterTurnStart;
    #endregion

    #region 부울 변수
    [Header("Boolean Variables")]
    private bool isSkillSelected = false;
    private bool isSkillExecuted = false;
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

        for(int i = 0; i < allyFormation.Length; ++i)
        {
            allyFormation[i] = null;
            enemyFormation[i] = null;
        }

        #region 아군과 적군 배치
        #region 적군 배치
        // DungeonInfo내에 있는 적들을 알맞은 곳에 배치
        int EnemyTotalSize = 0;
        for (int i = 0; i < dungeon.EnemyList.Count; ++i)
        {
            if (EnemyTotalSize > 4) return;
            GameObject enemyPrefab = dungeon.EnemyList[i];

            GameObject enemyGameObject = Instantiate(enemyPrefab);
            BaseCharacter enemyCharacter = enemyGameObject.GetComponent<BaseCharacter>();

            enemyCharacter.Initialize();
            enemyCharacter.IsAlly = false;

            //턴 소비 체크
            enemyCharacter.IsTurnUsed = false;
            //전투 시작시 적용되는 버프 적용
            enemyCharacter.ApplyBuff(BuffTiming.BattleStart);

            //전투 순서에 삽입
            combatQueue.Enqueue(enemyCharacter);
            enemyCharacter.RowOrder = EnemyTotalSize;
            enemyFormation[EnemyTotalSize++] = enemyCharacter;
            if(enemyCharacter.Size == 2)
            {
                enemyFormation[EnemyTotalSize++] = enemyCharacter;
            }
        }
        #endregion 적군 배치
        #region 아군 배치
        int AllyTotalSize = 0;
        for (int i = 0; i < GameManager.GetInstance.Allies.Count; ++i)
        {
            if(AllyTotalSize > 4) return;
            GameObject allyPrefab = GameManager.GetInstance.Allies[i];

            GameObject allyGameObject = Instantiate(allyPrefab);
            BaseCharacter allyCharacter = allyGameObject.GetComponent<BaseCharacter>();

            allyCharacter.Initialize();
            allyCharacter.IsAlly = true;

            //턴 소비 체크
            allyCharacter.IsTurnUsed = false;
            //전투 시작 시 적용되는 버프 적용
            allyCharacter.ApplyBuff(BuffTiming.BattleStart);

            //전투 순서에 삽입
            combatQueue.Enqueue(allyCharacter);
            allyCharacter.RowOrder = AllyTotalSize;
            allyFormation[AllyTotalSize++] = allyCharacter;
            if(allyCharacter.Size == 2)
            {
                allyFormation[AllyTotalSize++] = allyCharacter;
            }
        }
        #endregion 아군 배치
        PlaceFormation();
        #endregion 아군과 적군 배치

        OnCharacterTurnStart?.Invoke(allyFormation[0], false);
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
        CheckBuffs(BuffTiming.RoundStart);
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
    /// BuffTiming을 매개변수로 받아서 해당 시점에 버프를 적용
    /// </summary>
    void CheckBuffs(BuffTiming buffTiming)
    {
        int characterCount = combatQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue에서 항목을 제거
            BaseCharacter character = combatQueue.Dequeue();

            character.ApplyBuff(buffTiming);

            // 수정된 character를 Queue의 뒤쪽에 다시 추가.
            combatQueue.Enqueue(character);
        }
    }

    /// <summary>
    /// 캐릭터들을 속도순으로 정렬
    /// </summary>
    void DetermineOrder()
    {
        CurState = BattleState.DetermineOrder;
        //캐릭터를 속도순으로 정렬하면서 모두 전투에 참여할 수 있도록 변경
        ReorderCombatQueue(true, null);
        CharacterTurn();
    }

    /// <summary>
    /// combatQueue를 다시 속도순으로 정렬, ResetTurnUsed를 true로 하면 모든 캐릭터가 턴을 다시 쓸 수 있음
    /// </summary>
    /// <param name="_resetTurnUsed">true로 설정 시 모든 캐릭터 다시 턴 사용가능</param>
    /// <param name="processedCharacters"></param>
    void ReorderCombatQueue(bool _resetTurnUsed = false, List<BaseCharacter> processedCharacters = null)
    {
        List<BaseCharacter> allCharacters = new List<BaseCharacter>();

        if (processedCharacters != null)
        {
            allCharacters.AddRange(processedCharacters);
        }

        // combatQueue에 남아 있는 캐릭터를 모두 allCharacters 리스트에 추가
        while (combatQueue.Count > 0)
        {
            allCharacters.Add(combatQueue.Dequeue());
        }

        // allCharacters 리스트를 속도에 따라 재정렬
        allCharacters.Sort((character1, character2) => character2.Speed.CompareTo(character1.Speed));

        // 재정렬된 리스트를 바탕으로 combatQueue 재구성
        combatQueue.Clear();
        foreach (BaseCharacter character in allCharacters)
        {
            if (_resetTurnUsed)
            {
                character.IsTurnUsed = false;
            }
            combatQueue.Enqueue(character);
        }
    }

    /// <summary>
    /// 턴이 끝난 후, 바뀐 RowOrder 값을 기반으로 formation을 정렬 후 재배치한다
    /// 캐릭터들을 앞으로 당긴다
    /// </summary>
    void ReorderFormation()
    {
        Array.Sort(allyFormation, (character1, character2) => {
            if (character1 == null && character2 == null)
                return 0;
            else if (character1 == null)
                return 1; 
            else if (character2 == null)
                return -1; 

            return character1.RowOrder.CompareTo(character2.RowOrder);
        });
        Array.Sort(enemyFormation, (character1, character2) => {
            if (character1 == null && character2 == null)
                return 0;
            else if (character1 == null)
                return 1;
            else if (character2 == null)
                return -1;

            return character1.RowOrder.CompareTo(character2.RowOrder);
        });

        PlaceFormation();
    }

    /// <summary>
    /// allyFormation과 enemyFormation을 바탕으로 캐릭터들의 위치를 배치
    /// </summary>
    void PlaceFormation()
    {
        for (int AllyTotalSize = 0; AllyTotalSize < allyFormation.Length; ++AllyTotalSize)
        {
            if (allyFormation[AllyTotalSize] == null) continue;

            BaseCharacter ally = allyFormation[AllyTotalSize];
            int allySize = ally.Size;
            if (ally.IsSpawnSpecific == false)
            {
                //크기가 1인 적군
                if (allySize == 1)
                {
                    ally.gameObject.transform.position = allySinglePosition[AllyTotalSize];
                }
                //크기가 2인 적군
                else if (allySize == 2)
                {
                    ally.gameObject.transform.position = allyMultiplePosition[AllyTotalSize];
                    AllyTotalSize++;
                }
            }
            //위치를 정해줘야하는 특수한 객체
            else
            {
                //크기가 1인 적군
                if (allySize == 1)
                {
                    ally.gameObject.transform.position = ally.SpawnLocation;
                }
                //크기가 2인 적군
                else if (allySize == 2)
                {
                    ally.gameObject.transform.position = ally.SpawnLocation;
                    AllyTotalSize++;
                }
            }
        }
        for (int EnemyTotalSize = 0; EnemyTotalSize < enemyFormation.Length; ++EnemyTotalSize)
        {
            if (enemyFormation[EnemyTotalSize] == null) continue;

            BaseCharacter enemy = enemyFormation[EnemyTotalSize];
            int enemySize = enemy.Size;
            if (enemy.IsSpawnSpecific == false)
            {
                //크기가 1인 적군
                if (enemySize == 1)
                {
                    enemy.gameObject.transform.position = enemySinglePosition[EnemyTotalSize];
                }
                //크기가 2인 적군
                else if (enemySize == 2)
                {
                    enemy.gameObject.transform.position = enemyMultiplePosition[EnemyTotalSize];
                    EnemyTotalSize++;
                }
            }
            //위치를 정해줘야하는 특수한 객체
            else
            {
                //크기가 1인 적군
                if (enemySize == 1)
                {
                    enemy.gameObject.transform.position = enemy.SpawnLocation;
                }
                //크기가 2인 적군
                else if (enemySize == 2)
                {
                    enemy.gameObject.transform.position = enemy.SpawnLocation;
                    EnemyTotalSize++;
                }
            }
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
        List<BaseCharacter> processedCharacters = new List<BaseCharacter>();

        while (combatQueue.Count > 0)
        {
            #region 이전 턴에 쓰인 변수 초기화
            isSkillSelected = false;
            isSkillExecuted = false;
            currentSelectedSkill = null;
            #endregion
            currentCharacter = combatQueue.Dequeue();

            if (currentCharacter.IsDead || currentCharacter.IsTurnUsed)
            {
                processedCharacters.Add(currentCharacter);
                continue;
            }

            // 자신의 차례가 됐을 때 버프 적용
            if (currentCharacter.ApplyBuff(BuffTiming.TurnStart))
            {
                // 캐릭터의 스킬에 변경점이 있는지 확인
                // 적도 위치가 바뀔 수 있으니 스킬 확인을 해줌
                currentCharacter.CheckSkillsOnTurnStart();

                // 현재 턴의 캐릭터에 맞는 UI 업데이트
                OnCharacterTurnStart?.Invoke(currentCharacter, true);

                // TODO : 현재 턴이 적일 시 AI로 행동 결정(임시 코드)
                if (!currentCharacter.IsAlly)
                    StartCoroutine(EnemyAction(currentCharacter));
                
                // 스킬이 선택되고 실행될 때까지 대기
                while(!isSkillSelected || !isSkillExecuted)
                {
                    yield return null;
                }
            };

            // 자신 차례가 지난 후 턴 사용 처리
            currentCharacter.IsTurnUsed = true;

            ReorderFormation();
            // 스킬 사용으로 인한 속도 변경 처리
            ReorderCombatQueue(false, processedCharacters);

            processedCharacters.Add(currentCharacter);

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
        foreach(BaseCharacter characters in processedCharacters)
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

    #region 스킬 사용
    public void ExecuteSelectedSkill(int _index = -1)
    {
        if (currentSelectedSkill == null) return;

        // 스킬 사용한 캐릭터 애니메이션 실행, 스킬 사용 후 상대 캐릭터 애니메이션도 실행해야 함(회피도 애니메이션 있나) 
        BaseCharacter caster = currentSelectedSkill.SkillOwner;
        caster.PlayAnimation(currentSelectedSkill.SkillSO.AnimType);

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
           Enemies.Add(allyFormation[_index]);
        }
        //4<index<8인 경우는 아군이 적을 공격한 경우
        else if (_index < 8)
        {
            Enemies.Add(enemyFormation[_index - 4]);
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
        List<BaseCharacter> Receivers = new List<BaseCharacter>();
        if (currentSelectedSkill == null) return;
        Caster = currentSelectedSkill.SkillOwner;

        //Index 여부와 상관없이 selectedSkill의 범위 내의 모든 공격 가능한 몹들 공격
        for (int i = 0; i < currentSelectedSkill.SkillRadius.Length; i++)
        {
            //i<4일 경우 적이 아군을 공격
            if (i < 4 && currentSelectedSkill.SkillRadius[i])
            {
                BaseCharacter ally = allyFormation[i];
                if(ally == null) continue;

                //아군의 Size가 2인 경우
                if (ally.Size == 2)
                {
                    // 이미 Receivers 리스트에 동일한 GameObject를 참조하는 BaseCharacter가 없는 경우에만 추가
                    if (!Receivers.Any(e => e.gameObject == ally.gameObject))
                    {
                        Receivers.Add(ally);
                    }
                }
                else
                {
                    // Size가 1인 Ally은 그냥 추가
                    Receivers.Add(ally);
                }
                Receivers.Add(allyFormation[i]);
            }
            else if (4 <= i && i < 8 && currentSelectedSkill.SkillRadius[i])
            {
                BaseCharacter enemy = enemyFormation[i - 4];
                if(enemy == null) continue;

                //적의 Size가 2인 경우
                if(enemy.Size == 2)
                {
                    // 이미 Receivers 리스트에 동일한 GameObject를 참조하는 BaseCharacter가 없는 경우에만 추가
                    if (!Receivers.Any(e => e.gameObject == enemy.gameObject))
                    {
                        Receivers.Add(enemy);
                    }
                }
                else
                {
                    // Size가 1인 적은 그냥 추가
                    Receivers.Add(enemy);
                }

            }
        }

        if (Caster != null && Receivers.Count > 0)
        {
            StartCoroutine(ExecuteSkill(Caster, Receivers));
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
            if (receiver.CheckDead())
            {
                if (receiver.IsAlly)
                {
                    for (int i = 0; i < allyFormation.Length; i++)
                    {
                        if (allyFormation[i] != null && allyFormation[i] == receiver)
                        {
                            allyFormation[i] = null; // 자기 자신을 null로 설정
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < enemyFormation.Length; i++)
                    {
                        if (enemyFormation[i] != null && enemyFormation[i] == receiver)
                        {
                            enemyFormation[i] = null;
                        }
                    }

                }
            };
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

    #endregion

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
        CheckBuffs(BuffTiming.RoundEnd);
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

    /// <summary>
    /// 적군이 모두 죽었는지 확인
    /// </summary>
    bool CheckVictory(IEnumerable<BaseCharacter> characters)
    {
        foreach (BaseCharacter character in characters)
        {
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
    bool CheckDefeat(IEnumerable<BaseCharacter> characters)
    {
        foreach (BaseCharacter character in characters)
        {
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
            BaseCharacter curchar = combatQueue.Dequeue();
            if(curchar.IsAlly == false)
            {
                curchar.Destroy();
                Destroy(curchar.gameObject);
            }
        }
        combatQueue.Clear();
        GameManager.GetInstance.SelectRoom();
    }

    /// <summary>
    /// 매개변수로 들어온 캐릭터가 현재 포메이션에서 어느 위치에 있는지
    /// </summary>
    /// <param name="_character"></param>
    /// <returns></returns>
    public int GetCharacterIndex(BaseCharacter _character)
    {
        int index = 0;
        BaseCharacter[] formation;

        if (_character.IsAlly)
            formation = allyFormation;
        else
            formation = enemyFormation;
        
        for(int i = 0; i < 4; i++)
        {
            if (formation[i] == _character)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    /// <summary>
    /// index 위치에 캐릭터가 있는지
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsCharacterThere(int index)
    {
        if(index < 4)
        {
            if (allyFormation[index] != null)
            {
                return true;
            }
        }
        else if(index < 8)
        {
            if (enemyFormation[index - 4] != null)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 캐릭터의 위치를 이동시키는 함수
    /// </summary>
    /// <param name="move">얼마나 이동할 것인지, 음수면 뒤로 이동, 양수면 앞으로 이동</param>
    public void MoveCharacter(BaseCharacter character, int move)
    {
        int from = GetCharacterIndex(character);
        int to = Mathf.Clamp(from - move, 0, 3);    // 이동하려는 위치

        // 이동한 곳에 캐릭터가 있으면 두 캐릭터의 RowOrder 값을 교환
        // 바뀐 RowOrder 값은 턴이 끝날 때 
        if(character.IsAlly)
        {
            if(IsCharacterThere(to))
            {
                int temp = character.RowOrder;
                character.RowOrder = allyFormation[to].RowOrder;
                allyFormation[to].RowOrder = temp;
            }
        }
        else
        {
            if(IsCharacterThere(to + 4))
            {
                int temp = character.RowOrder;
                character.RowOrder = enemyFormation[to + 4].RowOrder;
                enemyFormation[to + 4].RowOrder = temp;
            }
        }
    }
    
    #region Getter Setter

    public BaseCharacter[] AllyFormation
    {
        get { return allyFormation; }
        private set { allyFormation = value; } 
    }

    public BaseCharacter[] EnemyFormation
    {
        get { return enemyFormation; }
        private set { enemyFormation = value; }
    }
    public BaseSkill CurrentSelectedSkill => currentSelectedSkill;
    #endregion
}
