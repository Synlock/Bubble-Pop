

[System.Serializable]
class PlayerData
{
    int score;

    public int GetScore() => score;
    public void SetScore(int newScore) => score = newScore;

    public PlayerData(int score)
    {
        this.score = score;
    }
}