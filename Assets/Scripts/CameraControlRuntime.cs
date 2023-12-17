using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameState;

public class CameraControlRuntime : MonoBehaviour
{
    [SerializeField] private Vector3 defaultCameraPos = new Vector3(-52, 75, -50);
    [SerializeField] private Vector3 defaultCameraRot = new Vector3(61, 0, 0);

    [SerializeField] private Vector3 region1CameraPos = new Vector3(-68, 38, -53);
    [SerializeField] private Vector3 region1CameraRot = new Vector3(46, -34, 0);

    [SerializeField] private Vector3 region2CameraPos = new Vector3(-50, 27, -52);
    [SerializeField] private Vector3 region2CameraRot = new Vector3(36, 387, 0);

    public Vector3 startPosition;
    private Vector3 endPosition;

    private Quaternion startRotation;
    private Quaternion endRotation;

    private float cameraSwitchDuration;

    private float elapsedTimePos;
    private float elapsedTimeRot;

    private Camera mainCamera;
    private bool cameraIsAtDesignatedTransform;

    public bool firstCameraSwitch;

    [SerializeField] private GameState gameState;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        cameraSwitchDuration = SetParameters.CameraSwitchDuration;
        cameraIsAtDesignatedTransform = false;
        firstCameraSwitch = false;
    }
    private void Start()
    {
        startPosition = defaultCameraPos;
        endPosition = region1CameraPos;

        startRotation = Quaternion.Euler(61f, 0, 0);
        endRotation = Quaternion.Euler(46f, -34f, 0);
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
    public void CameraMovement(Vector3 startPosition, Vector3 endPosition)//Camera moves from startposition to endposition in time 3 seconds(cameraSwitchDuration).
    {
        elapsedTimePos += Time.deltaTime;
        float percentageComplete = elapsedTimePos / cameraSwitchDuration;
        transform.position = Vector3.Lerp(startPosition,endPosition,percentageComplete);
    }
    private void Update()
    {
        if (firstCameraSwitch)
        {
            elapsedTimePos += Time.deltaTime;
            float percentageCompletePos = elapsedTimePos/cameraSwitchDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, percentageCompletePos);

            elapsedTimeRot += Time.deltaTime;
            float percentageCompleteRot = Mathf.Clamp01(elapsedTimeRot / cameraSwitchDuration);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, percentageCompleteRot);

            if (VectorApproximatelyEqual(transform.position,endPosition) && VectorApproximatelyEqual(transform.eulerAngles, endRotation.eulerAngles))
            {
                print("Reached region 1 position and rotation.");
                gameState.currentGameState = CurrentGameState.StrategyTimeP1;//maybe better to create a method and return true or false
                firstCameraSwitch = false;
            }
        }
    }
    private bool VectorApproximatelyEqual(Vector3 a, Vector3 b)
    {
        return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
    }
}
