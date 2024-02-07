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
                // JSON ������ �Ľ� ����
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
                // JSON ������ �Ľ� ����
                catch (System.Exception e)
                {
                    // ���� ������ �⺻ ���·� ����
                    data.Reset();
                    // try-catch ���� ���
                    Debug.LogError(e);
                }
            }
            // ���� �ҷ����� ����
            else
            {
                // ���� ������ �⺻ ���·� ����
                // �Ϲ������� �������� ���¸� ����� �⺻���� ���ɸ� �����صΰ� ���������� �� �ҷ��ͼ� ���
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
