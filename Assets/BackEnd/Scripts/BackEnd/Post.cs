using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using System;

public class Post : MonoBehaviour
{
    [SerializeField]
    private Sprite[] spriteItemIcon;
    [SerializeField]
    private Image imageItemIcon;
    [SerializeField]
    private TextMeshProUGUI textItemCount;
    [SerializeField]
    private TextMeshProUGUI textTitle;
    [SerializeField]
    private TextMeshProUGUI textContent;
    [SerializeField]
    private TextMeshProUGUI textExpirationDate;

    [SerializeField]
    private Button buttonRecieve;

    private BackendPostSystem backendPostsystem;
    private PopupPostBox popupPostBox;
    private PostData postData;

    public void Setup(BackendPostSystem postSystem, PopupPostBox postBox ,PostData postData)
    {
        buttonRecieve.onClick.AddListener(OnClickPostReceive);

        backendPostsystem = postSystem;
        popupPostBox = postBox;
        this.postData = postData;

        textTitle.text = postData.title;
        textContent.text = postData.content;

        foreach(string itemKey in postData.postReward.Keys)
        {
            if (itemKey.Equals("heart")) imageItemIcon.sprite = spriteItemIcon[0];
            else if (itemKey.Equals("gold")) imageItemIcon.sprite = spriteItemIcon[1];
            else if (itemKey.Equals("jewel")) imageItemIcon.sprite = spriteItemIcon[2];

            textItemCount.text = postData.postReward[itemKey].ToString();

            break;
        }

        Backend.Utils.GetServerTime(callback =>
        {
            if(!callback.IsSuccess())
            {
                Debug.LogError("서버 시간 불러오기에 실패했습니다.");
            }
            
            try
            {
                string serverTime = callback.GetFlattenJSON()["utcTime"].ToString();
                TimeSpan timeSpan = DateTime.Parse(postData.expirationDate) - DateTime.Parse(serverTime);

                textExpirationDate.text = $"{timeSpan.TotalHours:F0}시간 후 만료";
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        });
    }

    private void OnClickPostReceive()
    {
        popupPostBox.DestroyPost(gameObject);
        backendPostsystem.PostReceive(PostType.Admin, postData.inDate);
    }
}
