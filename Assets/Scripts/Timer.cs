using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GameState;

public class Timer : MonoBehaviour
{
    private GameObject gameStateManager;
    private GameState gameState;
    private CurrentGameState _currentGameState;

    [SerializeField] private TextMeshProUGUI timerText;
    private float remainingTime;

    private float timeLimitStrategyForColorToRed;
    private float timeLimitGameForColorToRed;

    public float RemainingTime
    {
        get { return remainingTime; }
        set { remainingTime = value; }
    }
    private void Start()
    {
        gameStateManager = GameObject.Find("GameStateManager");
        gameState = gameStateManager.GetComponent<GameState>();
        timeLimitStrategyForColorToRed = SetParameters.timeLimitForTextToRed_StrategyTime;
        timeLimitGameForColorToRed = SetParameters.timeLimitForTextToRed_GameTime;
    }
    private void Update()
    {
        _currentGameState = gameState.currentGameState;
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}",minutes,seconds);
        ChangeTimeTextColor();
    }
    private void ChangeTimeTextColor()
    {
        if (_currentGameState == CurrentGameState.CommonPlayTime)
        {
            if (remainingTime < timeLimitGameForColorToRed)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.green;
            }
        }
        else if ((_currentGameState == CurrentGameState.StrategyTimeP1) || (_currentGameState == CurrentGameState.StrategyTimeP2))
        {
            if (remainingTime < timeLimitStrategyForColorToRed)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.green;
            }
        }
    }
}
