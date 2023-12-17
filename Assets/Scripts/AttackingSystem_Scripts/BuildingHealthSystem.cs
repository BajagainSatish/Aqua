using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHealthSystem : MonoBehaviour
{
    private int maxBuildingHealth;
    public int currentBuildingHealth;
    private BuildingHealthbar buildingHealthBarScript;
    private BuildingCategorizer_Player buildingCategorizer_Player;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "Canvas")
            {
                GameObject canvasGameObject = transform.GetChild(i).gameObject;

                for (int j = 0; j < canvasGameObject.transform.childCount; j++)
                {
                    GameObject healthbarGameObject = canvasGameObject.transform.GetChild(j).gameObject;

                    if (healthbarGameObject.TryGetComponent<BuildingHealthbar>(out _))
                    {
                        buildingHealthBarScript = healthbarGameObject.GetComponent<BuildingHealthbar>();
                    }
                }
            }
        }
        buildingCategorizer_Player = GetComponent<BuildingCategorizer_Player>();
    }
    private void Start()
    {
        maxBuildingHealth = SetParameters.BuildingHealth;
        currentBuildingHealth = maxBuildingHealth;
        buildingHealthBarScript.SetBuildingMaxHealth(maxBuildingHealth);
    }
    private void Update()
    {
        CheckBuildingHealth();
    }
    public void BuildingTakeDamage(int damage)
    {
        currentBuildingHealth -= damage;
        buildingHealthBarScript.SetBuildingHealth(currentBuildingHealth);
    }

    public void SetBuildingHealth(int shipHealth)
    {
        currentBuildingHealth = shipHealth;
    }
    private void CheckBuildingHealth()
    {
        if (currentBuildingHealth <= 0)
        {
            buildingCategorizer_Player.buildingIsFunctional = false;
        }
        else
        {
            buildingCategorizer_Player.buildingIsFunctional = true;
        }
    }
}
