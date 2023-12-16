using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShipCategorizer_Level;
using static ShipCategorizer_Size;

public class CannonShoot : MonoBehaviour
{
    public int totalAmmoCount;
    public bool sufficientAmmoPresent;

    private ShipCategorizer_Size shipCategorizer_SizeScript;
    public HealthAmmoSystem ammoSystemScript;//used within another script

    public Transform targetEnemy;

    private void Awake()
    {
        shipCategorizer_SizeScript = GetComponent<ShipCategorizer_Size>();
        ammoSystemScript = GetComponent<HealthAmmoSystem>();

        if (shipCategorizer_SizeScript.shipSize == ShipSize.Small)
        {
            AssignAmmo_SizeBased(0);
        }
        else if (shipCategorizer_SizeScript.shipSize == ShipSize.Medium)
        {
            AssignAmmo_SizeBased(1);
        }
        else if (shipCategorizer_SizeScript.shipSize == ShipSize.Large)
        {
            AssignAmmo_SizeBased(2);
        }
    }
    private void Start()
    {
        sufficientAmmoPresent = true;
        targetEnemy = null;
    }
    private void Update()
    {
        HandleAmmoCount();
    }
    private void HandleAmmoCount()
    {
        if (totalAmmoCount <= 0)
        {
            sufficientAmmoPresent = false;
        }
        else
        {
            sufficientAmmoPresent = true;
        }
    }
    private void AssignAmmo_SizeBased(int index)
    {
        totalAmmoCount = SetParameters.CannonWeaponMaxAmmo[index];
    }
}

//Other functional portion in respective CannonController script