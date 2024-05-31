using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 아군 스탯을 표시하는 텍스트를 담는 클래스
/// </summary>
public class AllyStat : MonoBehaviour
{
    private TextMeshProUGUI labelTxt;
    private TextMeshProUGUI valueTxt;

    void Start()
    {
        labelTxt = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        valueTxt = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 스탯을 UI에 반영한다
    /// </summary>
    /// <param name="value"></param>
    /// <param name="isPercent">퍼센트 표시 스탯이면 true로 설정</param>
    public void SetText(float value, float changeValue, bool isPercent = false)
    {
        #region 증감 수치에 따른 텍스트 색상 변경
        if (changeValue > 0)
        {
            labelTxt.color = Color.yellow;
            valueTxt.color = Color.yellow;
        }
        else if(changeValue < 0)
        {
            labelTxt.color = Color.red;
            valueTxt.color = Color.red;
        }
        else
        {
            labelTxt.color = Color.black;
            valueTxt.color = Color.black;
        }
        #endregion

        if (isPercent)
        {
            valueTxt.text = value.ToString() + "%";
            return;
        }

        valueTxt.text = value.ToString();
    }

    public void SetDamageText(float minStat, float maxStat)
    {
        valueTxt.text = $"{minStat}-{maxStat}";
    }
}
