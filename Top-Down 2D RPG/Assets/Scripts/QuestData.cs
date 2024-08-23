using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    public string _questName; //퀘스트 이름
    public int[] _npcID; //해당 퀘스트와 연관된 npc id 목록

    public QuestData(string questName, int[] npcId)
    {
        _questName = questName;
        _npcID = npcId;
    }
}
