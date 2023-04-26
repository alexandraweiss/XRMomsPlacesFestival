using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARDemo : MonoBehaviour
{
	[SerializeField]
	private ARTrackedImageManager _trackedImageManager;
	
    void Awake()
    {
        _trackedImageManager.trackedImagesChanged += OnImagesChanged;
    }

    void OnDestroy()
    {
        _trackedImageManager.trackedImagesChanged -= OnImagesChanged;
    }
	
	void OnImagesChanged(ARTrackedImagesChangedEventArgs args)
	{
		foreach(var tracked in args.added)
		{
			Debug.Log(tracked.name);
		}
	}

#if PLATFORM_ANDROID
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            AndroidJavaObject currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call<bool>("moveTaskToBack", true);
        }
    }
#endif
}
