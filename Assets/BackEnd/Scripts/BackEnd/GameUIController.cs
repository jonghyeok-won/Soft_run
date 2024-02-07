using UnityEngine;
using TMPro;

public class GameUIController : MonoBehaviour
{
    [SerializeField]
    private DataManager dataManager;

    [Header("Game Over")]
    [SerializeField]
    private GameObject panelGameOver;
    [SerializeField]
    private TextMeshProUGUI textResultScore;

    [Header("Game Over UI Animation")]
    [SerializeField]
    private CountingEffect effectResultScore;

    [Header("Game Clear")]
    [SerializeField]
    private GameObject panelGameClear;
    [SerializeField]
    private TextMeshProUGUI textClearResultScore;

    [Header("Game Clear UI Animation")]
    [SerializeField]
    private CountingEffect effectClearScore;

    public void onGameOver()
    {
        panelGameOver.SetActive(true);
        textResultScore.text = dataManager.Score.ToString();
        Debug.Log("¿Ã∆—∆Æ Ω««‡");
        effectResultScore.Play(0, dataManager.Score);
    }

    public void onGameClear()
    {
        panelGameClear.SetActive(true);
        textClearResultScore.text = dataManager.Score.ToString();
        effectClearScore.Play(0, dataManager.Score);
    }

    public void BtnClickGoToLobby()
    {
        Utils.LoadScene(SceneNames.HOME);
    }
}
