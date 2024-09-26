using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private float deltaTime = 0.0f;

    void Update()
    {
        // deltaTime�� ������Ʈ�մϴ�.
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // FPS�� ����մϴ�.
        float fps = 1.0f / deltaTime;

        // ȭ�鿡 ǥ���� ��Ÿ���� �����մϴ�.
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(10, 10, 100, 30);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 24;
        style.normal.textColor = Color.white;

        // FPS ���� ȭ�鿡 ǥ���մϴ�.
        string text = string.Format("{0:0.} FPS", fps);
        GUI.Label(rect, text, style);
    }
}
