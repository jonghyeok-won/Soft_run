using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_PlayerMove : MonoBehaviour
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
    private BoxCollider2D boxCollider;

    public void Awake()
    {
        jump = 8;
        jump2 = 10;
        jumpCount = 0;
        isDamaged = false;
        damagedDuration = 2.0f;
        damagedTimer = 0.0f;
        abilityDuration = 3.0f; //Ư���ɷ� ���� �ð� 3��
        abilityTimer = 0.0f;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalSize = boxCollider.size;
        originalOffset = boxCollider.offset;
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
        //Ư���ɷ� ������ ��ġ
        Vector3 playerHeadPosition = transform.position + new Vector3(0.3f, 1.8f, 0f);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(playerHeadPosition);
        AbilityBar.transform.position = screenPosition;
        Vector3 playerHeadPosition2 = transform.position + new Vector3(0.3f, 2f, 0f);
        Vector3 screenPositon2 = Camera.main.WorldToScreenPoint(playerHeadPosition2);
        GageBar.transform.position = screenPositon2;

        //Ư���ɷ� ������
        if (!DataManager.Instance.PlayerDie && DataManager.Instance.abilityGageCurrent <= DataManager.Instance.abilityGageMax)
        {
            DataManager.Instance.abilityGageCurrent += 1 * Time.deltaTime; //1�ʴ� 1�� ������ �߰�
            AbilityBar.value = DataManager.Instance.abilityGageCurrent / DataManager.Instance.abilityGageMax;

        }
        //c��� �����Ƽ
        if (DataManager.Instance.ability)
        {
            originalLayer = gameObject.layer;
            gameObject.layer = LayerMask.NameToLayer("PlayerDamaged");
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);

            abilityTimer += Time.deltaTime;
            DataManager.Instance.GageCurrent -= Time.deltaTime;
            GageBar.value = DataManager.Instance.GageCurrent / DataManager.Instance.GageMax;
            if (abilityTimer >= abilityDuration)
            {
                gameObject.layer = LayerMask.NameToLayer("Player");
                spriteRenderer.color = new Color(1, 1, 1, 1);
                DataManager.Instance.ability = false;
                abilityTimer = 0.0f;
                DataManager.Instance.abilityGageCurrent = 0.0f;
            }
        }
        CheckIfOutOfCameraView();
        //�߶��ϸ� ü�� 0����
        if (transform.position.y < -10f)
        {
            DataManager.Instance.health = 0;
        }
    }
    public void Jump_Btn()
    {
        if (!DataManager.Instance.PlayerDie)
        {
            Debug.Log("����");
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
            Debug.Log("�����̵�1");
            anim.SetBool("isSliding", true);

            audioSource.clip = audioSlide;
            audioSource.Play();

            Vector2 newSize = boxCollider.size; // ĳ�̵� �������� �ݶ��̴��� ũ�⸦ �����ɴϴ�.
            newSize.y *= 0.5f;
            boxCollider.size = newSize; // �� ũ�⸦ �ݶ��̴��� �Ҵ��մϴ�.

            Vector2 newOffset = boxCollider.offset;
            newOffset.y -= newSize.y / 2;
            boxCollider.offset = newOffset;
        }
    }

    public void Slide_Btn_Up()
    {
        if (jumpCount == 0)
        {
            Debug.Log("�����̵�2");
            anim.SetBool("isSliding", false);
            boxCollider.size = originalSize; 
            boxCollider.offset = originalOffset;
        }
    }

    public void Ability_Btn()
    {
        if (DataManager.Instance.abilityGageCurrent >= DataManager.Instance.abilityGageMax)
        {
            DataManager.Instance.ability = true;
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
            anim.ResetTrigger("Jump"); // ���� Ʈ���� ����
            anim.ResetTrigger("DoubleJump"); // ���� ���� Ʈ���� ����
            anim.SetTrigger("C_specialrun"); // �ٸ� �ִϸ��̼����� ��ȯ
        }
    }

    void CheckIfOutOfCameraView()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            // ĳ���Ͱ� ī�޶� ����Ʈ ������ ������ ��� ��� ó��
            Debug.Log("ĳ���Ͱ� ī�޶� �þ� ������ �������ϴ�.");
            DataManager.Instance.health = 0;
            // �߰����� ��� ó�� ����...
        }
    }

}
