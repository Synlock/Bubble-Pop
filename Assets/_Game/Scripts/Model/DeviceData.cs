using UnityEngine;

class ScreenDimensions
{
    public static Vector2 GetScreenDimensions(int divideWidth = 1, int divideHeight = 1)
    {
        if (divideWidth <= 0)
            divideWidth = 1;

        if (divideHeight <= 0)
            divideHeight = 1;

        float width = Screen.width / divideWidth;
        float height = Screen.height / divideHeight;
        Vector2 screenDimensions = new Vector2(width, height);

        return screenDimensions;
    }
}