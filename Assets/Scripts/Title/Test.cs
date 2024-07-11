using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private TMP_InputField[] inputFields;
    [SerializeField] private Button confirmBtn;

    void Start()
    {
        gameObject.SetActive(false);
        confirmBtn.onClick.AddListener(Confirm);
    }

    private void Confirm()
    {
        int[] idList = new int[] { 0, -1, -1, -1 };

        int index = 0;

        foreach (var inputField in inputFields)
        {
            if (int.TryParse(inputField.text, out int id))
            {
                idList[index++] = id;
            }
        }

        DataCloud.playerData.formation = idList;

        gameObject.SetActive(false);
    }
}
