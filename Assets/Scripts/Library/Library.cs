using OneLine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Library_", menuName = "Scriptable Objects/Library")]
public class Library : ScriptableObject
{
    // Warning!. ���� �̸� �����ϸ� ������ �� ����

    #region Character Prefab
    [OneLineWithHeader, SerializeField] private List<Entry<GameObject>> characters;
    public GameObject GetCharacter(int id)
    {
        if (id < 0)
        {
            Debug.Log("Library: ID out of range");
            return null;
        }

        return characters.FirstOrDefault(entry => entry.ID == id).value;
    }

    public List<GameObject> GetCharacterList(int[] IDs)
    {
        List<GameObject> list = new List<GameObject>();

        foreach (int id in IDs)
        {
            GameObject entry = GetCharacter(id);

            if (entry != null)
                list.Add(entry);
        }

        return list;
    }
    #endregion

    #region Abnormal
    [OneLineWithHeader, SerializeField] private List<Entry<Abnormal>> abnormals;

    public Abnormal GetAbnormal(int id)
    {
        if (id < 0)
        {
            Debug.Log("Library: ID out of range");
            return null;
        }

        return abnormals.FirstOrDefault(entry => entry.ID == id).value;
    }
    #endregion

    #region Reward
    [SerializeField] private RewardList[] rewardLists;

    public Reward GetReward(RareType rarity)
    {
        var list = rewardLists[(int)rarity].rewards;

        return list.Random();
    }

    #endregion
    
    #region Strange
    
    [OneLineWithHeader, SerializeField] private List<Entry<GameObject>> stranges;

    public GameObject GetStrange(int id)
    {
        if (id < 0)
        {
            Debug.Log("Library: ID out of range");
            return null;
        }

        return stranges.FirstOrDefault(entry => entry.ID == id)?.value;
    }
    #endregion
    
    

    #region Charm
    [OneLineWithHeader, SerializeField] private List<Entry<BaseCharm>> charms;

    public BaseCharm GetCharm(int id)
    {
        if (id < 0)
        {
            Debug.Log("Library: ID out of range");
            return null;
        }

        return charms.FirstOrDefault(entry => entry.ID == id).value;
    }

    public int CharmCount => charms.Count;
    #endregion

    /// <summary>
    /// ���� �������� ��Ʈ�� �����͸� �����ͼ� ��ũ���ͺ� �ʱ�ȭ -> �� �ѹ���
    /// </summary>
    public void Initialize()
    {
        foreach (var character in characters)
        {
            if (character.value == null) continue;

            BaseCharacter baseCharacter = character.value.GetComponent<BaseCharacter>();
            baseCharacter.InitializeStatSO();
        }

        foreach (var abnormal in abnormals)
        {
            abnormal.value.Initialize();
        }
    }
}

[System.Serializable]
public class Entry<T>
{
    public int ID;
    public T value;
}

[System.Serializable]
public class RewardList
{
    public RareType rarity;
    public List<Reward> rewards;
}