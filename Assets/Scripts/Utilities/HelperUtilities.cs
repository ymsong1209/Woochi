using System;
using UnityEngine;
using System.Collections;
using System.Reflection;

public static class HelperUtilities
{
    ///<summary>
    ///String이 비어있는지 확인
    ///</summary>
    public static bool ValidateCheckEmptyString(UnityEngine.Object thisObject, string fieldName, string stringToCheck)
    {
        if (stringToCheck == "")
        {
            Debug.Log(fieldName + " is empty and must contain a value in object " + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    /// <summary>
    /// null value debug check
    /// </summary>
    public static bool ValidateCheckNullValue(UnityEngine.Object thisObject, string fieldName, UnityEngine.Object objectToCheck)
    {
        if (objectToCheck == null)
        {
            Debug.Log(fieldName + " is null and must contain a value in object " + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    /// <summary>
    /// list empty or contains null value check - returns true if there is an error
    /// </summary>
    public static bool ValidateCheckEnumerableValues(UnityEngine.Object thisObject, string fieldName, IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        if (enumerableObjectToCheck == null)
        {
            Debug.Log(fieldName + " is null in object " + thisObject.name.ToString());
            return true;
        }

        foreach (var item in enumerableObjectToCheck)
        {

            if (item == null)
            {
                Debug.Log(fieldName + " has null values in object " + thisObject.name.ToString());
                error = true;
            }
            else
            {
                count++;
            }
        }

        if (count == 0)
        {
            Debug.Log(fieldName + " has no values in object " + thisObject.name.ToString());
            error = true;
        }

        return error;
    }


    /// <summary>
    /// 입력으로 들어온 수가 100 이하인지 확인
    /// </summary>
    public static bool ValidateCheckOverHundred<T>(UnityEngine.Object thisObject, string fieldName, T value) where T : IComparable
    {
        T hundred = (T)Convert.ChangeType(100, typeof(T));

        if (value.CompareTo(hundred) > 100)
        {
            Debug.Log(fieldName + " is over 100 and must be below 100 in object " + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    /// <summary>
    /// 입력으로 들어온 수가 0 미만인지 확인
    /// </summary>
    public static bool ValidateCheckUnderZero<T>(UnityEngine.Object thisObject, string fieldName, T value) where T : IComparable
    {
        T zero = (T)Convert.ChangeType(0, typeof(T));

        if (value.CompareTo(zero) < 0)
        {
            Debug.Log(fieldName + " is under 0 and must be above 0 in object " + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    /// <summary>
    /// 입력으로 들어온 수가 1 이상인지 확인
    /// </summary>
    public static bool ValidateCheckOverOne<T>(UnityEngine.Object thisObject, string fieldName, T value) where T : IComparable
    {
        T one = (T)Convert.ChangeType(1, typeof(T));

        if (value.CompareTo(1) <= 0)
        {
            Debug.Log(fieldName + " is not over 1 in object " + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    /// <summary>
    /// 입력으로 들어온 수가 0~100 사이인지 확인
    /// </summary>
    public static bool ValidateRange0To100<T>(UnityEngine.Object thisObject, string fieldName, T value) where T : IComparable
    {
        T zero = (T)Convert.ChangeType(0, typeof(T));
        T hundred = (T)Convert.ChangeType(100, typeof(T));

        if (value.CompareTo(zero) < 0 || value.CompareTo(hundred) > 0)
        {
            Debug.Log(fieldName + " is not between 0 or 100 in object " + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    /// <summary>
    /// 씬 이동 : 로딩 씬 먼저 이동 후 다른 씬으로 이동한다
    /// </summary>
    /// <param name="sceneType">Build Setting에 등록되어 있는 씬 순서대로 동작</param>
    public static void MoveScene(SceneType sceneType)
    {
        LoadingScene.LoadScene(sceneType);
    }

    public static bool Buy(int price)
    {
        if (DataCloud.playerData.gold >= price)
        {
            DataCloud.playerData.gold -= price;
            EventManager.GetInstance.onChangedGold?.Invoke();   
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void AddGold(int gold)
    {
        DataCloud.playerData.gold += gold;
        EventManager.GetInstance.onChangedGold?.Invoke();
    }

    /// <summary>
    /// 부적을 얻을 수 있는지
    /// 얻을 수 있다면 랜덤으로 하나 주고 못 얻으면 false 반환
    /// </summary>
    public static bool CanGetCharm(out string text)
    {
        var charmList = DataCloud.playerData.battleData.charms;
        if(charmList.Count >= 5)
        {
            text = "부적은 5개까지만 가질 수 있습니다";
            return false;
        }

        int charmCount = GameManager.GetInstance.Library.CharmCount;
        int randomCharmID = UnityEngine.Random.Range(0, charmCount);
        charmList.Add(randomCharmID);
        text = $"{GameManager.GetInstance.Library.GetCharm(randomCharmID).CharmName}을 얻었습니다";
        return true;
    }

    public static string GetDisplayName(this Enum enumValue)
    {
        FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());
        
        DisplayAttribute attribute = field.GetCustomAttribute<DisplayAttribute>();
        
        return attribute != null ? attribute.DisplayName : enumValue.ToString();
    }
}
