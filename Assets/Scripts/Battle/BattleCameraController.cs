using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BattleCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera   battleCam;
    [SerializeField] private CinemachineVirtualCamera   focusCam;
    [SerializeField] private CinemachineTargetGroup     targetGroup;
    
    public void SetBattleCamera(bool isFocus)
    {
        battleCam.Priority = isFocus ? 10 : 20;
        focusCam.Priority = isFocus ? 20 : 10;
    }    
    
    public void SetTargetGroup(List<BaseCharacter> targets)
    {
        targetGroup.m_Targets = new CinemachineTargetGroup.Target[targets.Count];

        for (int i = 0; i < targets.Count; i++)
        {
            targetGroup.m_Targets[i].target = targets[i].transform;
            targetGroup.m_Targets[i].weight = 1;
        }

        SetBattleCamera(true);
    }
}
