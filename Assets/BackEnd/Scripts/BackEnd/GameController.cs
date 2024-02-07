using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onGameOver;
    [SerializeField]
    private UnityEvent onGameClear;
    [SerializeField]
    private DataManager dataManager;
    [SerializeField]
    private DailyRankRegister dailyRank;

    public bool IsGameOver { set; get; } = false; 

    public bool IsGameClear { set; get; } = false;

    public void GameOver()
    {
        if (IsGameOver == true) return;
        IsGameOver = true;

        onGameOver.Invoke();

        dailyRank.Process(dataManager.Score);

        // ����ġ ���� �� ������ ���� �˻�
        // ���� �ý��� �̱���, ����ġ �ִ� 100���� ����
        // ������ ���� �÷��� �� ������ 25�� ����
        BackendGameData.Instance.UserGameData.experience += 25;
        if(BackendGameData.Instance.UserGameData.experience >= 100)
        {
            BackendGameData.Instance.UserGameData.experience = 0;
            BackendGameData.Instance.UserGameData.level++;
        }

        BackendGameData.Instance.GameDataUpdate();
    }

    public void GameClear()
    {
        if(IsGameClear == true) return;
        IsGameClear = true;

        onGameClear.Invoke();

        dailyRank.Process(dataManager.Score);

        if (BackendGameData.Instance.UserGameData.experience >= 100)
        {
            BackendGameData.Instance.UserGameData.experience = 0;
            BackendGameData.Instance.UserGameData.level++;
        }

        BackendGameData.Instance.GameDataUpdate();
    }
}
