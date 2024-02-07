using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class Nickname : LoginBase
{
    [System.Serializable]
    public class NicknameEvent : UnityEngine.Events.UnityEvent { }
    public NicknameEvent onNicknameEvent = new NicknameEvent();

    [SerializeField]
    private Image imageNickname;
    [SerializeField]
    private TMP_InputField inputFieldNickname;

    [SerializeField]
    private Button btnUpdateNickname;

    private void OnEnable()
    {
        ResetUI(imageNickname);
        SetMessage("닉네임을 입력하세요");
    }

    public void OnClickUpdateNickname()
    {
        ResetUI(imageNickname);

        if (IsFieldDataEmpty(imageNickname, inputFieldNickname.text, "Nickname")) return;

        btnUpdateNickname.interactable = false;
        SetMessage("닉네임 변경중입니다..");

        UpdateNickname();
    }

    private void UpdateNickname()
    {
        Backend.BMember.UpdateNickname(inputFieldNickname.text, callback =>
        {
            btnUpdateNickname.interactable = true;

            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldNickname.text}(으)로 닉네임이 변경되었습니다.");

                onNicknameEvent?.Invoke();
            }
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 400:
                        message = "닉네임이 비어있거나 | 20자 이상이거나 | 앞/뒤에 공백이 있습니다.";
                        break;
                    case 409:
                        message = "이미 존재하는 닉네임입니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                GuideForIncorrectlyEnteredData(imageNickname, message);
            }
        });
    }
}
