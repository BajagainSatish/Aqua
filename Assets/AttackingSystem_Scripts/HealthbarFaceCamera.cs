using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthbarFaceCamera : MonoBehaviour
{
    private Transform mainCamera;

    private TextMeshProUGUI descriptionText;

    private Transform shipGameObject;
    private string shipLevelText;

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

        shipGameObject = MortarController.FindHighestParent(transform);

        if (shipGameObject.TryGetComponent<ShipCategorizer_Level>(out _))
        {
            shipLevelText = shipGameObject.GetComponent<ShipCategorizer_Level>().shipLevel.ToString();
            descriptionText.text = shipLevelText;//later handle case when ship's level is upgraded in runtime, level text also changes.
        }
        else if (shipGameObject.TryGetComponent<ShipCategorizer_Player>(out _))
        {
            descriptionText.text = "Supply";
        }
        else
        {
            descriptionText.text = "Main Building";
        }
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.forward);
    }
}
