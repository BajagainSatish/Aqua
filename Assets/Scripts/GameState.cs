using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameState;

public class GameState : MonoBehaviour
{
    private float strategyTime;
    private float gameTime;
    public enum CurrentGameState
    {
        StrategyTimeP1, StrategyTimeP2, CommonPlayTime, None
    }
    public CurrentGameState currentGameState;

    [SerializeField] private Timer timerScript;
    [SerializeField] private PlayerTurnSystem playerTurnSystemScript;
    [SerializeField] private CameraControlRuntime cameraControlRuntime;

    private bool hasAssignedTime;//flag is used to prevent repeatedly assigning the time in each frame
    private float waitForCameraSwitch;

    private void Awake()
    {
        strategyTime = SetParameters.StrategyTime;
        gameTime = SetParameters.CommonGameTime;
        waitForCameraSwitch = SetParameters.CameraSwitchDuration;
    }
    private void Start()
    {
        hasAssignedTime = false;
        currentGameState = CurrentGameState.None;
        cameraControlRuntime.SetCameraToDefaultPosition();
    }
    private void Update()
    {
        // Add code to check whether the main building of either player has been destroyed or not, then only proceed forward

        switch (currentGameState)
        {
            case CurrentGameState.StrategyTimeP1:
                if (!hasAssignedTime)
                {
                    cameraControlRuntime.ResetTempBoolForCameraChange();
                    playerTurnSystemScript.SetTurnToPlayer1();//added in this block as it will execute just once, better performance

                    timerScript.RemainingTime = strategyTime;
                    hasAssignedTime = true;
                }
                cameraControlRuntime.SetCameraToRegion1Position();
                StartCoroutine(StrategyTimeP1());
                break;

            case CurrentGameState.StrategyTimeP2:
                if (!hasAssignedTime)
                {
                    cameraControlRuntime.ResetTempBoolForCameraChange();
                    playerTurnSystemScript.SetTurnToPlayer2();

                    timerScript.RemainingTime = strategyTime;
                    hasAssignedTime = true;
                }
                cameraControlRuntime.SetCameraToRegion2Position();
                StartCoroutine(StrategyTimeP2());
                break;

            case CurrentGameState.CommonPlayTime:
                if (!hasAssignedTime)
                {
                    cameraControlRuntime.ResetTempBoolForCameraChange();
                    playerTurnSystemScript.SetTurnToGameTime();

                    timerScript.RemainingTime = gameTime;
                    hasAssignedTime = true;
                }
                cameraControlRuntime.SetCameraToDefaultPosition();
                StartCoroutine(GameTime());
                break;
            case CurrentGameState.None:
                playerTurnSystemScript.SetPlayerTurnToNone();
                break;
        }
    }
    private IEnumerator StrategyTimeP1()
    {
        yield return new WaitForSeconds(strategyTime);
        print("Player 1's strategy time is over");

        if (currentGameState == CurrentGameState.StrategyTimeP1)
        {
            currentGameState = CurrentGameState.StrategyTimeP2;
            hasAssignedTime = false;
        }
    }
    private IEnumerator StrategyTimeP2()
    {
        yield return new WaitForSeconds(strategyTime);
        print("Player 2's strategy time is over");

        if (currentGameState == CurrentGameState.StrategyTimeP2)
        {
            currentGameState = CurrentGameState.CommonPlayTime;
            hasAssignedTime = false;
        }
    }
    private IEnumerator GameTime()
    {
        print("Game time begins!");
        yield return new WaitForSeconds(gameTime);
        print("Game time is over");

        if (currentGameState == CurrentGameState.CommonPlayTime)
        {
            currentGameState = CurrentGameState.None;
            hasAssignedTime = false;
        }
    }
    public void GameStateToP1StrategyTime()//Initially play button is pressed
    {
        cameraControlRuntime.startPosition = cameraControlRuntime.transform.position;
        cameraControlRuntime.firstCameraSwitch = true;
    }
}
