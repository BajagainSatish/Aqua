using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthbarFaceCamera : MonoBehaviour
{
    private Transform mainCamera;
    private TextMeshProUGUI descriptionText;
    private Transform shipOrBuildingObject;
    private string shipLevelText;

    private ShipCategorizer_Player shipCategorizer_Player;
    private BuildingCategorizer_Player buildingCategorizer_Player;

    private string p1TextColor;
    private string p2TextColor;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "LevelText" || transform.GetChild(i).gameObject.name == "SupplyShipText" || transform.GetChild(i).gameObject.name == "MainBuildingText")
            {
                GameObject gameObject = transform.GetChild(i).gameObject;
                descriptionText = gameObject.GetComponent<TextMeshProUGUI>();
            }
        }
        p1TextColor = SetParameters.Player1TurnBackgroundColor;
        p2TextColor = SetParameters.Player2TurnBackgroundColor;
    }
    private void Start()
    {
        GameObject mainCameraGameObject = GameObject.Find("Main Camera");
        if (mainCameraGameObject != null)
        {
            mainCamera = mainCameraGameObject.transform;
        }
        else
        {
            Debug.LogWarning("Main camera not found!!! Healthbar won't face camera");
        }

        shipOrBuildingObject = MortarController.FindHighestParent(transform);

        if (shipOrBuildingObject.TryGetComponent<ShipCategorizer_Level>(out _))
        {
            shipLevelText = shipOrBuildingObject.GetComponent<ShipCategorizer_Level>().shipLevel.ToString();
            descriptionText.text = shipLevelText;//later handle case when ship's level is upgraded in runtime, level text also changes.

            shipCategorizer_Player = shipOrBuildingObject.GetComponent<ShipCategorizer_Player>();
        }
        else if (shipOrBuildingObject.TryGetComponent<ShipCategorizer_Player>(out _))
        {
            descriptionText.text = "Supply";
        }
        else
        {
            Transform islandBuilding = FindBuildingParent(transform);

            if (islandBuilding.TryGetComponent<BuildingCategorizer_Player>(out _))
            {
                buildingCategorizer_Player = islandBuilding.GetComponent<BuildingCategorizer_Player>();

                if (buildingCategorizer_Player.buildingIsMainBuilding)
                {
                    descriptionText.text = "Main Building";
                }
                else
                {
                    descriptionText.text = "Building";
                }
            }
        }
    }
    private void Update()
    {
        if (shipCategorizer_Player != null)
        {
            SetLevelTextColorBasedOnPlayer_Ship();
        }
        else if (buildingCategorizer_Player != null)
        {
            SetLevelTextColorBasedOnPlayer_Building();
        }
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.forward);
    }
    private void SetLevelTextColorBasedOnPlayer_Ship()
    {
        bool thisShipIsP1 = shipCategorizer_Player.isP1Ship;
        if (thisShipIsP1)
        {
            descriptionText.color = HexToColor(p1TextColor);
        }
        else
        {
            descriptionText.color = HexToColor(p2TextColor);
        }
    }
    private void SetLevelTextColorBasedOnPlayer_Building()
    {
        bool thisBuildingIsP1 = buildingCategorizer_Player.buildingIsOfP1;
        if (thisBuildingIsP1)
        {
            descriptionText.color = HexToColor(p1TextColor);
        }
        else
        {
            descriptionText.color = HexToColor(p2TextColor);
        }
    }
    public static Transform FindBuildingParent(Transform childTransform)
    {
        if (childTransform.TryGetComponent<BuildingCategorizer_Player>(out _))
        {
            return childTransform;
        }
        else
        {
            return FindBuildingParent(childTransform.parent);
        }
    }

    private Color HexToColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }
}
