using UnityEngine;

[System.Serializable]
public class MeshData
{
    public int id = 0;
    public GameObject gameObject;
    public Mesh mesh;
    public int price = 500;
    public bool isUnlocked = false;

    public int GetID() => id;
    public GameObject GetGameObject() => gameObject;
    public Mesh GetMesh() => mesh;
    public int GetPrice() => price;
    public bool GetIsUnlocked() => isUnlocked;

    public int SetID(int newID) => id = newID;
    public void SetGameObject(GameObject newGameObject) => gameObject = newGameObject;
    public void SetMesh(Mesh newMesh) => mesh = newMesh;
    public int SetPrice(int newPrice) => price = newPrice;
    public void SetIsUnlocked(bool newIsUnlocked) => isUnlocked = newIsUnlocked;

    public MeshData()
    {
        
    }
}
