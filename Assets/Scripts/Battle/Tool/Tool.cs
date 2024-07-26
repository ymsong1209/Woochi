using UnityEngine;
using UnityEngine.UI;

public class Tool : MonoBehaviour
{
    private Button btn;

    [SerializeField] protected string toolName;
    [SerializeField] protected string description;
    [SerializeField] protected int price;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CheckUsable);
    }

    public virtual void Use()
    {
        Debug.Log($"{toolName}�� {price}��ŭ ������ ����մϴ�");
        DataCloud.playerData.gold -= price;
    }

    public void CheckUsable()
    {
        if(DataCloud.playerData.gold < price)
        {
            Debug.Log("��尡 �����մϴ�");
        }
        else
        {
            Use();
        }
    }

    protected virtual string GetDescription()
    {
        return description;
    }
}
