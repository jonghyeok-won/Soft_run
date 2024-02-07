using UnityEngine;
using BackEnd;
using UnityEngine.Events;

public class BackendGameData
{
    [System.Serializable]
    public class GameDataLoadEvent : UnityEvent { }
    public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();

    private static BackendGameData instance = null;
    public static BackendGameData Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new BackendGameData();
            }

            return instance;
        }
    }

    private UserGameData userGameData = new UserGameData();
    public UserGameData UserGameData => userGameData;

    private string gameDataRowInData = string.Empty;

    public void GameDataInsert()
    {
        userGameData.Reset();

        Param param = new Param()
        {
            { "level", userGameData.level },
            { "gold", userGameData.gold },
            { "jewel", userGameData.jewel },
            { "experience", userGameData.experience },
            { "heart", userGameData.heart },
            { "dailyBestScore", userGameData.dailyBestScore }
        };

        Backend.GameData.Insert("USER_DATA", param, callback =>
        {
            if (callback.IsSuccess())
            {
                gameDataRowInData = callback.GetInDate();

                Debug.Log($"���� ���� ������ ���� ����. : {callback}");
            }
            else
            {
                Debug.LogError($"���� ���� ���̼� ���� ����. : {callback}");
            }
        });
    }

    public void GameDataLoad()
    {
        Backend.GameData.GetMyData("USER_DATA", new Where(), callback =>
        {
            if(callback.IsSuccess())
            {
                Debug.Log($"���� ���� ������ �ҷ����� ����. : {callback}");
                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();
                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
                    }
                    else
                    {
                        gameDataRowInData = gameDataJson[0]["inDate"].ToString();
                        userGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
                        userGameData.gold = int.Parse(gameDataJson[0]["gold"].ToString());
                        userGameData.jewel = int.Parse(gameDataJson[0]["jewel"].ToString());
                        userGameData.experience = float.Parse(gameDataJson[0]["experience"].ToString());
                        userGameData.heart = int.Parse(gameDataJson[0]["heart"].ToString());
                        userGameData.dailyBestScore = int.Parse(gameDataJson[0]["dailyBestScore"].ToString());

                        onGameDataLoadEvent?.Invoke();
                    }
                }
                catch (System.Exception e)
                {
                    userGameData.Reset();
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.LogError($"���� ���� �����͸� �ҷ����⿡ �����߽��ϴ�. :{callback}");
            }
            
        });
    }

    public void GameDataUpdate(UnityAction action = null)
    {
        if (userGameData == null)
        {
            Debug.LogError("�������� �ٿ�ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�." +
                            "Insert Ȥ�� Load�� ���� �����͸� �������ּ���.");
            return;
        }

        Param param = new Param()
        {
            { "level", userGameData.level},
            { "experience", userGameData.experience},
            { "gold", userGameData.gold},
            { "jewel", userGameData.jewel},
            { "heart", userGameData.heart},
            { "dailyBestScore" , userGameData.dailyBestScore}
        };

        // ���� ������ ������(gameDataRowInDate)�� ������ ���� �޽��� ���
        if (string.IsNullOrEmpty(gameDataRowInData))
        {
            Debug.LogError("������ inDate ������ ���� ���� ���� ������ ������ �����߽��ϴ�.");
        }
        // ���� �����̤� �������� ������ ���̺� ����Ǿ� �ִ� �� �� inDate �÷��� ����
        // �����ϴ� ������ owner_inDate�� ��ġ�ϴ� row�� �˻��Ͽ� �����ϴ�  UpdateV2() ȣ��
        else
        {
            Debug.Log($"{gameDataRowInData}�� ���� ���� ������ ������ ��û�մϴ�.");

            Backend.GameData.UpdateV2("USER_DATA", gameDataRowInData, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"���� ���� ������ ������ �����߽��ϴ�. : {callback}");

                    action?.Invoke();

                    onGameDataLoadEvent?.Invoke();
                }
                else
                {
                    Debug.LogError($"���� ���� ������ ������ �����߽��ϴ�. : {callback}");
                }
            });
        }
    }
}
