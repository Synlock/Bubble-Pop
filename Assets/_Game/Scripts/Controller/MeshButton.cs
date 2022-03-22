using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MeshButton : MonoBehaviour
{
    PlayerData playerData = new PlayerData();
    public MeshData meshData = new MeshData();
    Button button;
    TMP_Text priceText;

    Action OnMeshButtonPress;

    void Awake()
    {
        button = GetComponent<Button>();
        priceText = GetComponentInChildren<TMP_Text>();

        OnMeshButtonPress += OnMeshButtonPressHandler;
    }
    void Start()
    {
        PlayerData.LoadPlayerData(playerData);
        meshData = playerData.unlockedMeshes[meshData.GetID()];

        InitMeshButton();
        button.onClick.AddListener(() =>
        {
            OnMeshButtonPress.Invoke();
        });
    }

    void InitMeshButton()
    {
        MeshData meshData = playerData.unlockedMeshes[this.meshData.GetID()];
        if (meshData.GetIsUnlocked())
        {
            priceText.gameObject.SetActive(false);
        }
        else
        {
            priceText.gameObject.SetActive(true);
        }
    }

    void OnMeshButtonPressHandler()
    {
        MeshData meshData = playerData.unlockedMeshes[this.meshData.GetID()];
        if (meshData.GetIsUnlocked())
        {
            priceText.gameObject.SetActive(false);
            playerData.selectedMesh = meshData.GetMesh();
            PlayerData.SavePlayerData(playerData);
        }
        else
        {
            priceText.gameObject.SetActive(true);
            BuyNewMesh(meshData);
        }
    }

    void BuyNewMesh(MeshData meshData)
    {
        if (playerData.coins >= meshData.GetPrice())
        {
            playerData.coins -= meshData.GetPrice();
            meshData.SetIsUnlocked(true);
            playerData.selectedMesh = meshData.GetMesh();
            priceText.gameObject.SetActive(false);
            PlayerData.SavePlayerData(playerData);
        }
        else
        {
            //print to screen not enough coins
            Debug.Log("Not Enough Coins");
        }
    }
}
