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
    [SerializeField] private Vector3 region2CameraRot = new Vector3(36, 27, 0);

    public Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;

    public Quaternion startRotation;
    public Quaternion endRotation;

    private float cameraSwitchDuration;

    private float elapsedTimePos;
    private float elapsedTimeRot;

    private Camera mainCamera;

    public bool firstCameraSwitch;
    public bool secondCameraSwitch;
    public bool thirdCameraSwitch;

    private bool dontAccessCameraDuringSwitchView;
    public bool DontAccessCameraDuringSwitchView
    {
        get { return dontAccessCameraDuringSwitchView; }
    }

    [SerializeField] private GameState gameState;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        cameraSwitchDuration = SetParameters.CameraSwitchDuration;
        firstCameraSwitch = false;
        secondCameraSwitch = false;
        thirdCameraSwitch = false;
        dontAccessCameraDuringSwitchView = false;
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
        mainCamera.transform.position = defaultCameraPos;
        mainCamera.transform.eulerAngles = defaultCameraRot;
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
            dontAccessCameraDuringSwitchView = true;

            elapsedTimePos += Time.deltaTime;
            float percentageCompletePos = elapsedTimePos/cameraSwitchDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, percentageCompletePos);

            elapsedTimeRot += Time.deltaTime;
            float percentageCompleteRot = Mathf.Clamp01(elapsedTimeRot / cameraSwitchDuration);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, percentageCompleteRot);

            if (VectorApproximatelyEqual(transform.position,endPosition) && QuaternionApproximatelyEqual(transform.rotation, endRotation))
            {
                print("Reached region 1 position and rotation.");
                gameState.currentGameState = CurrentGameState.StrategyTimeP1;//maybe better to create a method and return true or false
                firstCameraSwitch = false;
                elapsedTimePos = 0;
                elapsedTimeRot = 0;//research if this assigning to 0 is neccessary or not.

                dontAccessCameraDuringSwitchView = false;

                endPosition = region2CameraPos;
                endRotation = Quaternion.Euler(36, 27, 0);
            }
        }
        else if (secondCameraSwitch)
        {
            dontAccessCameraDuringSwitchView = true;

            elapsedTimePos += Time.deltaTime;
            float percentageCompletePos = elapsedTimePos / cameraSwitchDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, percentageCompletePos);

            elapsedTimeRot += Time.deltaTime;
            float percentageCompleteRot = Mathf.Clamp01(elapsedTimeRot / cameraSwitchDuration);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, percentageCompleteRot);

            if (VectorApproximatelyEqual(transform.position, endPosition) && QuaternionApproximatelyEqual(transform.rotation, endRotation))
            {
                print("Reached region 2 position and rotation.");
                gameState.currentGameState = CurrentGameState.StrategyTimeP2;//maybe better to create a method and return true or false
                secondCameraSwitch = false;
                elapsedTimePos = 0;
                elapsedTimeRot = 0;//research if this assigning to 0 is neccessary or not.

                dontAccessCameraDuringSwitchView = false;

                endPosition = defaultCameraPos;
                endRotation = Quaternion.Euler(61, 0, 0);
            }
        }
        else if (thirdCameraSwitch)
        {
            dontAccessCameraDuringSwitchView = true;

            elapsedTimePos += Time.deltaTime;
            float percentageCompletePos = elapsedTimePos / cameraSwitchDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, percentageCompletePos);

            elapsedTimeRot += Time.deltaTime;
            float percentageCompleteRot = Mathf.Clamp01(elapsedTimeRot / cameraSwitchDuration);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, percentageCompleteRot);

            if (VectorApproximatelyEqual(transform.position, endPosition) && QuaternionApproximatelyEqual(transform.rotation, endRotation))
            {
                print("Reached default position and rotation.");
                gameState.currentGameState = CurrentGameState.CommonPlayTime;//maybe better to create a method and return true or false
                thirdCameraSwitch = false;
                elapsedTimePos = 0;
                elapsedTimeRot = 0;//research if this assigning to 0 is neccessary or not.

                dontAccessCameraDuringSwitchView = false;
            }
        }
    }
    private bool VectorApproximatelyEqual(Vector3 a, Vector3 b)
    {
        return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
    }
    private bool QuaternionApproximatelyEqual(Quaternion a, Quaternion b)
    {
        return Quaternion.Angle(a, b) < 0.01f; // Threshold
    }
}
