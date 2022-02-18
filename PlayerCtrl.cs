using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BlockDir {
    _NONE,
    _LEFT,
    _RIGHT,
    _TOP,
    _BOTTOM
}

public class playerCtrl : MonoBehaviour
{
    public GameObject manager;

    public GameObject bulletA;
    public GameObject bulletB;

    SpriteRenderer spRender;

    BlockDir _blockDir = BlockDir._NONE;

    public float maxShootingDelay = 0.2f;
    public float curShootingDelay = 0.0f;

    public float health = 100;

    bool isNew = false;

    public float power = 1;

    private void Awake()
    {
        spRender = transform.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Mov();

        Fire();
    }

    private void Fire()
    {
        curShootingDelay += Time.deltaTime;
        if (curShootingDelay < maxShootingDelay)
            return;

        curShootingDelay = 0;

        if (!Input.GetButton("Fire1"))
            return;

        // 1 발 쏘기
        if(power==1 || power==2)
        {
            GameObject bullet01 =
                Instantiate(bulletA, transform.position + Vector3.up * 0.5f, transform.rotation);
            Rigidbody2D rigid01 = bullet01.GetComponent<Rigidbody2D>();
            rigid01.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
        }
        else if(power==3)
        {
            // bullet 1
            GameObject bulletBB = Instantiate(bulletB, 
                transform.position + Vector3.up * 0.5f + Vector3.left * 0.2f, transform.rotation);
            Rigidbody2D rigidBB = bulletBB.GetComponent<Rigidbody2D>();
            rigidBB.AddForce(Vector2.up * 2, ForceMode2D.Impulse);

            // bullet 2
            GameObject bulletAA = Instantiate(bulletA, 
                transform.position + Vector3.up * 0.5f, transform.rotation);
            Rigidbody2D rigidAA = bulletAA.GetComponent<Rigidbody2D>();
            rigidAA.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            
            // bullet 3 
            GameObject bulletCC = Instantiate(bulletB, 
                transform.position + Vector3.up * 0.5f + Vector3.right * 0.2f, transform.rotation);
            Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
            rigidCC.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
        }
    }

    void Mov()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        switch (_blockDir)
        {
            case BlockDir._LEFT:
                if (h == -1)
                    h = 0;
                break;
            case BlockDir._RIGHT:
                if (h == 1)
                    h = 0;
                break;
            case BlockDir._TOP:
                if (v == 1)
                    v = 0;
                break;
            case BlockDir._BOTTOM:
                if (v == -1)
                    v = 0;
                break;
        }

        Vector3 curPos = transform.position;
        Vector3 deltaPos = new Vector3(h, v, 0) * 2.0f * Time.deltaTime;

        transform.Translate(deltaPos);
    }

    private void OnEnable()
    {
        isNew = true;
        spRender.color = new Color(1, 1, 1, 0.5f);

        //Invoke("isNewOff", 2.0f);
        StartCoroutine(isNewOff());
    }

    IEnumerator isNewOff()
    {
        //위 부분
        yield return new WaitForSeconds(2f); //<-- 기억
        // 아래 부분
        isNew = false;
        spRender.color = new Color(1, 1, 1, 1);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "blockPlayer") // 밖으로 못가게 영역제한 
        {
            switch (collision.gameObject.name)
            {
                case "blockTop":    _blockDir = BlockDir._TOP; break;
                case "blockBottom": _blockDir = BlockDir._BOTTOM; break;
                case "blockLeft":   _blockDir = BlockDir._LEFT; break;
                case "blockRight":  _blockDir = BlockDir._RIGHT; break;
            }
        }

        if (collision.gameObject.tag == "enemy" ||
            collision.gameObject.tag == "enemyBullet")
        {
            // 내가 새로 나타났다면 
            if(isNew)
                return;

            health -= 20;

            spRender.color = new Color(1, 0.2f, 0.2f, 0.59f);
            Invoke("restoreColor", 0.1f);

            if (health <= 0)
            {
                // manager 한테 깨워달라 부탁 
                manager.transform.GetComponent<gameMgr>().restorePlayer();

                // 안보이게
                gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.tag == "item")
        {
            power++;
            power = Mathf.Min(3, power); // 최대 3 이하
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "blockPlayer") // 밖으로 못가게 영역제한 
        {
            _blockDir = BlockDir._NONE;
        }
    }

    void restoreColor()
    {
        spRender.color = new Color(1, 1, 1, 1);
    }

    public void Initialize()
    {
        health = 100;
        power = 1;

        //isDead = false;
    }
}

