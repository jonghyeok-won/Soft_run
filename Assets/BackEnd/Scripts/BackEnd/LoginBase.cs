using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginBase : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMessage;

    /// <summary>
    /// �޼��� ����, InputField ���� �ʱ�ȭ
    /// </summary>
    /// <param name="images"></param>
    protected void ResetUI(params Image[] images)
    {
        textMessage.text = string.Empty;

        for (int i = 0; i < images.Length; ++i)
        {
            images[i].color = Color.white;
        }
    }

    //�Ű������� �ִ� ������ ���
    protected void SetMessage(string msg)
    {
        textMessage.text = msg;
    }

    /// <summary>
    /// �Է� ��ᰡ �ִ� InputField�� ���� ����
    /// ������ ���� �޽��� ���
    /// </summary>
    protected void GuideForIncorrectlyEnteredData(Image image, string msg)
    {
        textMessage.text = msg;
        image.color = Color.red;
    }

    /// <summary>
    /// �ʵ� ���� ����ִ��� Ȯ��(image: �ʵ�, field: ����, result: ��µ� ����)
    /// </summary>
    protected bool IsFieldDataEmpty(Image image, string field, string result)
    {
        if (field.Trim().Equals(""))
        {
            GuideForIncorrectlyEnteredData(image, $"\"{result}\"�ʵ带 ä���ּ���.");

            return true;
        }

        return false;
    }
}
