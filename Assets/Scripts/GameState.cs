using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    private float strategyTime;
    private float gameTime;
    public enum CurrentGameState
    {
        StrategyTimeP1, StrategyTimeP2, CommonPlayTime, None, GameOver
    }
    public CurrentGameState currentGameState;

    [SerializeField] private Timer timerScript;
    [SerializeField] private PlayerTurnSystem playerTurnSystemScript;
    [SerializeField] private CameraControlRuntime cameraControlRuntime;
    [SerializeField] private GameOver gameOver;
    [SerializeField] private GameObject playButton_DisplayState;

    [SerializeField] private Sprite playButtonSprite;
    [SerializeField] private Sprite strategyTimeP1Sprite;
    [SerializeField] private Sprite strategyTimeP2Sprite;
    [SerializeField] private Sprite gameTimeSprite;

    private Image image;

    private GameObject gameOverGameObject;
    private bool hasAssignedTime;//flag is used to prevent repeatedly assigning the time in each frame
    private bool gameBegin;

    private bool gameIsOver;

    private void Awake()
    {
        strategyTime = SetParameters.StrategyTime;
        gameTime = SetParameters.CommonGameTime;

        image = playButton_DisplayState.GetComponent<Image>();

        gameIsOver = false;
    }
    private void Start()
    {
        gameBegin = false;
        hasAssignedTime = false;
        currentGameState = CurrentGameState.None;
        cameraControlRuntime.SetCameraToDefaultPosition();

        gameOverGameObject = GameObject.Find("GameOver");
        gameOver = gameOverGameObject.GetComponent<GameOver>();

        image.sprite = playButtonSprite;
    }
    public void GameStateToP1StrategyTime()//Initially play button is pressed
    {
        if (!gameBegin)
        {
            cameraControlRuntime.startPosition = cameraControlRuntime.transform.position;
            Vector3 rotationEuler = cameraControlRuntime.transform.eulerAngles;
            cameraControlRuntime.startRotation = Quaternion.Euler(rotationEuler);
            cameraControlRuntime.firstCameraSwitch = true;
            playButton_DisplayState.SetActive(false);
            gameBegin = true;
        }
    }
    private void Update()
    {
        // Add code to check whether the main building of either player has been destroyed or not, then only proceed forward
        switch (currentGameState)
        {
            case CurrentGameState.StrategyTimeP1:
                if (!hasAssignedTime)
                {
                    playerTurnSystemScript.SetTurnToPlayer1();//added in this block as it will execute just once, better performance

                    playButton_DisplayState.SetActive(true);
                    image.sprite = strategyTimeP1Sprite;

                    timerScript.RemainingTime = strategyTime;//flag is used to prevent repeatedly assigning the time in each frame
                    hasAssignedTime = true;
                }
                StartCoroutine(StrategyTimeP1());
                break;

            case CurrentGameState.StrategyTimeP2:
                if (!hasAssignedTime)
                {
                    playerTurnSystemScript.SetTurnToPlayer2();

                    playButton_DisplayState.SetActive(true);
                    image.sprite = strategyTimeP2Sprite;

                    timerScript.RemainingTime = strategyTime;
                    hasAssignedTime = true;
                }
                StartCoroutine(StrategyTimeP2());
                break;

            case CurrentGameState.CommonPlayTime:
                if (!hasAssignedTime)
                {
                    playerTurnSystemScript.SetTurnToGameTime();

                    playButton_DisplayState.SetActive(true);
                    image.sprite = gameTimeSprite;

                    timerScript.RemainingTime = gameTime;
                    hasAssignedTime = true;
                }
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
        //print("Player 1's strategy time is over");

        if (currentGameState == CurrentGameState.StrategyTimeP1)
        {
            //currentGameState = CurrentGameState.StrategyTimeP2;//This value controlled from CameraControlRuntime
            hasAssignedTime = false;

            cameraControlRuntime.startPosition = cameraControlRuntime.transform.position;
            Vector3 rotationEuler = cameraControlRuntime.transform.eulerAngles;
            cameraControlRuntime.startRotation = Quaternion.Euler(rotationEuler);
            cameraControlRuntime.secondCameraSwitch = true;

            currentGameState = CurrentGameState.None;//necessary to prevent startPosition from being updated during runtime

            playButton_DisplayState.SetActive(false);
            //print("This block should execute just once!!!");
        }
    }
    private IEnumerator StrategyTimeP2()
    {
        yield return new WaitForSeconds(strategyTime);
        //print("Player 2's strategy time is over");

        if (currentGameState == CurrentGameState.StrategyTimeP2)
        {
            hasAssignedTime = false;

            cameraControlRuntime.startPosition = cameraControlRuntime.transform.position;
            Vector3 rotationEuler = cameraControlRuntime.transform.eulerAngles;
            cameraControlRuntime.startRotation = Quaternion.Euler(rotationEuler);
            cameraControlRuntime.thirdCameraSwitch = true;

            currentGameState = CurrentGameState.None;

            playButton_DisplayState.SetActive(false);
            //print("This block should execute just once!!!");
        }
    }
    private IEnumerator GameTime()
    {
        //print("Game time begins!");
        yield return new WaitForSeconds(gameTime);
        //print("Game time is over");

        if (currentGameState == CurrentGameState.CommonPlayTime)
        {
            GameOverCondition();
            currentGameState = CurrentGameState.None;
            hasAssignedTime = false;
        }
    }
    private void GameOverCondition()
    {
        if (!gameIsOver)
        {
            gameOver.GameOverDueToTimeOver();
            currentGameState = CurrentGameState.GameOver;
            gameIsOver = true;
        }
    }
}
