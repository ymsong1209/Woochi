using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �Ʊ� ������ ǥ���ϴ� �ؽ�Ʈ�� ��� Ŭ����
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
    /// ������ UI�� �ݿ��Ѵ�
    /// </summary>
    /// <param name="value"></param>
    /// <param name="isPercent">�ۼ�Ʈ ǥ�� �����̸� true�� ����</param>
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
