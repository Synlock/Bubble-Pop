[System.Serializable]
class PlayerData
{
    int score;
    int currentLevel;

    public int GetScore() => score;
    public int GetCurrentLevel() => currentLevel;


    public void SetScore(int newScore) => score = newScore;
    public void SetCurrentLevel(int newCurrentLevel) => currentLevel = newCurrentLevel;

    public PlayerData(int score, int currentLevel)
    {
        this.score = score;
        this.currentLevel = currentLevel;
    }
}