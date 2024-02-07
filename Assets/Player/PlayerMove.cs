using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float jump;
    public float jump2;
    //public AudioClip audioJump;
    //public AudioClip audioSlide;
    //public Slider AbilityBar;

    Animator anim;
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;
    Vector2 originalSize;
    Vector2 originalOffset;

    int jumpCount;
    bool isDamaged;
    float damagedDuration ;
    float damagedTimer;
    float abilityDuration ; 
    float abilityTimer;
 
    public void Awake()
    {
        jump = 8;
        jump2 = 10;
        jumpCount = 0;
        isDamaged = false;
        damagedDuration = 2.0f;
        damagedTimer = 0.0f;
        abilityDuration = 3.0f; //특수능력 무적 시간 3초
        abilityTimer = 0.0f;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Update()
    {
        if (isDamaged)
        {
            damagedTimer += Time.deltaTime;
            if (damagedTimer >= damagedDuration)
            {
                gameObject.layer = originalLayer;
                spriteRenderer.color = new Color(1, 1, 1, 1);
                isDamaged = false;
                damagedTimer = 0.0f;
            }
        }
        /*//특수능력 게이지 위치
        Vector3 playerHeadPosition = transform.position + new Vector3(1.5f, -1f, 0f);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(playerHeadPosition);
        AbilityBar.transform.position = screenPosition;
        
        //특수능력 게이지
        if (!DataManager.Instance.PlayerDie && DataManager.Instance.abilityGageCurrent <= DataManager.Instance.abilityGageMax)
        {
            DataManager.Instance.abilityGageCurrent += 1 * Time.deltaTime; //1초당 1씩 게이지 추가
            AbilityBar.value = DataManager.Instance.abilityGageCurrent / DataManager.Instance.abilityGageMax;

        }
        //c언어 어빌리티
        if (DataManager.Instance.ability)
        {
            originalLayer = gameObject.layer;
            gameObject.layer = LayerMask.NameToLayer("PlayerDamaged");
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);

            abilityTimer += Time.deltaTime;
            if (abilityTimer >= abilityDuration)
            {
                gameObject.layer = LayerMask.NameToLayer("Player");
                spriteRenderer.color = new Color(1, 1, 1, 1);
                DataManager.Instance.ability = false;
                abilityTimer = 0.0f;
                DataManager.Instance.abilityGageCurrent = 0.0f;
            }
        }*/
        //추락하면 체력 0으로
        if (transform.position.y < -10f)
        {
            DataManager.Instance.health = 0;
        }
    }
    public void Jump_Btn()
    {
        if (!DataManager.Instance.PlayerDie)
        {
            if (jumpCount == 0)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, jump, 0);
                jumpCount++;
                //audioSource.clip = audioJump;
                //audioSource.Play();
            }
            else if (jumpCount == 1)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, jump2, 0);
                jumpCount++;
                //audioSource.clip = audioJump;
                //audioSource.Play();
            }
        }
    }

    public void Slide_Btn_Down()
    {
        if (jumpCount == 0)
        {
            anim.SetBool("isSliding", true);

            //audioSource.clip = audioSlide;
            //audioSource.Play();

            originalSize = gameObject.GetComponent<BoxCollider2D>().size;

            Vector2 newSize = gameObject.GetComponent<BoxCollider2D>().size;
            newSize.y *= 0.5f; 
            gameObject.GetComponent<BoxCollider2D>().size = newSize;

            originalOffset = gameObject.GetComponent<BoxCollider2D>().offset;
            Vector2 newOffset = gameObject.GetComponent<BoxCollider2D>().offset;
            newOffset.y -= newSize.y / 2;
            gameObject.GetComponent<BoxCollider2D>().offset = newOffset;
        }
    }

    public void Slide_Btn_Up()
    {
        if (jumpCount == 0)
        { 
            anim.SetBool("isSliding", false);
            gameObject.GetComponent<BoxCollider2D>().size = originalSize;
            gameObject.GetComponent<BoxCollider2D>().offset = originalOffset;
        }
    }

    public void Ability_Btn()
    {
        if (DataManager.Instance.abilityGageCurrent >= DataManager.Instance.abilityGageMax)
        {
            DataManager.Instance.ability = true;
        }
    }

    private int originalLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Block") == 0)
        {
            originalLayer = gameObject.layer;
            gameObject.layer = LayerMask.NameToLayer("PlayerDamaged");
            Debug.Log("attack");
            DataManager.Instance.health -= 1;
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            isDamaged = true;
        }

        if (collision.gameObject.tag.CompareTo("Land") == 0)
        {
            jumpCount = 0;
        }
    }

    
}
