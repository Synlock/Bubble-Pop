using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerData playerData;
    int score;
    int currentLevel = 0;

    [SerializeField] LayerMask layer;

    Camera cam;
    Action OnMousePressed;

    public int GetScore() => score;
    public void SetScore(int newScore) => score = newScore;

    void Start()
    {
        playerData = new PlayerData(score, currentLevel);
        cam = Camera.main;

        OnMousePressed += OnMousePressedHandler;
    }
    void Update()
    {
        OnMousePressed.Invoke();
    }
    void OnMousePressedHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickToPopBubble(cam, playerData);
        }
    }

    void ClickToPopBubble(Camera cam, PlayerData playerData)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, layer))
        {
            var bubble = hit.collider.GetComponent<Bubble>();

            score += bubble.GetBubbleData().GetScore();
            playerData.SetScore(score);

            BubbleController.OnBubblePopped.Invoke(bubble.transform.position);
            bubble.gameObject.SetActive(false);
        }
    }
}