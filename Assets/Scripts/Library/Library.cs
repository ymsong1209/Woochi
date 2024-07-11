using OneLine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Library_", menuName = "Scriptable Objects/Library")]
public class Library : ScriptableObject
{
    [OneLineWithHeader]
    [SerializeField] private List<Entry> entryList;

    public GameObject Get(int id)
    {
        if(id < 0)
        {
            Debug.Log("Library: ID out of range");
            return null;
        }

        return entryList.FirstOrDefault(entry => entry.ID == id).prefab;
    }

    public List<GameObject> Get(int[] IDs)
    {
        List<GameObject> list = new List<GameObject>();

        foreach(int id in IDs)
        {
            GameObject entry = Get(id);

            if(entry != null)
                list.Add(entry);
        }

        return list;
    }
}

[System.Serializable]
public class Entry
{
    public int ID;
    public GameObject prefab;
}