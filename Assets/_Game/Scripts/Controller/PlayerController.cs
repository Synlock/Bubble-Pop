using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static int score;

    [SerializeField] LayerMask layer;

    Camera cam;
    Action OnMousePressed;

    public static int GetScore() => score;
    public static void SetScore(int newScore) => score = newScore;

    void Start()
    {
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
            ClickToPopBubble(cam);
        }
    }

    void ClickToPopBubble(Camera cam)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, layer))
        {
            var bubble = hit.collider.GetComponent<Bubble>();

            score += bubble.GetBubbleData().GetScore();

            BubbleController.OnBubblePopped.Invoke(bubble.transform.position);
            bubble.gameObject.SetActive(false);
        }
    }
}