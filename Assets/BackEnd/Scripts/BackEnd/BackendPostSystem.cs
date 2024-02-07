using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BackEnd;


public class BackendPostSystem : MonoBehaviour
{
    [System.Serializable]
    public class PostEvent : UnityEvent<List<PostData>> { }
    public PostEvent onGetPostListEvent = new PostEvent();

    private List<PostData> postList = new List<PostData>();

    public void PostListGet()
    {
        PostListGet(PostType.Admin);
    }

    public void PostReceive(PostType postType, string inDate)
    {
        PostReceive(postType, postList.FindIndex(item => item.inDate.Equals(inDate)));
        /*for(int i = 0; i< postList.Count; ++i)
        {
            if (postList[i].inDate.Equals(inDate))
            {
                PostReceive(postType, i);
                return;
            }
        }*/
    }

    public void PostRecieveAll()
    {
        PostRecieveAll(PostType.Admin);
    }

    public void PostListGet(PostType postType)
    {
        Backend.UPost.GetPostList(postType, callback =>
        {
            if(!callback.IsSuccess())
            {
                Debug.LogError($"������ �ҷ����� �� ������ �߻��߽��ϴ�. {callback}");
                return;
            }

            Debug.Log($"���� ����Ʈ �ҷ����⿡ �����߽��ϴ�. {callback}");

            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["postList"];

                if(jsonData.Count <= 0)
                {
                    Debug.LogWarning("�������� ����ֽ��ϴ�.");
                    return;
                }

                postList.Clear();

                for(int i = 0; i< jsonData.Count; ++i)
                {
                    PostData post = new PostData();

                    post.title = jsonData[i]["title"].ToString();
                    post.content = jsonData[i]["content"].ToString();
                    post.inDate = jsonData[i]["inDate"].ToString();
                    post.expirationDate = jsonData[i]["expirationDate"].ToString();

                    foreach(LitJson.JsonData itemJson in jsonData[i]["items"])
                    {
                        if (itemJson["chartName"].ToString() == Constants.GOODS_CHART_NAME)
                        {
                            string itemName = itemJson["item"]["itemName"].ToString();
                            int itemCount = int.Parse(itemJson["itemCount"].ToString());

                            if(post.postReward.ContainsKey(itemName))
                            {
                                post.postReward[itemName] += itemCount;
                            }
                            else
                            {
                                post.postReward.Add(itemName, itemCount);
                            }
                            
                            post.isCanRecieve = true;
                        }
                        else
                        {
                            Debug.LogWarning($"���� �������� �ʴ� ��Ʈ �����Դϴ�. :{itemJson["chartName"].ToString()}");
                            post.isCanRecieve = false;
                        }
                    }
                    postList.Add(post);
                }

                onGetPostListEvent?.Invoke(postList);

                for(int i = 0; i <postList.Count; ++i)
                {
                    Debug.Log($"{i}��° ����\n{postList[i].ToString()}");
                }
            }
            catch(System.Exception e)
            {
                Debug.LogError(e);  
            }
        });
    }

    public void PostReceive(PostType postType, int index)
    {
        if(postList.Count <= 0)
        {
            Debug.LogWarning("���� �� �ִ� ������ �������� �ʽ��ϴ�");
            return;
        }

        if(index >= postList.Count)
        {
            Debug.LogError($"�ش� ������ �������� �ʽ��ϴ�. : �䫊 index{index} / ���� �ִ� ���� : {postList.Count}");
            return;
        }

        Debug.Log($"{postType.ToString()}�� {postList[index].inDate} ��������� ��û�մϴ�.");

        Backend.UPost.ReceivePostItem(postType, postList[index].inDate, callback =>
        {
            if(!callback.IsSuccess())
            {
                Debug.LogError($"{postType.ToString()}�� {postList[index].inDate} ������� �� ������ �߻��߽��ϴ�. {callback}");
                return;
            }
            Debug.Log($"{postType.ToString()}�� {postList[index].inDate} ������ɿ� �����߽��ϴ�. : {callback}");

            postList.RemoveAt(index);

            if (callback.GetFlattenJSON()["postItems"].Count > 0)
            {
                SavePostToLocal(callback.GetFlattenJSON()["postItems"]);
                BackendGameData.Instance.GameDataUpdate();
            }
            else
            {
                Debug.LogWarning("���� ������ ������ �����ϴ�.");
            }
        });
    }

    public void PostRecieveAll(PostType postType)
    {
        if(postList.Count <= 0 )
        {
            Debug.LogWarning("���� �� �ִ� ������ �����ϴ�.");
            return;
        }
        Debug.Log($"{postType.ToString()} ��������� ��û�մϴ�.");

        Backend.UPost.ReceivePostItemAll(postType, callback =>
        {
            if(!callback.IsSuccess())
            {
                Debug.LogError("���� ���� �� ������ �߻��߽��ϴ�.");
                return;
            }

            Debug.Log("���� ��ü ���ɿ� �����߽��ϴ�.");

            postList.Clear();

            foreach (LitJson.JsonData postItemsJson in callback.GetFlattenJSON()["postItems"])
            {
                SavePostToLocal(postItemsJson);
            }

            BackendGameData.Instance.GameDataUpdate();
        });


    }

    public void SavePostToLocal(LitJson.JsonData item)
    {
        try
        {
            foreach(LitJson.JsonData itemJson in item)
            {
                string chartFileName = itemJson["item"]["chartFileName"].ToString();
                string chartName = itemJson["chartName"].ToString();

                int itemId = int.Parse(itemJson["item"]["itemId"].ToString());
                string itemName = itemJson["item"]["itemName"].ToString();
                string itemInfo = itemJson["item"]["itemInfo"].ToString();

                int itemCount = int.Parse(itemJson["itemCount"].ToString());

                if(chartName.Equals(Constants.GOODS_CHART_NAME))
                {
                    if(itemName.Equals("heart"))
                    {
                        BackendGameData.Instance.UserGameData.heart += itemCount;
                    }
                    else if(itemName.Equals("gold"))
                    {
                        BackendGameData.Instance.UserGameData.gold += itemCount;
                    }
                    else if(itemName.Equals("jewel"))
                    {
                        BackendGameData.Instance.UserGameData.jewel += itemCount;
                    }
                }

                Debug.Log($"{chartName} - {chartFileName}");
                Debug.Log($"[{itemId}] {itemName} : {itemInfo}, ȹ�� ���� : {itemCount}");
                Debug.Log($"�������� �����߽��ϴ�. : {itemName} - {itemCount}��");
            }
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
    }


}
    