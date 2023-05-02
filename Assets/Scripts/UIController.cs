using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(ARDemo))]
public class UIController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private TextMeshProUGUI _debugText;

    private ARDemo _demoScript;
    private RenderTexture _screenshotTex;
    private RenderTexture _camTex;

    private bool _showDebugText = false;
    private float _debugTextEndTime;

    private void Awake()
    {
        _demoScript = GetComponent<ARDemo>();    
        _screenshotTex = new RenderTexture(Screen.width, Screen.height, 24);
    }

    private void Update()
    {
        if (_showDebugText)
        {
            if (Time.time >= _debugTextEndTime)
            {
                _debugTextEndTime = 0f;
                _showDebugText = false;
                _debugText.text = "";
            }
        }
    }

    public void TakeScreenshot()
    {
        ShowDebugText("Taking screenshot", 1f);

        _camTex = _camera.activeTexture;
        
        _camera.targetTexture = _screenshotTex;
        _camera.Render();
        RenderTexture.active = _screenshotTex;
        Texture2D resultTex = new Texture2D(Screen.width, Screen.height);
        resultTex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        resultTex.Apply();
        _demoScript.AddImage(resultTex);

        _camera.targetTexture = _camTex;
    }

    public void ResetScene()
    {
        _demoScript.ResetScene();
    }

    public void ShowDebugText(string text, float timeout = 3f)
    {
        _showDebugText = true;
        _debugText.text = text;
        _debugTextEndTime = Time.time + timeout;
    }
}
