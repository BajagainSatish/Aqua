using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAlignTowardsEnemy : MonoBehaviour
{
    public Transform target;
    private float speed;
    
    private ShipCategorizer_Level shipCategorizer_LevelScript;
    private CannonShoot cannonShootScript;
    private ArcherShoot archerShootScript;
    private GunShoot gunShootScript;
    private MortarShoot mortarShootScript;

    private string thisShipType;

    private void Awake()
    {
        shipCategorizer_LevelScript = GetComponent<ShipCategorizer_Level>();
        if (TryGetComponent<CannonShoot>(out _))
        {
            cannonShootScript = GetComponent<CannonShoot>();
            thisShipType = "CannonShip";
        }
        else if (TryGetComponent<ArcherShoot>(out _))
        {
            archerShootScript = GetComponent<ArcherShoot>();
            thisShipType = "ArcherShip";
        }
        else if (TryGetComponent<GunShoot>(out _))
        {
            gunShootScript = GetComponent<GunShoot>();
            thisShipType = "GunmanShip";
        }
        else if (TryGetComponent<MortarShoot>(out _))
        {
            mortarShootScript = GetComponent<MortarShoot>();
            thisShipType = "MortarShip";
        }
    }
    private void Start()
    {
        if (shipCategorizer_LevelScript.shipLevel == ShipCategorizer_Level.ShipLevels.Level1)
        {
            AssignValue(0);
        }
        else if (shipCategorizer_LevelScript.shipLevel == ShipCategorizer_Level.ShipLevels.Level2)
        {
            AssignValue(1);
        }
        else if (shipCategorizer_LevelScript.shipLevel == ShipCategorizer_Level.ShipLevels.Level3)
        {
            AssignValue(2);
        }
        else if (shipCategorizer_LevelScript.shipLevel == ShipCategorizer_Level.ShipLevels.Level4)
        {
            AssignValue(3);
        }
    }
    private void Update()
    {
        if (thisShipType == "CannonShip" && cannonShootScript.targetEnemyForShipRotation != null)
        {
            target = cannonShootScript.targetEnemyForShipRotation;
        }
        else if (thisShipType == "ArcherShip" && archerShootScript.targetEnemyForShipRotation != null)
        {
            target = archerShootScript.targetEnemyForShipRotation;
        }
        else if (thisShipType == "GunmanShip" && gunShootScript.targetEnemyForShipRotation != null)
        {
            target = gunShootScript.targetEnemyForShipRotation;
        }
        else if (thisShipType == "MortarShip" && mortarShootScript.targetEnemyForShipRotation != null)
        {
            target = mortarShootScript.targetEnemyForShipRotation;
        }
        else
        {
            target = null;
        }

        if (target != null)
        {
            // Calculate the direction vector to the target
            Vector3 toTarget = target.position - transform.position;

            // Calculate the rotation to face the target
            Quaternion rotation = Quaternion.LookRotation(toTarget);

            // Calculate the ship's right direction
            Vector3 rightDirection = transform.right;

            // Calculate the dot product between the ship's right vector and the vector to the target
            float dotProduct = Vector3.Dot(rightDirection, toTarget);

            // Choose the rotation side based on the dot product
            Quaternion sidewaysRotation;
            if (dotProduct > 0)
            {
                // If the target is on the left side, rotate to the left side (e.g., -90 degrees)
                sidewaysRotation = Quaternion.Euler(0, -90, 0);
            }
            else
            {
                // If the target is on the right side or in front, rotate to the right side (e.g., 90 degrees)
                sidewaysRotation = Quaternion.Euler(0, 90, 0);
            }

            // Apply the sideways rotation to the calculated rotation
            Quaternion finalRotation = rotation * sidewaysRotation;

            // Apply the rotation gradually
            transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, speed * Time.deltaTime);

            // Adjust the local Euler angles to ensure the ship does not rotate along the x and z axis
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
    }
    private void AssignValue(int index)
    {
        speed = SetParameters.ShipRotationSpeed[index];
    }
}

