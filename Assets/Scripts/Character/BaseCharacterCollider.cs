using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseCharacterCollider : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BaseCharacter owner;
    [SerializeField] private Collider2D collider;
    private bool canInteract = false;

    void Start()
    {
        canInteract = false;
        owner = GetComponent<BaseCharacter>();
        collider = GetComponent<Collider2D>();
    }

    private bool IsMouseOverCollider()
    {
        // Convert the mouse position to world coordinates
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Cast a 2D Raycast from the mouse position and store the result in a RaycastHit2D
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        // Check if the Raycast hit a Collider2D
        if (hit.collider)
        {
            // Check if the hit Collider2D is the same as the Collider2D of this GameObject
            if (hit.collider == collider)
            {
                owner.OnSelected();
                return true;
            }
        }

        return false;
    }
    
    public bool CanInteract
    {
        get => canInteract;
        set => canInteract = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canInteract)
        {
            if (IsMouseOverCollider())
            {
                BattleManager.GetInstance.ExecuteSelectedSkill(owner);
            }
        }
    }
}
