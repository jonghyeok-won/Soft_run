using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class DailyRankLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject rankDataPrefab;    
    [SerializeField]
    private Scrollbar scrollbar;           
    [SerializeField]
    private Transform rankDataParent;   
    [SerializeField]
    private DailyRankData myRankData;       

    private List<DailyRankData> rankDataList;

    private void Awake()
    {
        rankDataList = new List<DailyRankData>();

        for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
        {
            GameObject clone = Instantiate(rankDataPrefab, rankDataParent);
            rankDataList.Add(clone.GetComponent<DailyRankData>());
        }
    }

    private void OnEnable()
    {
        scrollbar.value = 1;
        GetRankList();
        GetMyRank();
    }

    private void GetRankList()
    {
        Backend.URank.User.GetRankList(Constants.DAILY_RANK_UUID, Constants.MAX_RANK_LIST, callback =>
        {
            if (callback.IsSuccess())
            {
                try
                {
                    Debug.Log($"랭킹 조회에 성공 : {callback}");

                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    if (rankDataJson.Count <= 0)
                    {
                        for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            SetRankData(rankDataList[i], i + 1, "-", 0);
                        }

                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
                        int rankerCount = rankDataJson.Count;

                        for (int i = 0; i < rankerCount; ++i)
                        {
                            rankDataList[i].Rank = int.Parse(rankDataJson[i]["rank"].ToString());
                            rankDataList[i].Score = int.Parse(rankDataJson[i]["score"].ToString());

                            rankDataList[i].Nickname = rankDataJson[i].ContainsKey("nickname") == true ?
                                                       rankDataJson[i]["nickname"]?.ToString() : UserInfo.Data.gamerId;
                        }
                        for (int i = rankerCount; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            SetRankData(rankDataList[i], i + 1, "-", 0);
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                {
                    SetRankData(rankDataList[i], i + 1, "-", 0);
                }

                Debug.LogError($"랭킹 조회 중 오류 발생 : {callback}");
            }
        });
    }

    private void GetMyRank()
    {
        Backend.URank.User.GetMyRank(Constants.DAILY_RANK_UUID, callback =>
        {
            string nickname = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;

            if (callback.IsSuccess())
            {
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    if (rankDataJson.Count <= 0)
                    {
                        SetRankData(myRankData, 1000000000, nickname, 0);

                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
                        myRankData.Rank = int.Parse(rankDataJson[0]["rank"].ToString());
                        myRankData.Score = int.Parse(rankDataJson[0]["score"].ToString());

                        myRankData.Nickname = rankDataJson[0].ContainsKey("nickname") == true ?
                                              rankDataJson[0]["nickname"]?.ToString() : UserInfo.Data.gamerId;
                    }
                }
                catch (System.Exception e)
                {
                    SetRankData(myRankData, 1000000000, nickname, 0);

                    Debug.LogError(e);
                }
            }
            else
            {
                if (callback.GetMessage().Contains("userRank"))
                {
                    SetRankData(myRankData, 1000000000, nickname, 0);
                }
            }
        });
    }

    private void SetRankData(DailyRankData rankData, int rank, string nickname, int score)
    {
        rankData.Rank = rank;
        rankData.Nickname = nickname;
        rankData.Score = score;
    }
}

