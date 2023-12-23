using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipReceiveDamage : MonoBehaviour
{
    private ShipHealthAmmoSystem healthSystemScript;
    private ShipCategorizer_Player shipCategorizer_PlayerScript;

    private bool thisShipIsPlayer1;
    private bool thisShipMenAreAlive;
    private bool thisShipIsFunctional;

    private void Awake()
    {
        healthSystemScript = GetComponent<ShipHealthAmmoSystem>();
        shipCategorizer_PlayerScript = GetComponent<ShipCategorizer_Player>();
    }

    private void Start()
    {
        thisShipIsPlayer1 = shipCategorizer_PlayerScript.isP1Ship;
    }
    private void Update()
    {
        thisShipMenAreAlive = shipCategorizer_PlayerScript.shipMenAreAlive;
        thisShipIsFunctional = shipCategorizer_PlayerScript.shipIsFunctional;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ProjectileController>(out ProjectileController projectileControllerScript))
        {
            bool projectileIsOfPlayer1 = projectileControllerScript.isPlayer1Projectile;
            bool attackerShipIsArcherOrGunmanShip = projectileControllerScript.isArcherOrGunmanProjectile;

            if (thisShipIsPlayer1 != projectileIsOfPlayer1)
            {
                int damage = projectileControllerScript.weaponDamage;

                if (attackerShipIsArcherOrGunmanShip && thisShipMenAreAlive)//Damage only to ship men health
                {
                    healthSystemScript.ShipMenTakeDamage(damage);
                    other.gameObject.SetActive(false);
                    //print(this.name + " took only MAN damage: " + damage + " from archer/gunman ship.");
                }
                else if(!attackerShipIsArcherOrGunmanShip && (thisShipMenAreAlive || thisShipIsFunctional))
                {
                    healthSystemScript.ShipTakeDamage(damage);
                    healthSystemScript.ShipMenTakeDamage(damage);//later handle health below 0 case
                    other.gameObject.SetActive(false);
                    //print(this.name + " took whole damage: " + damage + " from cannon/mortar ship.");
                }
            }          
        }
    }
}
