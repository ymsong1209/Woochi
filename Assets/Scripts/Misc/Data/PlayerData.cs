using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    // bool
    public bool isFirstPlay;    // ������ ���� ó�� ���� -> Ʃ�丮��
    public bool isProgressing;  // �������̴� ������ �ִ���

    // Ally
    public List<int> allies;    // �÷��̾�(��ġ)�� �����ϰ� �ִ� ��ȯ��(��ġ �ڱ��ڽŵ� ����)
    public int[] formation;     // ������ ������ �����̼�(-1�� ���ڸ�)

    // Map Data
    public Map currentMap;

    public PlayerData()
    {
        isFirstPlay = true;
        isProgressing = false;

        allies = new List<int>();
        formation = new int[] { 0, -1, -1, -1};
        currentMap = null;

    }

    public void ResetData()
    {
        isProgressing = false;
        
        allies = new List<int>();
        currentMap = null;
    }
}
