using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameState;
public class DragShipClassifier : MonoBehaviour
{
    private bool dragShipBelongsToP1;
    public bool DragShipBelongsToP1
    {
        get
        {
            return dragShipBelongsToP1;
        }
    }
    private GameObject gameStateManager;
    private GameState gameState;
    private void Start()
    {
        gameStateManager = GameObject.Find("GameStateManager");
        gameState = gameStateManager.GetComponent<GameState>();
    }
    private void Update()
    {
        DistinguishShipAccordingToGameState();
    }
    private void DistinguishShipAccordingToGameState()
    {
        CurrentGameState _currentGameState = gameState.currentGameState;
        if (_currentGameState == CurrentGameState.StrategyTimeP1)
        {
            dragShipBelongsToP1 = true;
        }
        else if (_currentGameState == CurrentGameState.StrategyTimeP2)
        {
            dragShipBelongsToP1 = false;
        }
        //Ensure in other script that if current state is not strategy time 1 or 2, then ship cannot be instantiated.
        //Else possibility of error generation that, every ship thus generated will instantiate only in region 2 and not region 1.
    }
}
