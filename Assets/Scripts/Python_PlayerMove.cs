using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Python_PlayerMove : MonoBehaviour
{
    public float jump;
    public float jump2;
    public AudioClip audioJump;
    public AudioClip audioSlide;
    public Slider AbilityBar;
    public Slider GageBar;

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
        jump = 10;
        jump2 = 12;
        jumpCount = 0;
        isDamaged = false;
        damagedDuration = 2.0f;
        damagedTimer = 0.0f;
        abilityDuration = 3.0f; //특수능력 시간 3초
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
        //특수능력 게이지 위치
        Vector3 playerHeadPosition = transform.position + new Vector3(0.3f, 1.8f, 0f);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(playerHeadPosition);
        AbilityBar.transform.position = screenPosition;
        Vector3 playerHeadPosition2 = transform.position + new Vector3(0.3f, 2f, 0f);
        Vector3 screenPositon2 = Camera.main.WorldToScreenPoint(playerHeadPosition2);
        GageBar.transform.position = screenPositon2;

        //특수능력 게이지
        if (!DataManager.Instance.PlayerDie && DataManager.Instance.abilityGageCurrent <= DataManager.Instance.abilityGageMax)
        {
            DataManager.Instance.abilityGageCurrent += 1 * Time.deltaTime; //1초당 1씩 게이지 추가
            AbilityBar.value = DataManager.Instance.abilityGageCurrent / DataManager.Instance.abilityGageMax;

        }
        //python 어빌리티
        if (DataManager.Instance.ability&& !DataManager.Instance.PlayerDie)
        {
            abilityTimer += Time.deltaTime;
            DataManager.Instance.GageCurrent -= Time.deltaTime;
            GageBar.value = DataManager.Instance.GageCurrent / DataManager.Instance.GageMax;
            if (abilityTimer >= abilityDuration)
            {
                DataManager.Instance.ability = false;
                abilityTimer = 0.0f;
                DataManager.Instance.abilityGageCurrent = 0.0f;
            }
        }
        CheckIfOutOfCameraView();
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
                audioSource.clip = audioJump;
                audioSource.Play();
                anim.SetTrigger("Jump");
            }
            else if (jumpCount == 1)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, jump2, 0);
                jumpCount++;
                audioSource.clip = audioJump;
                audioSource.Play();
                anim.SetTrigger("DoubleJump");
            }
        }
    }

    public void Slide_Btn_Down()
    {
        if (jumpCount == 0)
        {
            anim.SetBool("isSliding", true);

            audioSource.clip = audioSlide;
            audioSource.Play();

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
            DataManager.Instance.magnetTimeCurrent += abilityDuration;

            DataManager.Instance.GageCurrent = 3f;
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

    void CheckIfOutOfCameraView()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            // 캐릭터가 카메라 뷰포트 밖으로 나갔을 경우 사망 처리
            Debug.Log("캐릭터가 카메라 시야 밖으로 나갔습니다.");
            DataManager.Instance.health = 0;
            // 추가적인 사망 처리 로직...
        }
    }
}
