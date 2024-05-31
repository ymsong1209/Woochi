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
    public void SetText(float value, float changeValue, bool isPercent = false)
    {
        #region ���� ��ġ�� ���� �ؽ�Ʈ ���� ����
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
