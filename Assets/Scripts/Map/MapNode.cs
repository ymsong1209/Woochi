using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

public class MapNode : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public Image image;                                                                                                                                                                                                                                                                        
    public Image circleImage;
    public Image visitedCircleImage;

    public Node Node { get; private set; }
    public NodeBlueprint NodeBlueprint { get; private set; }

    private float initScale;
    private const float HoverScaleFactor = 1.2f;
    private float mouseDownTime;

    private const float MaxClickDuration = 0.5f;

    public void SetUp(Node _node, NodeBlueprint _blueprint)
    {
        Node = _node;
        NodeBlueprint = _blueprint;

        image.sprite = _blueprint.sprite;

        if (_node.nodeType == NodeType.Boss) transform.localScale *= 1.5f;
        initScale = image.transform.localScale.x;

        circleImage.color = MapManager.GetInstance.view.visitedColor;
        circleImage.gameObject.SetActive(false);

        SetState(NodeState.Locked);
    }

    public void SetState(NodeState _state)
    {
        if (circleImage != null) circleImage.gameObject.SetActive(false);

        switch (_state)
        {
            case NodeState.Locked:
                image.DOKill();
                image.color = MapManager.GetInstance.view.lockedColor;
                break;
            case NodeState.Visited:
                image.DOKill();
                image.color = MapManager.GetInstance.view.visitedColor;

                if (circleImage != null) circleImage.gameObject.SetActive(true);
                break;
            case NodeState.Attainable:
                image.color = MapManager.GetInstance.view.lockedColor;
                image.DOKill();
                image.DOColor(Color.white, 0.5f).SetLoops(-1, LoopType.Yoyo);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_state), _state, null);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.transform.DOKill();
        image.transform.DOScale(initScale * HoverScaleFactor, 0.3f);
        
        GameManager.GetInstance.soundManager.PlaySFX("Map_Mouse");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        image.transform.DOKill();
        image.transform.DOScale(initScale, 0.3f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mouseDownTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(Time.time - mouseDownTime < MaxClickDuration)
        {
            MapPlayerTracker.Instance.SelectNode(this);
        }
    }

    public void ShowVisitAnimation()
    {
        const float fillDuration = 0.3f;
        visitedCircleImage.fillAmount = 0;

        DOTween.To(() => visitedCircleImage.fillAmount, x => visitedCircleImage.fillAmount = x, 1, fillDuration);
    }
}
