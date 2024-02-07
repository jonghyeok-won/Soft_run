using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class Login : LoginBase
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
    private Button btnLogin;

    /// <summary>
    /// "�α���"���p ������ �� ȣ��
    /// </summary>
    public void OnClickLogin()
    {
        // �Ű������� �Է��� InputField Ui�� ����� message ���� �ʱ�ȭ
        ResetUI(imageID, imagePW);

        // �ʵ� ���� ����ִ��� üũ
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "��й�ȣ")) return;

        // �α��� ��ư�� ��Ÿ���� ���ϵ��� ��ȣ�ۿ� ��Ȱ��ȭ
        btnLogin.interactable = false;

        // ������ �α����� ��û�ϴ� ���� ȭ�鿡 ��µǴ� ���� ������Ʈ
        StartCoroutine(nameof(LoginProcess));

        // �ڳ� ���� �α��� �õ�
        ResponseToLogin(inputFieldID.text, inputFieldPW.text);
    }


    /// <summary>
    /// �α��� �õ� �� �����κ��� ���޹��� message�� ������� ���� ó��
    /// </summary>
    private void ResponseToLogin(string ID, string PW)
    {
        // ������ �α��� ��û (�񵿱�)
        Backend.BMember.CustomLogin(ID, PW, callback =>
        {
            StopCoroutine(nameof(LoginProcess));

            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldID.text}�� ȯ���մϴ�");

                Utils.LoadScene(SceneNames.HOME);
            }
            else
            {
                btnLogin.interactable = true;

                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 401:
                        message = callback.GetMessage().Contains("customId") ? "�������� �ʴ� ���̵��Դϴ�" : "�߸��� ��й�ȣ �Դϴ�.";
                        break;
                    case 403:
                        message = callback.GetMessage().Contains("user") ? "���ܴ��� �����Դϴ�." : "���ܴ��� ����̽��Դϴ�.";
                        break;
                    case 410:
                        message = "Ż�� �������� �����Դϴ�.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                if (message.Contains("��к�ȣ"))
                {
                    GuideForIncorrectlyEnteredData(imagePW, message);
                }
                else
                {
                    GuideForIncorrectlyEnteredData(imageID, message);
                }
            }
        });
    }

    private IEnumerator LoginProcess()
    {
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;

            SetMessage($"�α��� ���Դϴ�... {time:F1}");

            yield return null;
        }
    }
}
