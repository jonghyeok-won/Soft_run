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
        SetMessage("�г����� �Է��ϼ���");
    }

    public void OnClickUpdateNickname()
    {
        ResetUI(imageNickname);

        if (IsFieldDataEmpty(imageNickname, inputFieldNickname.text, "Nickname")) return;

        btnUpdateNickname.interactable = false;
        SetMessage("�г��� �������Դϴ�..");

        UpdateNickname();
    }

    private void UpdateNickname()
    {
        Backend.BMember.UpdateNickname(inputFieldNickname.text, callback =>
        {
            btnUpdateNickname.interactable = true;

            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldNickname.text}(��)�� �г����� ����Ǿ����ϴ�.");

                onNicknameEvent?.Invoke();
            }
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 400:
                        message = "�г����� ����ְų� | 20�� �̻��̰ų� | ��/�ڿ� ������ �ֽ��ϴ�.";
                        break;
                    case 409:
                        message = "�̹� �����ϴ� �г����Դϴ�.";
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
