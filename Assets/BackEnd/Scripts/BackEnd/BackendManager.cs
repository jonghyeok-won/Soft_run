using UnityEngine;
using BackEnd;

public class BackendManager : MonoBehaviour
{
    private void Awake()
    {
        // Update() �޼ҵ��� Backend.AsyncPoll(); ȣ���� ���� ������Ʈ�� �ı����� �ʴ´�
        DontDestroyOnLoad(gameObject);

        // �ڳ� ���� �ʱ�ȭ
        BackendSetup();
    }

    private void Update()
    {
        if (Backend.IsInitialized)
        {
            Backend.AsyncPoll();
        }
    }

    private void BackendSetup()
    {
        //�ڳ� �ʱ�ȭ
        var bro = Backend.Initialize(true);

        //�ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            //�ʱ�ȭ ������ statusCode 204 Success
            Debug.Log($"�ʱ�ȭ ���� : {bro}");
        }
        else
        {
            //�ʱ�ȭ ���� �� statusCode 400�� ���� �߻�
            Debug.LogError($"�ʱ�ȭ ���� : {bro}");
        }
    }
}
