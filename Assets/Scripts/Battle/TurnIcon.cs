using UnityEngine;
using UnityEngine.UI;

public class TurnIcon : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject waiting;
    [SerializeField] private GameObject stun;

    private void Start()
    {
        stun.SetActive(false);
    }

    public void SetIcon(BaseCharacter character, bool isTurn = false)
    {
        icon.sprite = character.Icon;
        waiting.SetActive(!isTurn);
    }

    public void SetEmpty(Sprite sprite)
    {
        icon.sprite = sprite;
        waiting.SetActive(false);
    }
}
