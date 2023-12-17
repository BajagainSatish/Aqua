using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCategorizer_Player : MonoBehaviour
{
    public bool buildingIsOfP1;
    public bool buildingIsFunctional;
    public bool buildingIsMainBuilding;

    private void Start()
    {
        buildingIsFunctional = true;
    }
}
