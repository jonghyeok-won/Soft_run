using BackEnd;
using System;
using UnityEngine;

public class BackendFriendSystem : MonoBehaviour
{
    //[SerializeField]
    //private FriendSentRequestPage sentRequestPage;

    private string GetUserInfoBy(string nickname)
    {       
        var bro = Backend.Social.GetUserInfoByNickName(nickname);
        string inDate = string.Empty;

        if (!bro.IsSuccess())
        {
            Debug.LogError($"���� �˻� ���� ������ �߻�. : {bro}");
            return inDate;
        }

        try
        {
            LitJson.JsonData jsonData = bro.GetFlattenJSON()["row"];

            if (jsonData.Count <= 0)
            {
                Debug.LogWarning("������ indate ������ ������ �����ϴ�.");
                return inDate;
            }

            inDate = jsonData["inDate"].ToString();

            Debug.Log($"{nickname}�� inDate ���� {inDate} �Դϴ�.");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return inDate;
    }

    public void SendRequestFriend(string nickname)
    {
        string inDate = GetUserInfoBy(nickname);

        Backend.Friend.RequestFriend(inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"{nickname} ģ�� ��û �� ������ �߻�. : {callback}");
                return;
            }

            Debug.Log($"ģ�� ��û�� �����߽��ϴ�. : {callback}");

            //GetSentRequestList();
        });

    }


    /*public void GetSentRequestList()
    {
        Backend.Friend.GetSentRequestList(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($". : {callback}");
                return;
            }

            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];

                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning(".");
                    return;
                }

                sentRequestPage.DeactivateAll();

                foreach (LitJson.JsonData item in jsonData)
                {
                    FriendData friendData = new FriendData();

                    //friend.nickname		= item.ContainsKey("nickname") == true ? item["nickname"].ToString() : "NONAME";
                    friendData.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" : item["nickname"].ToString();
                    friendData.inDate = item["inDate"].ToString();
                    friendData.createdAt = item["createdAt"].ToString();

                    if (IsExpirationDate(friendData.createdAt))
                    {
                        RevokeSentRequest(friendData.inDate);
                        continue;
                    }

                    sentRequestPage.Activate(friendData);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        });
    }*/

    /*public void RevokeSentRequest(string inDate)
    {
        Backend.Friend.RevokeSentRequest(inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($". : {callback}");
                return;
            }

            Debug.Log($". : {callback}");
        });
    }*/

    /*private bool IsExpirationDate(string createdAt)
    {
        var bro = Backend.Utils.GetServerTime();

        if (!bro.IsSuccess())
        {
            Debug.LogError($". : {bro}");
            return false;
        }

        try
        {
            DateTime after3Days = DateTime.Parse(createdAt).AddDays(Constants.EXPIRATION_DAYS);
            string serverTime = bro.GetFlattenJSON()["utcTime"].ToString();
            TimeSpan timeSpan = after3Days - DateTime.Parse(serverTime);

            if (timeSpan.TotalHours < 0)
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return false;
    }*/
}

