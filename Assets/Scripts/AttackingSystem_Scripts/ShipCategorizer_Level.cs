using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShipCategorizer_Size;

public class ShipCategorizer_Level : MonoBehaviour
{
    public enum ShipLevels
    {
        Level1, Level2, Level3, Level4
    };
    public ShipLevels shipLevel;
    private ShipLevels previousShipLevel;

    //None of these values except ship level are changed, eg. ship health during update.
    public int shipHealth;
    public int shipMenHealth;
    private float shipSpeed;

    private int weaponBasedShipCost;
    private int sizeBasedShipCost;
    public int totalShipCost;

    public float weaponRange;
    public int weaponDamage;
    //weapon max ammo and weapon reload speed handled in other scripts

    private TargetingSystem_PhysicsOverlapSphere targetingSystem_PhysicsOverlapSphereScript;
    private ShipCategorizer_Size shipCategorizer_SizeScript;
    private ShipHealthAmmoSystem healthAmmoSystemScript;

    //Move this code to update or another approach if level of ship upgrades within game
    private void Awake()
    {
        targetingSystem_PhysicsOverlapSphereScript = GetComponent<TargetingSystem_PhysicsOverlapSphere>();
        shipCategorizer_SizeScript = GetComponent<ShipCategorizer_Size>();
        healthAmmoSystemScript = GetComponent<ShipHealthAmmoSystem>();

        previousShipLevel = shipLevel;

        if (shipLevel == ShipLevels.Level1)
        {
            AssignValue(0);
        }
        else if (shipLevel == ShipLevels.Level2)
        {
            AssignValue(1);
        }
        else if (shipLevel == ShipLevels.Level3)
        {
            AssignValue(2);
        }
        else if (shipLevel == ShipLevels.Level4)
        {
            AssignValue(3);
        }

        EvaluateShipSize();
        EvaluateShipCost();
    }

    private void Update()
    {
        if (shipLevel != previousShipLevel)//means level of ship has been changed
        {
            if (shipLevel == ShipLevels.Level1)
            {
                AssignValue(0);
            }
            else if (shipLevel == ShipLevels.Level2)
            {
                AssignValue(1);
            }
            else if (shipLevel == ShipLevels.Level3)
            {
                AssignValue(2);
            }
            else if (shipLevel == ShipLevels.Level4)
            {
                AssignValue(3);
            }
            previousShipLevel = shipLevel;
        }
    }

    private void EvaluateShipCost()
    {
        totalShipCost = weaponBasedShipCost + sizeBasedShipCost;
    }
    private void EvaluateShipSize()
    {
        if (shipCategorizer_SizeScript.shipSize == ShipSize.Small)
        {
            sizeBasedShipCost = SetParameters.SmallShipCost;
        }
        else if (shipCategorizer_SizeScript.shipSize == ShipSize.Medium)
        {
            sizeBasedShipCost = SetParameters.MediumShipCost;
        }
        else if (shipCategorizer_SizeScript.shipSize == ShipSize.Large)
        {
            sizeBasedShipCost = SetParameters.LargeShipCost;
        }
    }
    private void AssignValue(int index)
    {
        if (targetingSystem_PhysicsOverlapSphereScript.thisShipType == TargetingSystem_PhysicsOverlapSphere.ShipType.ArcherShip)
        {
            weaponRange = SetParameters.ArcherWeaponRange[index];
            shipHealth = SetParameters.ArcherShipHealth[index];
            shipMenHealth = SetParameters.ArcherShipMenHealth[index];
            weaponDamage = SetParameters.ArcherWeaponDamage[index];
            weaponBasedShipCost = SetParameters.ArcherShipCost[index];

            healthAmmoSystemScript.SetShipAndShipMenHealth(shipHealth,shipMenHealth);
        }
        else if (targetingSystem_PhysicsOverlapSphereScript.thisShipType == TargetingSystem_PhysicsOverlapSphere.ShipType.CannonShip)
        {
            weaponRange = SetParameters.CannonWeaponRange[index];
            shipHealth = SetParameters.CannonShipHealth[index];
            shipMenHealth = SetParameters.CannonShipMenHealth[index];
            weaponDamage = SetParameters.CannonWeaponDamage[index];
            weaponBasedShipCost = SetParameters.CannonShipCost[index];

            healthAmmoSystemScript.SetShipAndShipMenHealth(shipHealth, shipMenHealth);
        }
        else if (targetingSystem_PhysicsOverlapSphereScript.thisShipType == TargetingSystem_PhysicsOverlapSphere.ShipType.GunmanShip)
        {
            weaponRange = SetParameters.GunmanWeaponRange[index];
            shipHealth = SetParameters.GunmanShipHealth[index];
            shipMenHealth = SetParameters.GunmanShipMenHealth[index];
            weaponDamage = SetParameters.GunmanWeaponDamage[index];
            weaponBasedShipCost = SetParameters.GunmanShipCost[index];

            healthAmmoSystemScript.SetShipAndShipMenHealth(shipHealth, shipMenHealth);
        }
        else if (targetingSystem_PhysicsOverlapSphereScript.thisShipType == TargetingSystem_PhysicsOverlapSphere.ShipType.MortarShip)
        {
            weaponRange = SetParameters.MortarWeaponRange[index];
            shipHealth = SetParameters.MortarShipHealth[index];
            shipMenHealth = SetParameters.MortarShipMenHealth[index];
            weaponDamage = SetParameters.MortarWeaponDamage[index];
            weaponBasedShipCost = SetParameters.MortarShipCost[index];

            healthAmmoSystemScript.SetShipAndShipMenHealth(shipHealth, shipMenHealth);
        }

        shipSpeed = SetParameters.ShipSpeed[index];
    }
}
