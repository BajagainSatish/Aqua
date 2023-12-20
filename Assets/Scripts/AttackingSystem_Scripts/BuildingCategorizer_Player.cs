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
    [SerializeField] private GameOver gameOver;

    private GameObject gameOverGameObject;
    private bool scoreAlreadyUpdated;//flag to prevent repeatedly update of score inside update method

    private bool gameIsOver;

    private void Awake()
    {
        buildingValueForScoreSystem = SetParameters.BuildingDestroyScore;
        buildingIsFunctional = true;
        scoreAlreadyUpdated = false;
        gameIsOver = false;
    }
    private void Start()
    {
        gameOverGameObject = GameObject.Find("GameOver");
        gameOver = gameOverGameObject.GetComponent<GameOver>();
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
        if (buildingIsMainBuilding && !buildingIsFunctional && !gameIsOver)
        {
            gameOver.GameOverDueToMainBuildingDestroy(buildingIsOfP1);           
            gameState.currentGameState = CurrentGameState.GameOver;
            gameIsOver = true;
        }
    }
}
