using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitArr;

    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    private void GenerateData()
    {
        // Talk Data
        talkData.Add(1000, new string[] { "안녕?:0", "이 곳에 처음 왔구나?:1" });
        talkData.Add(2000, new string[] { "오, 처음보는 얼굴이네?:1", "이 곳이 처음이라면 내 옆의 호수를 둘러봐.:0", "숨겨진 비밀은 없지만 정말 아름답지.:2" });
        talkData.Add(100, new string[] { "평범한 나무상자다." });
        talkData.Add(200, new string[] { "누군가 사용한 흔적이 남은 책상이다." });

        // Quest Talk
        talkData.Add(10 + 1000, new string[] {"어서 와.:0",
                                              "이 마을에 놀라운 전설이 있다는데:1",
                                              "오른쪽 호수 쪽에 루도가 알려줄꺼야.:0"});
        talkData.Add(11 + 2000, new string[] {"여어.:1",
                                              "이 호수의 전설을 들으러 온거야?:0",
                                              "맨입으로는 곤란한데...:1",
                                              "내 집 근처에 떨어진 동전을 주워주면 알려줄게.:2"});

        talkData.Add(20 + 1000, new string[] {"루도의 동전?:1",
                                              "돈을 흘리고 다니면 못쓰지!:3",
                                              "나중에 루도에게 한마디 해야겠어.:3"});
        talkData.Add(20 + 2000, new string[] { "찾으면 꼭 좀 가져다 줘.:1", });
        talkData.Add(20 + 5000, new string[] { "근처에서 동전을 찾았다.", });

        talkData.Add(21 + 2000, new string[] { "엇, 찾아줘서 고마워.:2", });

        // PortaitData
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(1000 + 2, portraitArr[2]);
        portraitData.Add(1000 + 3, portraitArr[3]);
        portraitData.Add(2000 + 0, portraitArr[4]);
        portraitData.Add(2000 + 1, portraitArr[5]);
        portraitData.Add(2000 + 2, portraitArr[6]);
        portraitData.Add(2000 + 3, portraitArr[7]);

    }

    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id))
        {
            if(!talkData.ContainsKey(id - id%10))
            {
                return GetTalk(id - id % 100, talkIndex); // 기본 대사
            }
            else
            {
                return GetTalk(id - id % 10, talkIndex); // 퀘스트 첫 대사
            }
        }

        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }
}
