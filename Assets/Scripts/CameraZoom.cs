using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private float zoomSpeed;
    private float minZoomFOV;
    private float maxZoomFOV;

    private float previousBetweenFingerDistance;  //holds distance between two fingers used for zooming in previous frame

    private bool isZooming;
    public bool IsZooming() { return isZooming; }

    private CameraControlRuntime cameraControlRuntime;
    private void Awake()
    {
        cameraControlRuntime = GetComponent<CameraControlRuntime>();

        zoomSpeed = SetParameters.ZoomSpeed;
        minZoomFOV = SetParameters.MinZoomFOV;
        maxZoomFOV = SetParameters.MaxZoomFOV;
    }

    private void Update()
    {
        bool dontAccessCamera = cameraControlRuntime.DontAccessCameraDuringSwitchView;
        if (!dontAccessCamera)
        {
            isZooming = false;
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);
                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    previousBetweenFingerDistance = (touch1.position - touch2.position).magnitude;
                }
                else
                {
                    HandleCameraZoom(touch1, touch2);
                    isZooming = true;
                }
            }
        }   
    }
    private void HandleCameraZoom(Touch touch1, Touch touch2)
    {
        float currentBetweenFingerDistance = (touch1.position - touch2.position).magnitude;
        float deltaFingerDistance = currentBetweenFingerDistance - previousBetweenFingerDistance;

        if (deltaFingerDistance == 0f)
            return;

        float zoomAmount = deltaFingerDistance * zoomSpeed * Time.deltaTime;
        float newFOV = Camera.main.fieldOfView - zoomAmount;

        // Apply restrictions
        newFOV = Mathf.Clamp(newFOV, minZoomFOV, maxZoomFOV);

        // Update the camera's field of view to simulate zooming
        Camera.main.fieldOfView = newFOV;

        previousBetweenFingerDistance = currentBetweenFingerDistance;
    }
}
