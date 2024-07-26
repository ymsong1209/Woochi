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
        btn.onClick.AddListener(Use);
    }

    public virtual void Use()
    {
        if (HelperUtilities.CanBuy(price))
        {
            Debug.Log($"{toolName}�� {price}��ŭ ������ ����մϴ�");
        }
        else
        {
            return;
        }
    }

    protected virtual string GetDescription()
    {
        return description;
    }
}
