using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameState;
public class BuildingCategorizer_Player : MonoBehaviour
{
    public bool buildingIsOfP1;
    public bool buildingIsFunctional;
    public bool buildingIsMainBuilding;
    private int buildingValueForScoreSystem;

    [SerializeField] private GameState gameState;
    [SerializeField] private ScoreSystem scoreSystem;
    private bool scoreAlreadyUpdated;//flag to prevent repeatedly update of score inside update method

    private void Awake()
    {
        buildingValueForScoreSystem = SetParameters.BuildingDestroyScore;
        buildingIsFunctional = true;
        scoreAlreadyUpdated = false;
    }

    private void Update()
    {
        OnBuildingDestroyUpdateEnemyScore();
        GameOverCondition();
    }

    private void OnBuildingDestroyUpdateEnemyScore()
    {
        if (!buildingIsFunctional && !scoreAlreadyUpdated)
        {
            if (buildingIsOfP1)//Update score of P2
            {
                scoreSystem.IncreasePlayer2Score(buildingValueForScoreSystem);
            }
            else//Update score of P1
            {
                scoreSystem.IncreasePlayer1Score(buildingValueForScoreSystem);
            }
            scoreAlreadyUpdated = true;
        }
    }

    private void GameOverCondition()
    {
        if (buildingIsMainBuilding && !buildingIsFunctional)
        {
            if (buildingIsOfP1)
            {
                print("Player 1 loses!");
                print("Player 2 wins!!!");

                //Add condition to stop game/prevent ships from attacking.
            }
            else
            {
                print("Player 2 loses!");
                print("Player 1 wins!!!");
            }
            gameState.currentGameState = CurrentGameState.GameOver; 
        }
    }
}
