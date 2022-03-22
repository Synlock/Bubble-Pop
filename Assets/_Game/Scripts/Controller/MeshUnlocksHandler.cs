using System.Collections.Generic;
using UnityEngine;

public class MeshUnlocksHandler : MonoBehaviour
{
    Transform myTransform;
    PlayerData playerData = new PlayerData();
    public List<MeshData> meshes = new List<MeshData>();

    void Awake()
    {
        PlayerData.LoadPlayerData(playerData);
        InitMeshIcons();
    }

    void InitMeshIcons()
    {
        myTransform = transform;

        for (int i = 0; i < myTransform.childCount; i++)
        {
            MeshButton meshBtn = myTransform.GetChild(i).gameObject.GetComponent<MeshButton>();
            meshBtn.meshData.SetID(i);
            meshBtn.meshData.SetGameObject(meshBtn.gameObject);

            if(playerData.unlockedMeshes.Count <= i)
                playerData.unlockedMeshes.Add(meshBtn.meshData);
        }
        PlayerData.SavePlayerData(playerData);
    }
}
