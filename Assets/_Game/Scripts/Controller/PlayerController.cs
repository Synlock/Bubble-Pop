using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerData playerData;
    int score;

    [SerializeField] LayerMask layer;

    Camera cam;
    Action OnMousePressed;

    public int GetScore() => score;

    void Start()
    {
        playerData = new PlayerData(score);
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
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, layer))
            {
                Debug.DrawRay(cam.transform.position, ray.direction * 100f, Color.red);
                var bubble = hit.collider.GetComponent<Bubble>();
                score += bubble.GetBubbleData().GetScore();
                playerData.SetScore(score);
                Destroy(bubble.gameObject, 0.5f);
            }
        }
    }
}