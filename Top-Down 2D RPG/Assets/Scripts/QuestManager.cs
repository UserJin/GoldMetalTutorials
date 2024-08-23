using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int _questId; //퀘스트 번호
    public int _questActionIdex; //퀘스트 진행도
    public GameObject[] _questObjects; //퀘스트 연관 오브젝트
    Dictionary<int, QuestData> _questList; //(퀘스트 번호, 퀘스트 데이터)

    // Start is called before the first frame update
    void Awake()
    {
        _questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    // 퀘스트 정보를 생성
    // 파일 시스템과 연결하여 txt파일에서 읽어오는 방식으로 개선가능
    private void GenerateData()
    {
        _questList.Add(10, new QuestData("마을 사람들과 대화하기"
                                        , new int[] {1000, 2000}));
        _questList.Add(20, new QuestData("루도의 동전 찾아주기"
                                        , new int[] { 5000, 2000 }));
        _questList.Add(30, new QuestData("퀘스트 올 클리어!"
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

        // 퀘스트 오브젝트 조작
        ControlObject();

        // 퀘스트가 종료되면 다음 퀘스트로 변경
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
