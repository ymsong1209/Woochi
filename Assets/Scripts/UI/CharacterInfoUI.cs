using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ĳ���� ������ ���õ� UI���� �����ϴ� Ŭ����
/// ĳ���� �ʻ�ȭ, �̸�, ���� ���� ǥ��
/// </summary>
public class CharacterInfoUI : MonoBehaviour
{
    [SerializeField] private Image characterPortrait;
    [SerializeField] private TextMeshProUGUI characterName;

    void Start()
    {
        #region �̺�Ʈ ���
        BattleManager.GetInstance.OnCharacterTurnStart += ShowCharacterUI;

        #endregion
    }

    public void ShowCharacterUI(BaseCharacter _character)
    {
        ShowCharacterInfo(_character);
        ShowCharacterStat(_character);
    }

    /// <summary>
    /// ĳ���� ������ ���� UI�� ǥ��
    /// </summary>
    /// <param name="_character"></param>
    private void ShowCharacterInfo(BaseCharacter _character)
    {
        // ĳ���� �ʻ�ȭ ��������Ʈ�� ���� ��� �����ؾ� �� �� �����̶� �ּ� ó��
        // ��� ����
        // 1. CharacterStatSO�� ĳ���� ���õ� �͵��� �����ϴ� ������ ���ٸ� ���⿡ �����ϰ� BaseCharacter���� ������ �� �ְ� �ϴ°� ���
        // 2. �ƴϸ� ���� ���� ������ ���� �ű⿡ �����ϰ� ������ �� �ְ� �ϴ� ���
        // characterPortrait.sprite = _character.CharacterPortrait;

        characterName.text = _character.name;
    }

    private void ShowCharacterStat(BaseCharacter _character)
    {
        // TODO : ĳ������ ������ ǥ���ϴ� UI�� ����
    }
}
