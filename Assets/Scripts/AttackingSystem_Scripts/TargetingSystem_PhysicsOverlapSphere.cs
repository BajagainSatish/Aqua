using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ShipCategorizer_Size;
using static GameState;

public class TargetingSystem_PhysicsOverlapSphere : MonoBehaviour
{
    private ShipCategorizer_Player thisShipCategorizerPlayerScript;
    private ShipCategorizer_Level thisShipCategorizer_LevelScript;
    private ShipCategorizer_Size shipCategorizer_SizeScript;

    private bool isPlayer1;
    private GameObject target;
    private Collider targetCollider;

    private GameObject scaleFactorGameObject;
    private GameObject parentShooterObject;
    private string shooter;

    private GameObject[] shooters;
    private ArcherController[] archerControllerScript;
    private CannonController[] cannonControllerScript;
    private GunmanController[] gunmanControllerScript;
    private MortarController[] mortarControllerScript;

    private Transform shipCenter;
    //public bool testActiveShip;
    private float shipMaxRange;

    private List<Collider> enemyShipsInRange = new List<Collider>();
    private List<Collider> enemyBuildingsInRange = new List<Collider>();

    private int shipMenCount;

    public enum ShipType
    {
        ArcherShip, CannonShip, GunmanShip, MortarShip, SupplyShip
    };
    public ShipType thisShipType;
    private Vector3 myShipPosition;
    private bool thisShipIsFunctional;
    private bool thisShipMenAreAlive;
    private bool thisShipIsCannonOrMortarShip;

    //For purpose of ship rotation towards enemy only
    private ArcherShoot archerShoot;
    private GunShoot gunShoot;
    private CannonShoot cannonShoot;
    private MortarShoot mortarShoot;

    private bool shipOrBuildingIsInRange;
    public bool ShipOrBuildingIsInRange
    {
        get
        {
            return shipOrBuildingIsInRange;
        }
    }
    private GameState gameState;
    private void Awake()
    {
        thisShipCategorizerPlayerScript = GetComponent<ShipCategorizer_Player>();
        thisShipCategorizer_LevelScript = GetComponent<ShipCategorizer_Level>();
        shipCategorizer_SizeScript = GetComponent<ShipCategorizer_Size>();

        ShipSize currentShipSize = DetermineThisShipSize();
        shipMenCount = GetShipMenCount(currentShipSize);

        shooters = new GameObject[shipMenCount];
        archerControllerScript = new ArcherController[shipMenCount];
        cannonControllerScript = new CannonController[shipMenCount];
        gunmanControllerScript = new GunmanController[shipMenCount];
        mortarControllerScript = new MortarController[shipMenCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject gameObject = transform.GetChild(i).gameObject;
            if (gameObject.name == "ScaleFactorGameObject")
            {
                scaleFactorGameObject = gameObject;
            }
        }
        for (int i = 0; i < scaleFactorGameObject.transform.childCount; i++)
        {
            GameObject gameObject = scaleFactorGameObject.transform.GetChild(i).gameObject;
            if (gameObject.name == "Archers" || gameObject.name == "Gunmen" || gameObject.name == "CannonUnit" || gameObject.name == "MortarUnit")
            {
                parentShooterObject = gameObject;
                shooter = parentShooterObject.name;
            }
        }
        if (parentShooterObject != null)
        {
            for (int i = 0; i < parentShooterObject.transform.childCount; i++)
            {
                if (shooter == "Archers")
                {
                    shooters[i] = parentShooterObject.transform.GetChild(i).gameObject;
                    archerControllerScript[i] = shooters[i].GetComponent<ArcherController>();
                }
                else if (shooter == "CannonUnit")
                {
                    shooters[i] = parentShooterObject.transform.GetChild(i).gameObject;
                    cannonControllerScript[i] = shooters[i].GetComponent<CannonController>();
                }
                else if (shooter == "Gunmen")
                {
                    shooters[i] = parentShooterObject.transform.GetChild(i).gameObject;
                    gunmanControllerScript[i] = shooters[i].GetComponent<GunmanController>();
                }
                else if (shooter == "MortarUnit")
                {
                    shooters[i] = parentShooterObject.transform.GetChild(i).gameObject;
                    mortarControllerScript[i] = shooters[i].GetComponent<MortarController>();
                }
            }
        }

        if (TryGetComponent<ArcherShoot>(out _))
        {
            thisShipType = ShipType.ArcherShip;
            thisShipIsCannonOrMortarShip = false;
            archerShoot = GetComponent<ArcherShoot>();
        }
        else if (TryGetComponent<CannonShoot>(out _))
        {
            thisShipType = ShipType.CannonShip;
            thisShipIsCannonOrMortarShip = true;
            cannonShoot = GetComponent<CannonShoot>();
        }
        else if (TryGetComponent<GunShoot>(out _))
        {
            thisShipType = ShipType.GunmanShip;
            thisShipIsCannonOrMortarShip = false;
            gunShoot = GetComponent<GunShoot>();
        }
        else if (TryGetComponent<MortarShoot>(out _))
        {
            thisShipType = ShipType.MortarShip;
            thisShipIsCannonOrMortarShip = true;
            mortarShoot = GetComponent<MortarShoot>();
        }
        else//Remove this condition, since this script is attached only to attacker ship
        {
            thisShipType = ShipType.SupplyShip;
            thisShipIsCannonOrMortarShip = false;
        }

        shipCenter = transform.GetChild(0).transform;
        shipOrBuildingIsInRange = false;
    }
    private void Start()
    {
        GameObject gameStateManager = GameObject.Find("GameStateManager");
        gameState = gameStateManager.GetComponent<GameState>();
        isPlayer1 = thisShipCategorizerPlayerScript.isP1Ship;
    }

    private void Update()
    {
        shipMaxRange = thisShipCategorizer_LevelScript.weaponRange;//later rather than putting this code in Update(), put it when level is switched. Better performance.
        myShipPosition = shipCenter.position;

        thisShipIsFunctional = thisShipCategorizerPlayerScript.shipIsFunctional;
        thisShipMenAreAlive = thisShipCategorizerPlayerScript.shipMenAreAlive;

        CurrentGameState currentGameState = gameState.currentGameState;
        if (currentGameState == CurrentGameState.CommonPlayTime)//Prevent attack between ships during strategy time
        {
            if (thisShipIsFunctional && thisShipMenAreAlive)
            {
                //check if main building is in range, and attack it if found
                if (!AttackMainBuildingInRangeToOurList())
                {
                    AddEnemyShipsInRangeToOurList();
                    RemoveShipsOutsideRangeFromOurList();
                    DetermineWhichShipToAttack();
                }
                //if there are no ships in range, then only attack any building
            }
            else
            {
                enemyShipsInRange.Clear();
                enemyBuildingsInRange.Clear();
                target = null;
            }
            AssignTargetToEachAttackerShip();
            //TestShipCode();
        }
    }
    private bool AttackMainBuildingInRangeToOurList()
    {
        Collider[] colliderArray = Physics.OverlapSphere(shipCenter.position, shipMaxRange);

        // Create a copy of the original list to avoid modification during iteration
        List<Collider> tempList = new List<Collider>(enemyBuildingsInRange);

        bool foundMainBuildingInRange = false;

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent<BuildingCategorizer_Player>(out _))
            {
                BuildingCategorizer_Player buildingCategorizer_Player = collider.GetComponent<BuildingCategorizer_Player>();
                bool buildingInRangeIsPlayer1 = buildingCategorizer_Player.buildingIsOfP1;
                bool buildingInRangeIsFunctional = buildingCategorizer_Player.buildingIsFunctional;
                bool buildingInRangeIsMainBuilding = buildingCategorizer_Player.buildingIsMainBuilding;

                if (buildingInRangeIsPlayer1 != isPlayer1)
                {
                    if (buildingInRangeIsFunctional && buildingInRangeIsMainBuilding)
                    {
                        //Vector3 enemyBuildingPosition = collider.transform.GetChild(0).transform.position;
                        Vector3 enemyBuildingPosition = collider.transform.position;//replace later by child object, maintained at a level to calculate distance

                        //be careful, as for each building, child 0 gameobject is assigned at very high height and may not detect.

                        // Calculate the distance between this ship and the current enemy ship
                        float distance = Vector3.Distance(myShipPosition, enemyBuildingPosition);

                        if (distance <= shipMaxRange)
                        {
                            if (!tempList.Contains(collider))
                            {
                                /*if (testActiveShip)
                                {
                                    print("Added " + collider.name + " to our list.");
                                }*/
                                enemyBuildingsInRange.Add(collider);
                            }
                            shipOrBuildingIsInRange = true;
                            foundMainBuildingInRange = true;
                            //No attack code here, just verified that main building is in range.

                            //Assign target as center of main building
                            targetCollider = collider;
                            target = targetCollider.gameObject;//Center of enemyBuilding is target
                        }
                        else
                        {
                            shipOrBuildingIsInRange = false;//can this code generate error later?
                        }
                    }
                }
            }
        }
        return foundMainBuildingInRange;
    }
    private void AddEnemyShipsInRangeToOurList()
    {
        Collider[] colliderArray = Physics.OverlapSphere(shipCenter.position, shipMaxRange);

        // Create a copy of the original list to avoid modification during iteration
        List<Collider> tempList = new List<Collider>(enemyShipsInRange);

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent<ShipCategorizer_Player>(out _))
            {
                ShipCategorizer_Player shipCategorizer_PlayerScript = collider.GetComponent<ShipCategorizer_Player>();
                bool shipInRangeIsPlayer1 = shipCategorizer_PlayerScript.isP1Ship;
                bool shipInRangeIsFunctional = shipCategorizer_PlayerScript.shipIsFunctional;
                bool shipInRangeMenAreAlive = shipCategorizer_PlayerScript.shipMenAreAlive;

                if (thisShipIsCannonOrMortarShip)
                {
                    if (shipInRangeIsFunctional || shipInRangeMenAreAlive)//attack until both ship health as well as ship men's health are zero
                    {
                        if (shipInRangeIsPlayer1 != isPlayer1)
                        {
                            Vector3 enemyShipPosition = collider.transform.GetChild(0).transform.position;

                            // Calculate the distance between this ship and the current enemy ship
                            float distance = Vector3.Distance(myShipPosition, enemyShipPosition);

                            if (distance <= shipMaxRange)
                            {
                                if (!tempList.Contains(collider))
                                {
                                    /*if (testActiveShip)
                                    {
                                        print("Added " + collider.name + " to our list.");
                                    }*/
                                    enemyShipsInRange.Add(collider);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (shipInRangeMenAreAlive)//attack only if ship men's health is not zero
                    {
                        if (shipInRangeIsPlayer1 != isPlayer1)
                        {
                            Vector3 enemyShipPosition = collider.transform.GetChild(0).transform.position;

                            // Calculate the distance between this ship and the current enemy ship
                            float distance = Vector3.Distance(myShipPosition, enemyShipPosition);

                            if (distance <= shipMaxRange)
                            {
                                if (!tempList.Contains(collider))
                                {
                                    /*if (testActiveShip)
                                    {
                                        print("Added " + collider.name + " to our list.");
                                    }*/
                                    enemyShipsInRange.Add(collider);
                                }
                            }
                        }
                    }
                }          
            }
        }
    }
    private void AddAllBuildingsInRangeToOurList()
    {
        Collider[] colliderArray = Physics.OverlapSphere(shipCenter.position, shipMaxRange);

        // Create a copy of the original list to avoid modification during iteration
        List<Collider> tempList = new List<Collider>(enemyBuildingsInRange);

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent<BuildingCategorizer_Player>(out _))
            {
                BuildingCategorizer_Player buildingCategorizer_Player = collider.GetComponent<BuildingCategorizer_Player>();
                bool buildingInRangeIsPlayer1 = buildingCategorizer_Player.buildingIsOfP1;
                bool buildingInRangeIsFunctional = buildingCategorizer_Player.buildingIsFunctional;

                if (buildingInRangeIsPlayer1 != isPlayer1)
                {
                    if (buildingInRangeIsFunctional)//attack until both ship health as well as ship men's health are zero
                    {
                        //Vector3 enemyBuildingPosition = collider.transform.GetChild(0).transform.position;
                        Vector3 enemyBuildingPosition = collider.transform.position;//replace later by child object, maintained at a level to calculate distance

                        //be careful, as for each building, child 0 gameobject is assigned at very high height and may not detect.

                        // Calculate the distance between this ship and the current enemy ship
                        float distance = Vector3.Distance(myShipPosition, enemyBuildingPosition);

                        if (distance <= shipMaxRange)
                        {
                            if (!tempList.Contains(collider))
                            {
                                /*if (testActiveShip)
                                {
                                    print("Added " + collider.name + " to our list.");
                                }*/
                                enemyBuildingsInRange.Add(collider);
                            }
                        }
                    }
                }
            }
        }
    }
    private void RemoveShipsOutsideRangeFromOurList()
    {
        // Create a copy of the original list to avoid modification during iteration
        List<Collider> tempEnemyShipsInRangeList = new List<Collider>(enemyShipsInRange);

        foreach (Collider enemyShip in tempEnemyShipsInRangeList)
        {
            Vector3 enemyShipPosition = enemyShip.transform.GetChild(0).transform.position;

            // Calculate the distance between this ship and the current enemy ship
            float distance = Vector3.Distance(myShipPosition, enemyShipPosition);

            ShipCategorizer_Player shipCategorizer_PlayerScript = enemyShip.GetComponent<ShipCategorizer_Player>();
            bool shipInRangeIsFunctional = shipCategorizer_PlayerScript.shipIsFunctional;
            bool shipInRangeMenAreAlive = shipCategorizer_PlayerScript.shipMenAreAlive;

            if (thisShipIsCannonOrMortarShip)
            {
                if (distance > shipMaxRange || (!shipInRangeIsFunctional && !shipInRangeMenAreAlive))
                {
                    /*if (testActiveShip)
                    {
                        print("Removed " + enemyShip.name + " from our list.");
                    }*/
                    enemyShipsInRange.Remove(enemyShip);
                }
            }
            else
            {
                if (distance > shipMaxRange || !shipInRangeMenAreAlive)
                {
                    /*if (testActiveShip)
                    {
                        print("Removed " + enemyShip.name + " from our list.");
                    }*/
                    enemyShipsInRange.Remove(enemyShip);
                }
            }                      
        }
    }
    private void RemoveBuildingsOutsideRangeFromOurList()
    {
        // Create a copy of the original list to avoid modification during iteration
        List<Collider> tempEnemyBuildingsInRangeList = new List<Collider>(enemyBuildingsInRange);

        foreach (Collider enemyBuilding in tempEnemyBuildingsInRangeList)
        {
            //Vector3 enemyBuildingPosition = enemyBuilding.transform.GetChild(0).transform.position;
            Vector3 enemyBuildingPosition = enemyBuilding.transform.position;//replace later by child object, maintained at a level to calculate distance

            // Calculate the distance between this ship and the current enemy ship
            float distance = Vector3.Distance(myShipPosition, enemyBuildingPosition);

            BuildingCategorizer_Player buildingCategorizer_PlayerScript = enemyBuilding.GetComponent<BuildingCategorizer_Player>();
            bool buildingInRangeIsFunctional = buildingCategorizer_PlayerScript.buildingIsFunctional;

            if (distance > shipMaxRange || !buildingInRangeIsFunctional)
            {
                /*if (testActiveShip)
                {
                    print("Removed " + enemyShip.name + " from our list.");
                }*/
                enemyBuildingsInRange.Remove(enemyBuilding);
            }
        }
    }
    private void DetermineWhichShipToAttack()
    {
        NoTargetIfNoShipInRange();
        OneShipInRangeCase();
        MultipleShipsInRange();
    }
    private void NoTargetIfNoShipInRange()
    {
        if (enemyShipsInRange.Count == 0)
        {
            target = null;
            targetCollider = null;
            AddAllBuildingsInRangeToOurList();
            RemoveBuildingsOutsideRangeFromOurList();
            DetermineWhichBuildingToAttack();
        }
    }
    private void NoTargetIfNoBuildingInRange()
    {
        if (enemyBuildingsInRange.Count == 0)
        {
            shipOrBuildingIsInRange = false;
            target = null;
            targetCollider = null;
        }
    }
    private void DetermineWhichBuildingToAttack()
    {
        NoTargetIfNoBuildingInRange();
        OneBuildingInRangeCase();
        MultipleBuildingsInRange();
    }

    private void OneShipInRangeCase()
    {
        if (enemyShipsInRange.Count == 1)
        {
            shipOrBuildingIsInRange = true;
            foreach (Collider oneEnemyShipInList in enemyShipsInRange)
            {
                targetCollider = oneEnemyShipInList;
                target = targetCollider.gameObject;//ShipCenter of enemyShip is target
            }
        }
    }
    private void OneBuildingInRangeCase()
    {
        if (enemyBuildingsInRange.Count == 1)
        {
            shipOrBuildingIsInRange = true;
            foreach (Collider oneEnemyBuildingInList in enemyBuildingsInRange)
            {
                targetCollider = oneEnemyBuildingInList;
                target = targetCollider.gameObject;//Center of enemyBuilding is target
            }
        }
    }
    private void MultipleShipsInRange()
    {
        if (enemyShipsInRange.Count > 1)
        {
            shipOrBuildingIsInRange = true;
            if (target == null)
            {
                SelectAnotherShipInRangeAsTarget();
            }
            IfTargetShipMovesOutsideOfRange();
        }
    }
    private void MultipleBuildingsInRange()
    {
        if (enemyBuildingsInRange.Count > 1)
        {
            shipOrBuildingIsInRange = true;
            if (target == null)
            {
                SelectAnotherBuildingInRangeAsTarget();
            }
            IfTargetBuildingMovesOutsideOfRange();
        }
    }
    private void SelectAnotherShipInRangeAsTarget()
    {
        if (enemyShipsInRange.Count > 1)
        {
            // Keep track of the nearest ship and its distance
            Collider nearestEnemyShip = null;
            float nearestDistance = float.MaxValue;//Initially set to very large arbitrary value so that initial comparison in first iteration is always true.

            //Find nearmost collider
            foreach (Collider enemyShip in enemyShipsInRange)
            {
                Vector3 enemyShipPosition = enemyShip.transform.GetChild(0).transform.position;

                // Calculate the distance between this ship and the current enemy ship
                float distance = Vector3.Distance(myShipPosition, enemyShipPosition);

                // Check if this is the closest ship so far
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemyShip = enemyShip;
                }
            }

            //Finally assign target
            if (nearestEnemyShip != null)
            {
                target = nearestEnemyShip.gameObject;
            }
        }
    }
    private void SelectAnotherBuildingInRangeAsTarget()
    {
        if (enemyBuildingsInRange.Count > 1)
        {
            // Keep track of the nearest building and its distance
            Collider nearestEnemyBuilding = null;
            float nearestDistance = float.MaxValue;//Initially set to very large arbitrary value so that initial comparison in first iteration is always true.

            //Find nearmost collider
            foreach (Collider enemyBuilding in enemyBuildingsInRange)
            {
                Vector3 enemyBuildingPosition = enemyBuilding.transform.GetChild(0).transform.position;

                // Calculate the distance between this ship and the current enemy ship
                float distance = Vector3.Distance(myShipPosition, enemyBuildingPosition);

                // Check if this is the closest ship so far
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemyBuilding = enemyBuilding;
                }
            }

            //Finally assign target
            if (nearestEnemyBuilding != null)
            {
                target = nearestEnemyBuilding.gameObject;
            }
        }
    }
    private void IfTargetShipMovesOutsideOfRange()
    {
        //Check for presence of target inside range. If target goes outside range, select another nearmost ship as target.

        // Create a copy of the original list to avoid modification during iteration
        List<Collider> tempEnemyShipsInRangeList = new List<Collider>(enemyShipsInRange);

        if (targetCollider != null)
        {
            //If our target ship is not inside enemyShipsInRange List, select another ship
            if (!tempEnemyShipsInRangeList.Contains(targetCollider))
            {
                SelectAnotherShipInRangeAsTarget();
            }
        }       
    }
    private void IfTargetBuildingMovesOutsideOfRange()//maybe this case is not possible, and not required
    {
        //Check for presence of target inside range. If we go outside target's range, select another nearmost building as target.

        // Create a copy of the original list to avoid modification during iteration
        List<Collider> tempEnemyBuildingsInRangeList = new List<Collider>(enemyBuildingsInRange);

        if (targetCollider != null)
        {
            //If our target ship is not inside enemyShipsInRange List, select another ship
            if (!tempEnemyBuildingsInRangeList.Contains(targetCollider))
            {
                SelectAnotherBuildingInRangeAsTarget();
            }
        }
    }
    private void AssignTargetToEachAttackerShip()
    {
        if (target != null)
        {
            GameObject targetShipCenter = target.transform.GetChild(0).gameObject;
            Transform targetShipCenterTransform = targetShipCenter.transform;

            if (thisShipType == ShipType.ArcherShip)
            {
                foreach (ArcherController subArcherControllerScript in archerControllerScript)
                {
                    subArcherControllerScript.B = targetShipCenterTransform;
                }
                archerShoot.targetEnemyForShipRotation = targetShipCenterTransform;
            }
            else if (thisShipType == ShipType.CannonShip)
            {
                foreach (CannonController subCannonControllerScript in cannonControllerScript)
                {
                    subCannonControllerScript.B = targetShipCenterTransform;
                }
                cannonShoot.targetEnemyForShipRotation = targetShipCenterTransform;
            }
            else if (thisShipType == ShipType.GunmanShip)
            {
                foreach (GunmanController subGunmanControllerScript in gunmanControllerScript)
                {
                    subGunmanControllerScript.B = targetShipCenterTransform;
                }
                gunShoot.targetEnemyForShipRotation = targetShipCenterTransform;
            }
            else if (thisShipType == ShipType.MortarShip)
            {
                foreach (MortarController subMortarControllerScript in mortarControllerScript)
                {
                    subMortarControllerScript.B = targetShipCenterTransform;
                }
                mortarShoot.targetEnemyForShipRotation = targetShipCenterTransform;
            }
        }
        else
        {
            if (thisShipType == ShipType.ArcherShip)
            {
                foreach (ArcherController subArcherControllerScript in archerControllerScript)
                {
                    subArcherControllerScript.B = null;
                }
                archerShoot.targetEnemyForShipRotation = null;
            }
            else if (thisShipType == ShipType.CannonShip)
            {
                foreach (CannonController subCannonControllerScript in cannonControllerScript)
                {
                    subCannonControllerScript.B = null;
                }
                cannonShoot.targetEnemyForShipRotation = null;
            }
            else if (thisShipType == ShipType.GunmanShip)
            {
                foreach (GunmanController subGunmanControllerScript in gunmanControllerScript)
                {
                    subGunmanControllerScript.B = null;
                }
                gunShoot.targetEnemyForShipRotation = null;
            }
            else if (thisShipType == ShipType.MortarShip)
            {
                foreach (MortarController subMortarControllerScript in mortarControllerScript)
                {
                    subMortarControllerScript.B = null;
                }
                mortarShoot.targetEnemyForShipRotation = null;
            }
        }
    }
    private ShipSize DetermineThisShipSize()
    {
        if (shipCategorizer_SizeScript.shipSize == ShipSize.Small)
        {
            return ShipSize.Small;
        }
        else if (shipCategorizer_SizeScript.shipSize == ShipSize.Medium)
        {
            return ShipSize.Medium;
        }
        else
        {
            return ShipSize.Large;
        }
    }
    private int GetShipMenCount(ShipSize shipSize)
    {
        switch (shipSize)
        {
            case ShipSize.Small:
                return SetParameters.SmallShipMenCount;
            case ShipSize.Medium:
                return SetParameters.MediumShipMenCount;
            case ShipSize.Large:
                return SetParameters.LargeShipMenCount;
            default:
                return 0;
        }
    }
    /*private void TestShipCode()
    {
        if (testActiveShip)
        {
            foreach (Collider enemyShip in enemyShipsInRange)
            {
                print(enemyShip.name);
            }
            if (target != null)
            {
                print("Target: " + target.GetComponentInParent<Collider>().name);
            }
        }
    }*/
}

//Use of the method Physics.OverlapSphere() also adds current ship into the array. The length of array increases as number of ships in range increases and decreases accordingly.
//By default, if no any ships are in range, array length is 1(this ship's collider is included). Other colliders, eg. Base Ground if in range are also added that may increase array length.
//If there are 3 ships in range, array length should be >= 5(Array Length) = 3(ships in range) + 1(ship with this script which also has collider) + 1(Other colliders, eg.Base Ground), 1 nearyby island, etc.