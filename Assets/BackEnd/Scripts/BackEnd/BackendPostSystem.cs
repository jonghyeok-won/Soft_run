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
                Debug.LogError($"우편을 불러오는 중 에러가 발생했습니다. {callback}");
                return;
            }

            Debug.Log($"우편 리스트 불러오기에 성공했습니다. {callback}");

            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["postList"];

                if(jsonData.Count <= 0)
                {
                    Debug.LogWarning("우편함이 비어있습니다.");
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
                            Debug.LogWarning($"아직 지원하지 않는 차트 정보입니다. :{itemJson["chartName"].ToString()}");
                            post.isCanRecieve = false;
                        }
                    }
                    postList.Add(post);
                }

                onGetPostListEvent?.Invoke(postList);

                for(int i = 0; i <postList.Count; ++i)
                {
                    Debug.Log($"{i}번째 우편\n{postList[i].ToString()}");
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
            Debug.LogWarning("받을 수 있는 우편이 존재하지 않습니다");
            return;
        }

        if(index >= postList.Count)
        {
            Debug.LogError($"해당 우편은 존재하지 않습니다. : 요쳥 index{index} / 우편 최대 갯수 : {postList.Count}");
            return;
        }

        Debug.Log($"{postType.ToString()}의 {postList[index].inDate} 우편수령을 요청합니다.");

        Backend.UPost.ReceivePostItem(postType, postList[index].inDate, callback =>
        {
            if(!callback.IsSuccess())
            {
                Debug.LogError($"{postType.ToString()}의 {postList[index].inDate} 우편수령 중 에러가 발생했습니다. {callback}");
                return;
            }
            Debug.Log($"{postType.ToString()}의 {postList[index].inDate} 우편수령에 성공했습니다. : {callback}");

            postList.RemoveAt(index);

            if (callback.GetFlattenJSON()["postItems"].Count > 0)
            {
                SavePostToLocal(callback.GetFlattenJSON()["postItems"]);
                BackendGameData.Instance.GameDataUpdate();
            }
            else
            {
                Debug.LogWarning("수령 가능한 우편이 없습니다.");
            }
        });
    }

    public void PostRecieveAll(PostType postType)
    {
        if(postList.Count <= 0 )
        {
            Debug.LogWarning("받을 수 있는 우편이 없습니다.");
            return;
        }
        Debug.Log($"{postType.ToString()} 우편수령을 요청합니다.");

        Backend.UPost.ReceivePostItemAll(postType, callback =>
        {
            if(!callback.IsSuccess())
            {
                Debug.LogError("우편 수령 중 에러가 발생했습니다.");
                return;
            }

            Debug.Log("우편 전체 수령에 성공했습니다.");

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
                Debug.Log($"[{itemId}] {itemName} : {itemInfo}, 획득 수량 : {itemCount}");
                Debug.Log($"아이템을 수령했습니다. : {itemName} - {itemCount}개");
            }
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
    }


}
    