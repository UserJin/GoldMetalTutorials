using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    public string _questName; //����Ʈ �̸�
    public int[] _npcID; //�ش� ����Ʈ�� ������ npc id ���

    public QuestData(string questName, int[] npcId)
    {
        _questName = questName;
        _npcID = npcId;
    }
}
