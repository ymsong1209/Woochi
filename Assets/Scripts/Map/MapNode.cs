using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public enum NodeState
{
    Locked,
    Visited,
    Attainable
}

public class MapNode : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public Image icon;
    public Image circle;
    public Image Mark;

    public Node Node { get; private set; }
    public NodeBlueprint NodeBlueprint { get; private set; }

    private float initScale;
    private const float hoverScale = 1.2f;
    private float mouseDownTime;

    private const float mouseDownTimeThreshold = 0.5f;

    public void Set(Node _node, NodeBlueprint _blueprint)
    {
        Node = _node;
        NodeBlueprint = _blueprint;

        icon.sprite = _blueprint.icon;

        if (_node.type == NodeType.Boss) transform.localScale *= 1.5f;
        initScale = icon.transform.localScale.x;

        circle.color = Color.white;
        circle.gameObject.SetActive(false);
    }

    public void SetState(NodeState _state)
    {
        switch (_state)
        {
            case NodeState.Locked:
                icon.DOKill();
                icon.color = Color.gray;
                break;
            case NodeState.Visited:
                icon.DOKill();
                icon.color = Color.white;
                break;
            case NodeState.Attainable:
                icon.color = Color.gray;
                icon.DOKill();
                icon.DOColor(Color.white, 0.5f).SetLoops(-1, LoopType.Yoyo);
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        icon.transform.DOKill();
        icon.transform.DOScale(initScale * hoverScale, 0.3f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        icon.transform.DOKill();
        icon.transform.DOScale(initScale, 0.3f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mouseDownTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(Time.time - mouseDownTime < mouseDownTimeThreshold)
        {
            // Select Node
        }
    }

    public void ShowVisitAnimation()
    {
        const float fillDuration = 0.3f;
        Mark.fillAmount = 0;

        DOTween.To(() => Mark.fillAmount, x => Mark.fillAmount = x, 1, fillDuration);
    }
}
