[System.Serializable]
public class PlayerData
{
    public int level;

    public int GetLevel() => level;

    public void SetLevel(int newLevel) => level = newLevel;

    public PlayerData()
    {

    }
    public PlayerData(int level)
    {
        this.level = level;
    }
}