using UnityEngine;
using TMPro;

public class PopupUpdateProfileViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textNickname;
    [SerializeField]
    private TextMeshProUGUI textGamerID;

    public void UpdateNickname()
    {
        textNickname.text = UserInfo.Data.nickname == null ?
                            UserInfo.Data.gamerId : UserInfo.Data.nickname; 

        textGamerID.text = UserInfo.Data.gamerId;
    }
}
