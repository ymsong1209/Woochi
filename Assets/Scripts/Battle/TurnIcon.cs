using UnityEngine;
using UnityEngine.UI;

public class TurnIcon : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetIcon(BaseCharacter character, bool isTurn = false)
    {
        // ���� ���� üũ

        if(isTurn)
        {
            icon.sprite = character.Icons[0];
        }
        else
        {
            icon.sprite = character.Icons[1];
        }
    }

    public void SetEmpty(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
