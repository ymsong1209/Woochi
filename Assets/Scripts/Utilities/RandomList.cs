using System.Collections.Generic;
using UnityEngine;
using OneLine;

[System.Serializable]
public class RandomList<T>
{
    [OneLineWithHeader]
    public List<RandomObject<T>> list;

    /// <summary>
    /// 정해진 확률대로 얻어오는 함수
    /// </summary>
    public T Get()
    {
        float randomValue = Random.Range(0f, 1f);
        float cumulative = 0f;

        foreach (var randomObject in list)
        {
            cumulative += randomObject.probability;

            if (randomValue <= cumulative)
            {
                return randomObject.value;
            }
        }

        return default;
    }
}

[System.Serializable]
public class RandomObject<T>
{
    public T value;
    public float probability;
}
