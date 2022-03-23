using System.Collections;
using System.IO;
using UnityEngine;

public class InitPlayerDataOnFirstLoad : MonoBehaviour
{
    PlayerData playerData = new PlayerData();
    [SerializeField] Mesh defaultMesh;
    void Awake()
    {
        PlayerData.LoadPlayerData(playerData);
        if (!playerData.isFirstLoad) return;

        playerData.SetLevel(1);
        playerData.SetCoins(0);
        playerData.unlockedMeshes = new System.Collections.Generic.List<MeshData>();
        playerData.selectedMesh = defaultMesh;
        playerData.isFirstLoad = false;

        StartCoroutine(SaveDelay());
    }
    IEnumerator SaveDelay()
    {
        yield return new WaitForSeconds(1f);

        PlayerData.SavePlayerData(playerData);
    }
}
