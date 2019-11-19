using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    private Touch touch;
    float deltaTime = 0.0f;
    public bool active;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        CheckTouch();
    }

    void CheckTouch()
    {
        if (Input.touchCount == 2)
        {
            touch = Input.GetTouch(1); // second finger
            if (touch.phase == TouchPhase.Began)
            {
                active = !active;
            }
        }
    }

    void OnGUI()
    {
        if (active)
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 6 / 100;
            style.normal.textColor = Color.red;
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }
    }
}