using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameMgr : MonoBehaviour
{

    public GameObject player;

    public GameObject[] enemies;
    public Transform[] spawnPlaces;

    public float maxSpawnDelay = 1;
    public float curSpawnDelay = 0;

    float curScore = 0;
    Transform scoreTxt;


    // Start is called before the first frame update
    void Start()
    {
        scoreTxt = GameObject.Find("Canvas").transform.Find("score");
    }

    // Update is called once per frame
    void Update()
    {
        curSpawnDelay += Time.deltaTime;
        if (curSpawnDelay > maxSpawnDelay)
        {
            curSpawnDelay = 0;

            int randEnemy = Random.Range(0, 1);
            int randomPoint = Random.Range(0, 5);

            GameObject enemy = Instantiate(enemies[randEnemy],
                spawnPlaces[randomPoint].position,
                spawnPlaces[randomPoint].rotation);

            Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
            EnemyCtrl enemyScript = enemy.GetComponent<EnemyCtrl>();

            enemyScript.player = player;
            enemyScript.manager = this.gameObject;

            if(randomPoint==3) // left
                rigid.velocity = new Vector2(enemyScript.speed * 0.5f, enemyScript.speed* - 1);
            else if(randomPoint==4)// right
                rigid.velocity = new Vector2(enemyScript.speed * -0.5f, enemyScript.speed * -1);
            else 
                rigid.velocity = new Vector2(0, enemyScript.speed * (-1));
        }
    }

    public void incScore(float amount)
    {
        curScore += amount;
        scoreTxt.GetComponent<Text>().text = "score: " + curScore.ToString();
    }

    public void restorePlayer()
    {
        Invoke("respawnPlayerLater", 2f);
    }

    public void respawnPlayerLater()
    {
        player.transform.position = Vector3.down * 0.6f;
        player.SetActive(true);
        player.transform.GetComponent<playerCtrl>().Initialize(); // 초기화 
    }
}
