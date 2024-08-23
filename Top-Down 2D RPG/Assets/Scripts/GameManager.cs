using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;

    public Animator talkPanel;
    public TypeEffect talk; // Ÿ���� ����Ʈ
    public TextMeshProUGUI questText; // ����Ʈ ����
    public Image portraitImg;
    public Sprite previousPortrait; //���� �ʻ�ȭ ���
    public Animator portraitAnim;
    public GameObject scanObject;
    public GameObject menuSet;
    public GameObject player;


    public bool isAction;
    public int talkIndex;
    

    public void Start()
    {
        GameLoad();
        questText.text = questManager.CheckQuest();
    }

    void Update()
    {
        // sub menu
        if(Input.GetButtonDown("Cancel"))
        {
            if (menuSet.activeSelf)
            {
                menuSet.SetActive(false);
            }
            else
            {
                menuSet.SetActive(true);
            }
        }
    }

    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjectData objectData = scanObject.GetComponent<ObjectData>();
        Talk(objectData._id, objectData._isNpc);

        // ��ȭâ ���
        talkPanel.SetBool("isShow", isAction);
    }


    void Talk(int id, bool isNpc)
    {
        // ��ȭ �ҷ�����
        int questTalkIndex;
        string talkData = "";

        if (talk.isAniamtion)
        {
            talk.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }
        
        // ��ȭ ����
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            questText.text = questManager.CheckQuest(id);
            return;
        }
        
        // ��ȭ ����
        if (isNpc)
        {
            talk.SetMsg(talkData.Split(':')[0]);

            // show portrait
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);
            // Animation Portrait
            if(previousPortrait != portraitImg.sprite)
            {
                portraitAnim.SetTrigger("doEffect");
                previousPortrait = portraitImg.sprite;
            }
        }
        else
        {
            talk.SetMsg(talkData);

            // Portrait ����
            portraitImg.color = new Color(1, 1, 1, 0);
        }

        isAction = true;
        talkIndex++;
    }

    public void GameSave()
    {
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("QuestId", questManager._questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager._questActionIdex);
        Debug.Log(questManager._questId);
        Debug.Log(questManager._questActionIdex);
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }

    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
            return;

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector3(x, y, 0);
        questManager._questId = questId;
        questManager._questActionIdex = questActionIndex;
        questManager.ControlObject();
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
