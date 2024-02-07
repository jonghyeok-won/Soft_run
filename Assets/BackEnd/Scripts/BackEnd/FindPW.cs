using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class FindPW : LoginBase
{
    [SerializeField]
    private Image imageID;
    [SerializeField]
    private TMP_InputField inputFieldID;
    [SerializeField]
    private Image imageEmail;
    [SerializeField]
    private TMP_InputField inputFieldEmail;

    [SerializeField]
    private Button btnFindPW;

    public void OnClickFindPW()
    {
        ResetUI(imageID, imageEmail);

        if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "���� �ּ�")) return;

        if (!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncorrectlyEnteredData(imageEmail, "���� ������ �߸��Ǿ����ϴ�.");
            return;
        }

        btnFindPW.interactable = false;
        SetMessage("���� �߼����Դϴ�...");

        FindCustumPW();
    }

    private void FindCustumPW()
    {
        // ��й�ȣ �ʱ�ȭ, �ʱ�ȭ�� ��й�ȣ ������ �̸��Ϸ� �߼�
        Backend.BMember.ResetPassword(inputFieldID.text, inputFieldEmail.text, callback =>
        {
            btnFindPW.interactable = true;

            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldEmail.text}�ּҷ� ������ �߼��Ͽ����ϴ�.");
            }
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 404:
                        message = "�ش� �̸����� ����ϴ� ����ڰ� �����ϴ�.";
                        break;
                    case 429:
                        message = "24�ð� �̳��� 5ȸ �̻� ���̵�/��й�ȣ ã�⸦ �õ��߽��ϴ�.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                if (message.Contains("�̸���"))
                {
                    GuideForIncorrectlyEnteredData(imageEmail, message);
                }
                else
                {
                    SetMessage(message);
                }
            }
        });
    }
}