using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public float speed = 1;
    Rigidbody2D rigid;

    public GameObject player;

    public GameObject bulletA;

    public float maxShootingDelay = 1.0f;
    public float curShootingDelay = 0.0f;

    public float health = 100;
    SpriteRenderer spriteRender;

    public GameObject manager;

    public GameObject itemToDrop; // drop item 

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        curShootingDelay += Time.deltaTime;
        if (curShootingDelay < maxShootingDelay)
            return;

        curShootingDelay = 0;

        GameObject bullet01 = Instantiate(bulletA, transform.position, transform.rotation);
        Rigidbody2D rigid01 = bullet01.GetComponent<Rigidbody2D>();

        Vector3 dirVec = player.transform.position - this.transform.position;
        rigid01.AddForce(dirVec.normalized * 2.0f, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "borderBullet") // 자신 소멸
            Destroy(this.gameObject);

        if (collision.gameObject.tag == "playerBullet") // 플레이어 총알
        {
            bullet bullet = collision.gameObject.GetComponent<bullet>();

            //OnHit(bullet.damage); // 자신의 hp 깎기 
            health -= bullet.damage;
            if (health <= 0)
            {
                GameObject item01 
                    = Instantiate(itemToDrop, 
                                  transform.position + Vector3.down * 0.2f, 
                                  transform.rotation); // 아이템 드롭 

                // 아래로 느리게 이동
                item01.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1) * 30);  

                manager.GetComponent<gameMgr>().incScore(20);
                Destroy(gameObject);
            }

            spriteRender.color = new Color(0.46f, 0.87f, 0.95f, 1);
            Invoke("restoreColor", 0.1f);

            Destroy(collision.gameObject);// 총알 소멸 
        }
    }

    void restoreColor()
    {
        spriteRender.color = new Color(1, 1, 1, 1);
    }
}
