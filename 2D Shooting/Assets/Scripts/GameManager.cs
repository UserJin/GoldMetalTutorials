using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public int stage;
    public Animator stageAnim;
    public Animator clearAnim;
    public Animator fadeAnim;

    public Transform playerPos;

    public string[] enemyObjs;
    public Transform[] spawnPoints;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public TextMeshProUGUI scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    public ObjectManager objectManager;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    private void Awake()
    {
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL", "EnemyB" };
        spawnList = new List<Spawn>();
        StageStart();
    }

    public void StageStart()
    {
        // �������� UI �ҷ�����
        stageAnim.SetTrigger("On");
        stageAnim.GetComponent<TextMeshProUGUI>().text = "Stage" + stage +"\nStart";
        clearAnim.GetComponent<TextMeshProUGUI>().text = "Stage" + stage + "\nClear!!";

        // �� ���� ���� �б�
        ReadSpawnFile();

        // ���̵� ��
        fadeAnim.SetTrigger("In");
    }

    public void StageEnd()
    {
        // Ŭ���� UI �ҷ�����
        clearAnim.SetTrigger("On");

        // ���̵� �ƿ�
        fadeAnim.SetTrigger("Out");

        // �÷��̾� ��ġ �ʱ�ȭ
        player.transform.position = playerPos.position;

        // �������� �ѹ� ����
        stage++;
        if (stage > 2)
            Invoke("GameOver", 3);
        else
            Invoke("StageStart", 3);
    }

    void ReadSpawnFile()
    {
        // ���� �ʱ�ȭ
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // ������ ���� �б�
        TextAsset textFile = Resources.Load("Stage " + stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while(stringReader != null)
        {
            string line = stringReader.ReadLine();
            //Debug.Log(line);
            if (line == null) // �о�� �����Ͱ� ���ٸ� Ż��
                break;

            // ������ ������ ����
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        // �ؽ�Ʈ ���� �ݱ�
        stringReader.Close();

        // ù��° �� ���� ������
        nextSpawnDelay = spawnList[0].delay;
    }

    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay >= nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            curSpawnDelay = 0;
        }

        // UI ���ھ� ������Ʈ
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
        
    }

    void SpawnEnemy()
    {
        // ���� ������ ���
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type)
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }
        int enemyPoint = spawnList[spawnIndex].point;

        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = this.player;
        enemyLogic.objectManager = this.objectManager;
        enemyLogic.gameManager = this;

        if(enemyPoint == 5 || enemyPoint == 6) // Right Spawn
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed*(-1), -1);
        }
        else if (enemyPoint == 7 || enemyPoint == 8) // Left Spawn
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else // Middle Spawn
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed*(-1));
        }

        // ������ �ε��� ����
        spawnIndex++;
        if(spawnIndex == spawnList.Count) // ������ ����Ʈ�� ����� ��� ���� ������ �ڿ��� ���� ���� �÷��׸� Ȱ��ȭ
        {
            spawnEnd = true;
            return;
        }

        // ������ ������ ����
        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2.0f);
    }
    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        player.GetComponent<Player>().isHit = false;
    }

    public void UpdateLifeIcon(int life) // �÷��̾� ü�� ������ ����
    {
        for(int index = 0; index < 3 ; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom) // �÷��̾� ��ź ������ ����
    {
        for (int index = 0; index < 3; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void GameOver() // ���ӿ��� UI Ȱ��ȭ
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry() // ���� ����� �Լ�
    {
        SceneManager.LoadScene(0);
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        GameObject explosion = objectManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }
}
