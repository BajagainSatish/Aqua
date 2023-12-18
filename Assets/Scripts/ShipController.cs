using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ShipController : MonoBehaviour
{
    [SerializeField] private Transform targetBuilding;
    private float shipSpeed;

    private ShipCategorizer_Level shipCategorizer_Level;
    private int shipLevel;

    private void Awake()
    {
        shipCategorizer_Level = GetComponent<ShipCategorizer_Level>();
    }
    private void Start()
    {
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
    }
    private void Update()
    {
        if (targetBuilding != null)
        {
            print("shipspeed: " + shipSpeed);

            // Calculate the direction from the ship to the target building without normalization
            Vector3 direction = (targetBuilding.position - transform.position);

            // Ensure the ship is looking at the target building
            transform.LookAt(targetBuilding.position);

            // Move the ship towards the target building with the correct scaling
            transform.Translate(direction.normalized * shipSpeed * Time.deltaTime, Space.World);
        }
    }
}
