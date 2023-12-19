using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameState;
public class BuildingCategorizer_Player : MonoBehaviour
{
    public bool buildingIsOfP1;
    public bool buildingIsFunctional;
    public bool buildingIsMainBuilding;

    [SerializeField] private GameState gameState;

    private void Awake()
    {
        buildingIsFunctional = true;
    }
    private void Update()
    {
        GameOverCondition();
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
