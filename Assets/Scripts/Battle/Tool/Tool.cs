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
        Debug.Log($"{toolName}을 {price}만큼 지불해 사용합니다");
        DataCloud.playerData.gold -= price;
    }

    public void CheckUsable()
    {
        if(DataCloud.playerData.gold < price)
        {
            Debug.Log("골드가 부족합니다");
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
