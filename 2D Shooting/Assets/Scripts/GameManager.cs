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
        // 스테이지 UI 불러오기
        stageAnim.SetTrigger("On");
        stageAnim.GetComponent<TextMeshProUGUI>().text = "Stage" + stage +"\nStart";
        clearAnim.GetComponent<TextMeshProUGUI>().text = "Stage" + stage + "\nClear!!";

        // 적 스폰 파일 읽기
        ReadSpawnFile();

        // 페이드 인
        fadeAnim.SetTrigger("In");
    }

    public void StageEnd()
    {
        // 클리어 UI 불러오기
        clearAnim.SetTrigger("On");

        // 페이드 아웃
        fadeAnim.SetTrigger("Out");

        // 플레이어 위치 초기화
        player.transform.position = playerPos.position;

        // 스테이지 넘버 증가
        stage++;
        if (stage > 2)
            Invoke("GameOver", 3);
        else
            Invoke("StageStart", 3);
    }

    void ReadSpawnFile()
    {
        // 변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // 리스폰 파일 읽기
        TextAsset textFile = Resources.Load("Stage " + stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while(stringReader != null)
        {
            string line = stringReader.ReadLine();
            //Debug.Log(line);
            if (line == null) // 읽어올 데이터가 없다면 탈출
                break;

            // 리스폰 데이터 생성
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        // 텍스트 파일 닫기
        stringReader.Close();

        // 첫번째 적 생성 딜레이
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

        // UI 스코어 업데이트
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
        
    }

    void SpawnEnemy()
    {
        // 스폰 데이터 사용
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

        // 리스폰 인덱스 증가
        spawnIndex++;
        if(spawnIndex == spawnList.Count) // 리스폰 리스트에 저장된 모든 적을 스폰한 뒤에는 스폰 종료 플래그를 활성화
        {
            spawnEnd = true;
            return;
        }

        // 리스폰 딜레이 갱신
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

    public void UpdateLifeIcon(int life) // 플레이어 체력 아이콘 갱신
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

    public void UpdateBoomIcon(int boom) // 플레이어 폭탄 아이콘 갱신
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

    public void GameOver() // 게임오버 UI 활성화
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry() // 게임 재시작 함수
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
