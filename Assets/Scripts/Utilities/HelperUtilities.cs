using System;
using UnityEngine;
using System.Collections;

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




}
