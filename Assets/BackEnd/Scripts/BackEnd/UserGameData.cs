[System.Serializable]
public class UserGameData
{
    public int gold;
    public int level;
    public int jewel;
    public float experience;
    public int heart;
    public int dailyBestScore;

    public void Reset()
    {
        gold = 0;
        level = 1;
        jewel = 0;
        experience = 0;
        heart = 30;
        dailyBestScore = 0;
    }
}
