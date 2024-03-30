using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 캐릭터 정보와 관련된 UI들을 관리하는 클래스
/// 캐릭터 초상화, 이름, 스탯 등을 표시
/// </summary>
public class CharacterInfoUI : MonoBehaviour
{
    [SerializeField] private Image characterPortrait;
    [SerializeField] private TextMeshProUGUI characterName;

    void Start()
    {
        #region 이벤트 등록
        BattleManager.GetInstance.OnCharacterTurnStart += ShowCharacterUI;

        #endregion
    }

    public void ShowCharacterUI(BaseCharacter _character)
    {
        ShowCharacterInfo(_character);
        ShowCharacterStat(_character);
    }

    /// <summary>
    /// 캐릭터 정보를 조작 UI에 표시
    /// </summary>
    /// <param name="_character"></param>
    private void ShowCharacterInfo(BaseCharacter _character)
    {
        // 캐릭터 초상화 스프라이트는 아직 어디에 저장해야 할 지 미정이라 주석 처리
        // 방법 생각
        // 1. CharacterStatSO를 캐릭터 관련된 것들을 저장하는 것으로 본다면 여기에 저장하고 BaseCharacter에서 가져올 수 있게 하는게 어떨까
        // 2. 아니면 따로 도감 식으로 만들어서 거기에 저장하고 가져올 수 있게 하는 방법
        // characterPortrait.sprite = _character.CharacterPortrait;

        characterName.text = _character.name;
    }

    private void ShowCharacterStat(BaseCharacter _character)
    {
        // TODO : 캐릭터의 스탯을 표시하는 UI를 구현
    }
}
