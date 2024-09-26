using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private float deltaTime = 0.0f;

    void Update()
    {
        // deltaTime을 업데이트합니다.
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // FPS를 계산합니다.
        float fps = 1.0f / deltaTime;

        // 화면에 표시할 스타일을 설정합니다.
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(10, 10, 100, 30);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 24;
        style.normal.textColor = Color.white;

        // FPS 값을 화면에 표시합니다.
        string text = string.Format("{0:0.} FPS", fps);
        GUI.Label(rect, text, style);
    }
}
