using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterCollider : MonoBehaviour
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

    void Update()
    {
        // Check if canInteract is true and the left mouse button is clicked
        if (canInteract && Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭시 스킬 발동
            if (IsMouseOverCollider())
            {
                BattleManager.GetInstance.ExecuteSelectedSkill(owner);
            }
        }
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
}
