using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class RegisterAccount : LoginBase
{
    [SerializeField]
    private Image imageID;
    [SerializeField]
    private TMP_InputField inputFieldID;
    [SerializeField]
    private Image imagePW;
    [SerializeField]
    private TMP_InputField inputFieldPW;
    [SerializeField]
    private Image imageConfirmPW;
    [SerializeField]
    private TMP_InputField inputFieldConfirmPW;
    [SerializeField]
    private Image imageEmail;
    [SerializeField]
    private TMP_InputField inputFieldEmail;

    [SerializeField]
    private Button btnRegisterAccount;

    /// <summary>
    /// "���� ����"��ư�� ������ �� ȣ��
    /// </summary>
    public void OnClickRegisterAccount()
    {
        // �Ű������� �Է��� InputField UI�� ����� Message ���� �ʱ�ȭ
        ResetUI(imageID, imagePW, imageConfirmPW, imageEmail);
        // �ʵ� ���� ����ִ��� üũ
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "��й�ȣ")) return;
        if (IsFieldDataEmpty(imageConfirmPW, inputFieldConfirmPW.text, "��й�ȣ Ȯ��")) return;
        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "���� �ּ�")) return;

        // ��й�ȣ�� ��й�ȣ Ȯ���� ������ �ٸ� ��
        if (!inputFieldPW.text.Equals(inputFieldConfirmPW.text))
        {
            GuideForIncorrectlyEnteredData(imageConfirmPW, "��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
            return;
        }

        if (!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncorrectlyEnteredData(imageEmail, "���� ������ �߸��Ǿ����ϴ�.");
            return;
        }

        btnRegisterAccount.interactable = false;
        SetMessage("���� �������Դϴ�...");

        // �ڳ� ���� ���� ���� �õ�
        CustomSignUp();
    }

    /// <summary>
    /// ���� ���� �õ� �� �����κ��� ���޹��� message�� ������� ���� ó��
    /// </summary>
    private void CustomSignUp()
    {
        Backend.BMember.CustomSignUp(inputFieldID.text, inputFieldPW.text, callback =>
        {
            btnRegisterAccount.interactable = true;

            //���� ���� ����
            if (callback.IsSuccess())
            {
                // E-mail ���� ������Ʈ
                Backend.BMember.UpdateCustomEmail(inputFieldEmail.text, callback =>
                {
                    if (callback.IsSuccess())
                    {
                        SetMessage($"���� ���� ����. {inputFieldID.text}�� ȯ���մϴ�.");

                        // ���� ������ �������� �� �ش� ������ ���� ���� ����
                        BackendGameData.Instance.GameDataInsert();

                        Utils.LoadScene(SceneNames.HOME);
                    }
                });
            }
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 409:
                        message = "�̹� �����ϴ� ���̵��Դϴ�.";
                        break;
                    case 403:
                    case 401:
                    case 400:
                    default:
                        message = callback.GetMessage();
                        break;
                }

                if (message.Contains("���̵�"))
                {
                    GuideForIncorrectlyEnteredData(imageID, message);
                }
                else
                {
                    SetMessage(message);
                }
            }
        });
    }
}
