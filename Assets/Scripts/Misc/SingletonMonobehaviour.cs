using UnityEngine;

public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;

    public static T GetInstance
    {
        get
        {
            return m_Instance;
        }

    }

    protected virtual void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}