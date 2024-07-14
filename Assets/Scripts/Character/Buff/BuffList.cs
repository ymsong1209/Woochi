using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffList : MonoBehaviour
{
    [SerializeField] private BuffIcon[] buffIcons = new BuffIcon[(int)BuffEffect.END];
    
    public BaseBuff InstantiateBuffAtIcon(BaseCharacter opponent, BaseBuff buff)
    {
        // Find the bufflistcanvas GameObject under the opponent
        Transform buffList = opponent.transform.Find("BuffList");
        if (buffList == null)
        {
            Debug.LogError("buffList not found under opponent" + opponent.gameObject.name.ToString());
            return null;
        }
        
        // 모든 자손을 순회하여 알맞은 BuffIcon을 찾음
        BuffIcon targetBuffIcon = buffIcons[(int)buff.BuffEffect];
        //버프 아이콘 활성화
        if(targetBuffIcon && !targetBuffIcon.gameObject.activeSelf)
        {
            targetBuffIcon.gameObject.SetActive(true);
            targetBuffIcon.Activate();
        }
        BaseBuff instantiatedBuff = Instantiate(buff, targetBuffIcon.transform);
        return instantiatedBuff;
        
    }
}
