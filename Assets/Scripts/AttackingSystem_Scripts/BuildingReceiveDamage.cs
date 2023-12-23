using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingReceiveDamage : MonoBehaviour
{
    private BuildingHealthSystem healthSystemScript;
    private BuildingCategorizer_Player buildingCategorizer_PlayerScript;

    private bool thisBuildingIsPlayer1;

    private void Awake()
    {
        healthSystemScript = GetComponent<BuildingHealthSystem>();
        buildingCategorizer_PlayerScript = GetComponent<BuildingCategorizer_Player>();
    }

    private void Start()
    {
        thisBuildingIsPlayer1 = buildingCategorizer_PlayerScript.buildingIsOfP1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ProjectileController>(out ProjectileController projectileControllerScript))
        {
            bool projectileIsOfPlayer1 = projectileControllerScript.isPlayer1Projectile;

            if (thisBuildingIsPlayer1 != projectileIsOfPlayer1)
            {
                int damage = projectileControllerScript.weaponDamage;
                //damage to building
                healthSystemScript.BuildingTakeDamage(damage);
                print(this.name + " took whole damage: " + damage);

                other.gameObject.SetActive(false);
            }
        }
    }
}
