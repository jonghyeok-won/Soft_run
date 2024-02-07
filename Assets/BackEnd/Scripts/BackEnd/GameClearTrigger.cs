using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearTrigger : MonoBehaviour
{
    private GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

        if (gameController == null)
        {
            Debug.LogError("GameController가 씬에서 발견되지 않았습니다.");
        }
        else
        {
            Debug.Log("GameController가 성공적으로 연결되었습니다.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("트리거에 무언가가 충돌했습니다. 충돌한 객체: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어가 GameClear 트리거에 도달했습니다.");

            if (gameController != null)
            {
                DataManager.Instance.PlayerClear = true;
            }
            else
            {
                Debug.LogError("GameController가 null입니다.");
            }
        }
        else
        {
            Debug.Log("플레이어가 아닌 것이 트리거에 충돌했습니다.");
        }
    }
}
