using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDisable : MonoBehaviour
{
    private ProjectileController projectileController;

    [SerializeField] private float projectileLifetime;
    private bool isP1Projectile;
    private void Start()
    {
        projectileController = GetComponent<ProjectileController>();
        isP1Projectile = projectileController.isPlayer1Projectile;
    }
    private void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine(DeactivateProjectile());
        }
    }
    private IEnumerator DeactivateProjectile()
    {
        yield return new WaitForSeconds(projectileLifetime);
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ShipCategorizer_Player>(out _))
        {
            ShipCategorizer_Player shipCategorizer_Player = other.GetComponent<ShipCategorizer_Player>();
            bool enemyShipIsP1 = shipCategorizer_Player.isP1Ship;
            bool enemyShipsMenAreAlive = shipCategorizer_Player.shipMenAreAlive;//Projectile should pass through destroyed ships colliders

            //Under the assumption that if shipmenaredead, then ship is always no longer functional.
            if ((isP1Projectile != enemyShipIsP1) && enemyShipsMenAreAlive)
            {
                gameObject.SetActive(false);
            }
        }
        else if (other.TryGetComponent<BuildingCategorizer_Player>(out _))
        {
            BuildingCategorizer_Player buildingCategorizer_Player = other.GetComponent<BuildingCategorizer_Player>();
            bool enemyBuildingIsP1 = buildingCategorizer_Player.buildingIsOfP1;
            bool enemyBuildingIsFunctional = buildingCategorizer_Player.buildingIsFunctional;

            if ((isP1Projectile != enemyBuildingIsP1) && enemyBuildingIsFunctional)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            gameObject.SetActive(false);
        }
    }
}
