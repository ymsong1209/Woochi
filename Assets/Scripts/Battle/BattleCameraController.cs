using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BattleCameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    [Header("Target Group")]
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private List<CinemachineTargetGroup.Target> targets = new List<CinemachineTargetGroup.Target>();

    [Header("Post Process")]
    [SerializeField] private PostProcessVolume postProcessVolume;

    private void Start()
    {
        BattleManager.GetInstance.OnFocusStart += FocusIn;
        BattleManager.GetInstance.OnFocusEnd += FocusOut;
        BattleManager.GetInstance.OnFocusEnter += FocusEnter;
    }

    public void FocusIn()
    {
        mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("HUD"));
        postProcessVolume.enabled = true;

        targetGroup.m_Targets = targets.ToArray();
    }

    public void FocusOut()
    {
        mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("HUD");
        postProcessVolume.enabled = false;

        targets.Clear();
    }

    private void FocusEnter(BaseCharacter _character)
    {
        CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target
        {
            target = _character.transform,
            radius = 3,
            weight = 1
        };

        targets.Add(target);
    }
}
