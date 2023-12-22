using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShipCategorizer_Level;
using static ShipCategorizer_Size;
public class ShipScoreSystem : MonoBehaviour
{
    private ShipCategorizer_Player shipCategorizerPlayer;
    private ShipCategorizer_Level shipCategorizerLevel;
    private ShipCategorizer_Size shipCategorizerSize;
    [SerializeField] private ScoreSystem scoreSystem;

    private GameObject canvasObject;

    private bool _isP1Ship;
    private bool _shipMenAreAlive;

    private ShipLevels _currentShipLevel;

    private int totalShipValueForScoreSystem;
    private int sizeBasedScore;
    private int levelAndTypeBasedScore;

    private bool scoreAlreadyUpdated;//flag to prevent repeatedly update of score inside update method

    private void Awake()
    {
        shipCategorizerPlayer = GetComponent<ShipCategorizer_Player>();
        shipCategorizerLevel = GetComponent<ShipCategorizer_Level>();
        shipCategorizerSize = GetComponent<ShipCategorizer_Size>();
        scoreAlreadyUpdated = false;
    }
    private void Start()
    {
        EvaluateValueBasedOnShipSize();
        FindPlayerScoreSystem();
    }
    private void Update()
    {
        _currentShipLevel = shipCategorizerLevel.shipLevel;
        EvaluateValueBasedOnShipTypeAndLevel();//level changes during Update()
        EvaluateTotalScore();
        AccessValuesFromOtherScripts();
        OnShipDestroyUpdateEnemyScore();
    }
    private void OnShipDestroyUpdateEnemyScore()
    {
        if (!_shipMenAreAlive && !scoreAlreadyUpdated)
        {
            print("sizebasedvalue: " + sizeBasedScore);
            print("level of ship: " + _currentShipLevel);
            print("level/type based value: " + levelAndTypeBasedScore);
            print("total value of this ship: " + name + totalShipValueForScoreSystem);

            if (_isP1Ship)//Update score of P2
            {
                scoreSystem.IncreasePlayer2Score(totalShipValueForScoreSystem);
            }
            else//Update score of P1
            {
                scoreSystem.IncreasePlayer1Score(totalShipValueForScoreSystem);
            }
            scoreAlreadyUpdated = true;
        }
    }
    private void EvaluateValueBasedOnShipSize()
    {
        if (shipCategorizerSize.shipSize == ShipSize.Small)
        {
            sizeBasedScore = SetParameters.SmallShipDestroyScore;
        }
        else if (shipCategorizerSize.shipSize == ShipSize.Medium)
        {
            sizeBasedScore = SetParameters.MediumShipDestroyScore;
        }
        else if (shipCategorizerSize.shipSize == ShipSize.Large)
        {
            sizeBasedScore = SetParameters.LargeShipDestroyScore;
        }
    }
    private void EvaluateValueBasedOnShipTypeAndLevel()
    {
        if (TryGetComponent<ArcherShoot>(out _))
        {
            levelAndTypeBasedScore = SetParameters.ArcherShipDestroyScore[LevelToInt(_currentShipLevel) - 1];
        }
        else if (TryGetComponent<CannonShoot>(out _))
        {
            levelAndTypeBasedScore = SetParameters.CannonShipDestroyScore[LevelToInt(_currentShipLevel) - 1];
        }
        else if (TryGetComponent<MortarShoot>(out _))
        {
            levelAndTypeBasedScore = SetParameters.MortarShipDestroyScore[LevelToInt(_currentShipLevel) - 1];
        }
        else if (TryGetComponent<GunShoot>(out _))
        {
            levelAndTypeBasedScore = SetParameters.GunmanShipDestroyScore[LevelToInt(_currentShipLevel) - 1];
        }
    }
    private void EvaluateTotalScore()
    {
        totalShipValueForScoreSystem = sizeBasedScore + levelAndTypeBasedScore;
    }
    private int LevelToInt(ShipLevels shipLevel)
    {
        if (shipLevel == ShipLevels.Level1)
        {
            return 1;
        }
        else if (shipLevel == ShipLevels.Level2)
        {
            return 2;
        }
        else if (shipLevel == ShipLevels.Level3)
        {
            return 3;
        }
        else
        {
            return 4;
        }
    }
    private void FindPlayerScoreSystem()
    {
        canvasObject = GameObject.Find("Canvas_Main");
        for (int i = 0; i < canvasObject.transform.childCount; i++)
        {
            GameObject childObject = canvasObject.transform.GetChild(i).gameObject;
            if (childObject.name == "SideMenuParent")
            {
                GameObject sideMenu = childObject.transform.GetChild(0).gameObject;

                if (sideMenu.name == "SideMenu")
                {
                    for (int j = 0; j < sideMenu.transform.childCount; j++)
                    {
                        GameObject scoreSystemGameObject = sideMenu.transform.GetChild(j).gameObject;
                        if (scoreSystemGameObject.name == "PlayerScoreSystem")
                        {
                            scoreSystem = scoreSystemGameObject.GetComponent<ScoreSystem>();
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Update this script to find player score system!!");
                    Debug.LogWarning("Player Score System not found by ship!!!");
                }
            }
        }
    }
    private void AccessValuesFromOtherScripts()
    {
        _isP1Ship = shipCategorizerPlayer.isP1Ship;
        _shipMenAreAlive = shipCategorizerPlayer.shipMenAreAlive;
    }
}
