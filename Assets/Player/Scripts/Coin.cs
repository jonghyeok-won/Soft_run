using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject player;
    public AudioClip audioCoin;

    AudioSource audioSource;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("PlayerPosition");
        float distance = Vector2.Distance(gameObject.transform.position, player.transform.position);
        if (DataManager.Instance.PlayerDie == false && DataManager.Instance.magnetTimeCurrent > 0)
        {
            if (distance < 6)
            {
                Vector2 dir = player.transform.position - transform.position;

                transform.Translate(dir.normalized * DataManager.Instance.itemMoveSpeed * Time.deltaTime, Space.World);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            //audioSource.clip = audioCoin;
            //audioSource.Play();
            Debug.Log("PlaySound");
            DataManager.Instance.score += DataManager.Instance.plusScore;
            gameObject.SetActive(false);
        }
    }
    void Start()
    {
        
    }

}
