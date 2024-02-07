using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public bool PlayerDie = false;
    public bool PlayerClear = false;
    public bool ability = false;
    public GameObject GameController;

    public int score = 0;
    public int plusScore = 1;

    public int Score
    {
        set => score = Mathf.Max(0, value);
        get => score;
    }
    
    public int health;
    public int numOfHearts;

    public float magnetTimeCurrent = 0f;
    public float magnetTimeMax = 5f;
    public float itemMoveSpeed = 15f;

    public float abilityGageCurrent = 0f;
    public float abilityGageMax = 10f;

    public float GageCurrent = 0f;
    public float GageMax = 3f;

    [SerializeField]
    private GameController gameController;

    private void Update()
    {
        if(PlayerDie)
        {
            Debug.Log("1");
            Die();
        }
        if(PlayerClear)
        {
            Clear();
        }
    }
    public void SetUp(GameController gameController)
    {
        this.gameController = gameController;
    }
    /*
    public static DataManager Instance
    {
        get
        {
            return instance;
        }
    }*/
    
    private void Awake()
    {
        /*health = 3;
        numOfHearts = 3;*/
        score = 0;
        
        if (Instance == null)
        {
            //DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
    }
    
    public void Die()
    {
        Debug.Log("2");
        if (gameController != null)
        {
            gameController.GameOver();
        }
        else
        {
            Debug.LogError("GameManager is null!");
        }    
    }

    public void Clear()
    {
        if (gameController != null)
        {
            gameController.GameClear();
        }
        else
        {
            Debug.LogError("GameManager is null!");
        }
    }
    
}
