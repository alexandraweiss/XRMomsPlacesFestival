using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARDemo : MonoBehaviour
{
	[SerializeField]
	private ARTrackedImageManager _trackedImageManager;
	[SerializeField]
    private GameObject[] _prefabsToSpawn;
    [SerializeField]
    private TextMeshProUGUI _debugText;

    void Awake()
    {
        _trackedImageManager.trackedImagesChanged += OnImagesChanged;
    }

    void OnDestroy()
    {
        _trackedImageManager.trackedImagesChanged -= OnImagesChanged;
    }
	
    void AddImage(Texture2D newImage)
    {
        if (!_trackedImageManager.descriptor.supportsMutableLibrary)
        {
            
        }
        if (_trackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
        {
            mutableLibrary.ScheduleAddImageWithValidationJob(newImage, DateTime.Now.ToString(), 0.3f);
        }
    }

	void OnImagesChanged(ARTrackedImagesChangedEventArgs args)
	{
		foreach(ARTrackedImage tracked in args.added)
		{
            System.Random r = new System.Random();
            int randomIndex = r.Next(0, _prefabsToSpawn.Count());
            GameObject toSpawn = Instantiate(_prefabsToSpawn[randomIndex]);
            _debugText.text = _prefabsToSpawn[randomIndex].name;
            toSpawn.transform.SetParent(tracked.transform);
            toSpawn.transform.localPosition = Vector3.zero;
            toSpawn.transform.rotation = Quaternion.identity;
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
