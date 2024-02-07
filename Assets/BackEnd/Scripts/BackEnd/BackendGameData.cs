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

                Debug.Log($"게임 정보 데이터 삽입 성공. : {callback}");
            }
            else
            {
                Debug.LogError($"게임 정보 데이서 삽입 실패. : {callback}");
            }
        });
    }

    public void GameDataLoad()
    {
        Backend.GameData.GetMyData("USER_DATA", new Where(), callback =>
        {
            if(callback.IsSuccess())
            {
                Debug.Log($"게임 정보 데이터 불러오기 성공. : {callback}");
                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();
                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
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
                Debug.LogError($"게임 정보 데이터를 불러오기에 실패했습니다. :{callback}");
            }
            
        });
    }

    public void GameDataUpdate(UnityAction action = null)
    {
        if (userGameData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다." +
                            "Insert 혹은 Load를 통해 데이터를 생성해주세요.");
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

        // 게임 정보의 고유값(gameDataRowInDate)이 없으면 에러 메시지 출력
        if (string.IsNullOrEmpty(gameDataRowInData))
        {
            Debug.LogError("유저의 inDate 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
        }
        // 게임 정보이ㅡ 고유값이 있으면 테이블에 저장되어 있는 값 중 inDate 컬럼의 값과
        // 소유하는 유저의 owner_inDate가 일치하는 row를 검색하여 수정하는  UpdateV2() 호출
        else
        {
            Debug.Log($"{gameDataRowInData}의 게임 정보 데이터 수정을 요청합니다.");

            Backend.GameData.UpdateV2("USER_DATA", gameDataRowInData, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"게임 정보 데이터 수정에 성공했습니다. : {callback}");

                    action?.Invoke();

                    onGameDataLoadEvent?.Invoke();
                }
                else
                {
                    Debug.LogError($"게임 정보 데이터 수정에 실패했습니다. : {callback}");
                }
            });
        }
    }
}
