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

        // 경험치 증가 및 레벨업 여부 검사
        // 레벨 시스템 미구현, 경험치 최대 100으로 가정
        // 게임을 한판 플레이 할 때마다 25씩 증가
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
