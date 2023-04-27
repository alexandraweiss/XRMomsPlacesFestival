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

    private void Awake()
    {
        _demoScript = GetComponent<ARDemo>();    
        _screenshotTex = new RenderTexture(Screen.width, Screen.height, 24);
    }

    public void TakeScreenshot()
    {
        _debugText.text = "Taking screenshot";

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
}
