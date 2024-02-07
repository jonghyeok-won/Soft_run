using UnityEngine;
using BackEnd;
using LitJson;
using Unity.VisualScripting;

public class UserInfo : MonoBehaviour
{
    [System.Serializable]
    public class UserInfoEvent : UnityEngine.Events.UnityEvent { }

    public UserInfoEvent onUserInfoEvent = new UserInfoEvent();

    private static UserInfoData data = new UserInfoData();
    public static UserInfoData Data => data;

    public void GetUserInfofromBackend()
    {
        Backend.BMember.GetUserInfo(callback =>
        {
            if (callback.IsSuccess())
            {
                // JSON 데이터 파싱 성공
                try
                {
                    JsonData json = callback.GetReturnValuetoJSON()["row"];

                    data.gamerId = json["gamerId"].ToString();
                    data.countryCode = json["countryCode"]?.ToString();
                    data.nickname = json["nickname"]?.ToString();
                    data.inDate = json["inDate"].ToString();
                    data.emailForFindPassword = json["emailForFindPassword"]?.ToString();
                    data.subscriptionType = json["subscriptionType"].ToString();
                    data.federationId = json["federationId"]?.ToString();
                }
                // JSON 데이터 파싱 실패
                catch (System.Exception e)
                {
                    // 유저 정보를 기본 상태로 설정
                    data.Reset();
                    // try-catch 에러 출력
                    Debug.LogError(e);
                }
            }
            // 정보 불러오기 실패
            else
            {
                // 유저 정보를 기본 상태로 설정
                // 일반적으로 오프라인 상태를 대비해 기본적인 적옵를 저장해두고 오프라인일 때 불러와서 사용
                data.Reset();
                Debug.LogError(callback.GetMessage());

            }

            onUserInfoEvent?.Invoke();
        });
    }
}

public class UserInfoData
{
    public string gamerId;
    public string countryCode;
    public string nickname;
    public string inDate;
    public string emailForFindPassword;
    public string subscriptionType;
    public string federationId;

    public void Reset()
    {
        gamerId = "Offline";
        countryCode = "Unknown";
        nickname = "Noname";
        inDate = string.Empty;
        emailForFindPassword = string.Empty;
        subscriptionType = string.Empty;
        federationId = string.Empty;
    }
}
