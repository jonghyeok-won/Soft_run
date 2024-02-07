using UnityEngine;
using UnityEngine.Diagnostics;

public class LogoScenario : MonoBehaviour
{
    [SerializeField]
    private Progress progress;
    [SerializeField]
    private SceneNames nextScene;

    private void Awake()
    {
        SystemSetup();
    }

    private void SystemSetup()
    {
        //Ȱ��ȭ���� ���� ���¿����� ������ ��� ����
        Application.runInBackground = true;

        //�ػ� ����(9:18.5, 1440 * 2960)
        int width = 1920;
        int height = 1080;
        Screen.SetResolution(width, height, true);


        // ȭ���� ������ �ʵ��� ����
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // �ε� �ִϸ��̼� ����, ��� �Ϸ�� OnAfterProgress() �޼ҵ� ����
        progress.Play(OnAfterProgress);
    }

    private void OnAfterProgress()
    {
        Utils.LoadScene(nextScene);
    }
}
