using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int _questId; //����Ʈ ��ȣ
    public int _questActionIdex; //����Ʈ ���൵
    public GameObject[] _questObjects; //����Ʈ ���� ������Ʈ
    Dictionary<int, QuestData> _questList; //(����Ʈ ��ȣ, ����Ʈ ������)

    // Start is called before the first frame update
    void Awake()
    {
        _questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    // ����Ʈ ������ ����
    // ���� �ý��۰� �����Ͽ� txt���Ͽ��� �о���� ������� ��������
    private void GenerateData()
    {
        _questList.Add(10, new QuestData("���� ������ ��ȭ�ϱ�"
                                        , new int[] {1000, 2000}));
        _questList.Add(20, new QuestData("�絵�� ���� ã���ֱ�"
                                        , new int[] { 5000, 2000 }));
        _questList.Add(30, new QuestData("����Ʈ �� Ŭ����!"
                                        , new int[] { 0 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        return _questId + _questActionIdex;
    }

    public string CheckQuest(int id)
    {
        if(id == _questList[_questId]._npcID[_questActionIdex])
            _questActionIdex++;

        // ����Ʈ ������Ʈ ����
        ControlObject();

        // ����Ʈ�� ����Ǹ� ���� ����Ʈ�� ����
        if (_questActionIdex == _questList[_questId]._npcID.Length)
        {
            NextQuest();
        }

        return _questList[_questId]._questName;
    }
    public string CheckQuest()
    {
        return _questList[_questId]._questName;
    }

    void NextQuest()
    {
        _questId += 10;
        _questActionIdex = 0;
    }

    public void ControlObject()
    {
        switch(_questId)
        {
            case 10:
                if(_questActionIdex == 2)
                {
                    _questObjects[0].SetActive(true);
                }
                break;
            case 20:
                if (_questActionIdex == 0)
                    _questObjects[0].SetActive(true);
                if(_questActionIdex == 1)
                {
                    _questObjects[0].SetActive(false);
                }
                break;
        }
    }
}
