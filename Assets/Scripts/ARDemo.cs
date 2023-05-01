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
	private ARRaycastManager _raycastManager;
	[SerializeField]
    private GameObject[] _prefabsToSpawn;
    [SerializeField]
    private Camera _arCamera;
    private UIController _uiController;

    private List<ARTrackedImage> _images = new List<ARTrackedImage>();
    private Dictionary<ARTrackedImage, GameObject> _imageToObjectMapping = new Dictionary<ARTrackedImage, GameObject>();
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();


    void Awake()
    {
        _trackedImageManager.trackedImagesChanged += OnImagesChanged;
        ARCameraManager camManager = _arCamera.GetComponent<ARCameraManager>();
        camManager.autoFocusRequested = true;
        _uiController = GetComponent<UIController>();
    }

    void OnDestroy()
    {
        _trackedImageManager.trackedImagesChanged -= OnImagesChanged;
    }
	

    public void AddImage(Texture2D newImage)
    {
        if (_trackedImageManager.descriptor != null && !_trackedImageManager.descriptor.supportsMutableLibrary)
        {
            _uiController.ShowDebugText("Descriptor: mutable libraries not supported");
            return;
        }
        if (_trackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
        {
            _uiController.ShowDebugText("Adding image", 0.5f);
            mutableLibrary.ScheduleAddImageWithValidationJob(newImage, DateTime.Now.ToString(), 0.25f);
        }
        else
        {
            _uiController.ShowDebugText("Library not mutable");
        }
    }

    void OnImagesChanged(ARTrackedImagesChangedEventArgs args)
	{
        foreach (ARTrackedImage addedImage in args.added)
		{
            _images.Add(addedImage);
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

        foreach(ARTrackedImage image in _images)
        {
            if(!_imageToObjectMapping.ContainsKey(image))
            {
                Vector3 normal = Vector3.up;
                Ray ray = new Ray(_arCamera.transform.position, image.transform.position - _arCamera.transform.position);
                if (_raycastManager.Raycast(ray, _hits, TrackableType.Image))
                {
                    if (_hits.Count > 0)
                    {
                        normal = _hits[0].pose.forward;
                        _hits.Clear();
                    }
                }

                System.Random r = new System.Random();
                int randomIndex = r.Next(0, _prefabsToSpawn.Count());
                GameObject toSpawn = Instantiate(_prefabsToSpawn[randomIndex]);
                toSpawn.transform.SetParent(image.transform);
                toSpawn.transform.localPosition = Vector3.zero;
                toSpawn.transform.rotation = Quaternion.Euler(normal);
                _imageToObjectMapping[image] = toSpawn;
            }
        }
    }
#endif
}
