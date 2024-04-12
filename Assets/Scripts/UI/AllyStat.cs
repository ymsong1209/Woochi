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
    public void SetText(float value, bool isPercent = false)
    {
        if (isPercent)
        {
            valueTxt.text = value.ToString("F1") + "%";
            return;
        }

        valueTxt.text = value.ToString();
    }

    public void SetText(float minStat, float maxStat)
    {
        valueTxt.text = $"{minStat}-{maxStat}";
    }
}
