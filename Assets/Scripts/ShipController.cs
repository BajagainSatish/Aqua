using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GameState;

public class ShipController : MonoBehaviour
{
    [SerializeField] private Transform targetBuilding;
    private float shipSpeed;

    private ShipCategorizer_Player shipCategorizer_Player;
    private ShipCategorizer_Level shipCategorizer_Level;
    private TargetingSystem_PhysicsOverlapSphere targetingSystem_PhysicsOverlapSphere;
    private GameState gameState;

    private NavMeshAgent navMeshAgent;
    private int shipLevel;
    private bool isP1Ship;

    private bool _shipOrBuildingInRange;

    private void Awake()
    {
        shipCategorizer_Player = GetComponent<ShipCategorizer_Player>();
        shipCategorizer_Level = GetComponent<ShipCategorizer_Level>();
        targetingSystem_PhysicsOverlapSphere = GetComponent<TargetingSystem_PhysicsOverlapSphere>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        GameObject gameStateManager = GameObject.Find("GameStateManager");
        if (gameStateManager != null)
        {
            gameState = gameStateManager.GetComponent<GameState>();
        }

        string shipLevelString = shipCategorizer_Level.shipLevel.ToString();
        if (shipLevelString == "Level1")
        {
            shipLevel = 1;
        }
        else if (shipLevelString == "Level2")
        {
            shipLevel = 2;
        }
        else if (shipLevelString == "Level3")
        {
            shipLevel = 3;
        }
        else if (shipLevelString == "Level4")
        {
            shipLevel = 4;
        }
        shipSpeed = SetParameters.ShipSpeed[shipLevel - 1];
        isP1Ship = shipCategorizer_Player.isP1Ship;
    }
    private void Update()
    {
        AssignSuitableEnemyAsTarget();
        CurrentGameState currentGameState = gameState.currentGameState;

        if (currentGameState == CurrentGameState.CommonPlayTime)
        {
            _shipOrBuildingInRange = targetingSystem_PhysicsOverlapSphere.ShipOrBuildingIsInRange;

            if (_shipOrBuildingInRange)
            {
                navMeshAgent.speed = 0;
                navMeshAgent.isStopped = true;  // Stop the agent               
            }
            else
            {
                navMeshAgent.speed = shipSpeed;
                navMeshAgent.destination = targetBuilding.transform.position;
                navMeshAgent.isStopped = false;
            }
        }
    }
    private void AssignSuitableEnemyAsTarget()
    {
        if (targetBuilding == null)
        {
            if (isP1Ship)
            {
                GameObject region2 = GameObject.Find("Region2");
                GameObject buildings = region2.transform.GetChild(0).gameObject;
                targetBuilding = buildings.transform.GetChild(0).transform;
            }
            else
            {
                GameObject region1 = GameObject.Find("Region1");
                GameObject buildings = region1.transform.GetChild(0).gameObject;
                targetBuilding = buildings.transform.GetChild(0).transform;
            }
        }    
    }
}
