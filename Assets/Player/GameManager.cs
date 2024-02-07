using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] NumberImage;
    public Sprite[] Number;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public int currentscore;

    void Update()
    {
        //���� ����
        int temp = DataManager.Instance.score / 100;
        NumberImage[0].GetComponent<Image>().sprite = Number[temp];
        int temp2 = DataManager.Instance.score % 100;
        temp2 = temp2 / 10;
        NumberImage[1].GetComponent<Image>().sprite = Number[temp2];
        int temp3 = DataManager.Instance.score % 10;
        NumberImage[2].GetComponent<Image>().sprite = Number[temp3];
        //ü�� ĭ �ð�ȭ
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < DataManager.Instance.health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < DataManager.Instance.numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        //ü���� 0�� �Ǹ� ���
        if (DataManager.Instance.health <= 0)
        {
            DataManager.Instance.PlayerDie = true;
            currentscore = DataManager.Instance.score;
        }
        //�ڼ� ������ �ð� ����
        if (DataManager.Instance.magnetTimeCurrent > 0)
        {
            DataManager.Instance.magnetTimeCurrent -= 1 * Time.deltaTime;
        }
    }
}
