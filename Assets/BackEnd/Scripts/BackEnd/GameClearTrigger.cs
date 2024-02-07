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
            Debug.LogError("GameController�� ������ �߰ߵ��� �ʾҽ��ϴ�.");
        }
        else
        {
            Debug.Log("GameController�� ���������� ����Ǿ����ϴ�.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Ʈ���ſ� ���𰡰� �浹�߽��ϴ�. �浹�� ��ü: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾ GameClear Ʈ���ſ� �����߽��ϴ�.");

            if (gameController != null)
            {
                DataManager.Instance.PlayerClear = true;
            }
            else
            {
                Debug.LogError("GameController�� null�Դϴ�.");
            }
        }
        else
        {
            Debug.Log("�÷��̾ �ƴ� ���� Ʈ���ſ� �浹�߽��ϴ�.");
        }
    }
}
