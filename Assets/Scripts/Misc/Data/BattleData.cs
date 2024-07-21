using System.Collections.Generic;

[System.Serializable]
public class BattleData
{
    public List<int> allies;    // �÷��̾�(��ġ)�� �����ϰ� �ִ� ��ȯ��(��ġ �ڱ��ڽŵ� ����)
    public int[] formation;     // ������ ������ �����̼�(-1�� ���ڸ�)

    public BattleData()
    {
        allies = new List<int>() { 0, 1, 2 };
        formation = new int[] { 0, -1, -1, -1};
    }
}
