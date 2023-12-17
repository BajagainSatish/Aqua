using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlRuntime : MonoBehaviour
{
    [SerializeField] private Vector3 defaultCameraPos = new Vector3(-52, 75, -50);
    [SerializeField] private Vector3 defaultCameraRot = new Vector3(61, 0, 0);

    [SerializeField] private Vector3 region1CameraPos = new Vector3(-68, 38, -53);
    [SerializeField] private Vector3 region1CameraRot = new Vector3(46f, 326, 0);

    [SerializeField] private Vector3 region2CameraPos = new Vector3(-50, 27, -52);
    [SerializeField] private Vector3 region2CameraRot = new Vector3(36, 387, 0);

    private Camera mainCamera;

    private bool cameraIsAtDesignatedTransform;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        cameraIsAtDesignatedTransform = false;
    }
    public void SetCameraToDefaultPosition()
    {
        if (!cameraIsAtDesignatedTransform)
        {
            mainCamera.transform.position = defaultCameraPos;
            mainCamera.transform.eulerAngles = defaultCameraRot;
            cameraIsAtDesignatedTransform = true;
        }
    }
    public void SetCameraToRegion1Position()
    {
        if (!cameraIsAtDesignatedTransform)
        {
            mainCamera.transform.position = region1CameraPos;
            mainCamera.transform.eulerAngles = region1CameraRot;
            cameraIsAtDesignatedTransform = true;
        }
    }
    public void SetCameraToRegion2Position()
    {
        if (!cameraIsAtDesignatedTransform)
        {
            mainCamera.transform.position = region2CameraPos;
            mainCamera.transform.eulerAngles = region2CameraRot;
            cameraIsAtDesignatedTransform = true;
        }
    }
    public void ResetTempBoolForCameraChange()
    {
        cameraIsAtDesignatedTransform = false;
    }
}
