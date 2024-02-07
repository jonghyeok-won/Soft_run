using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DataManager.Instance.health -= 1;
            gameObject.SetActive(false);
        }
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
