using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealthBar : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private Gradient gradient;
    private Image fill;

    private GameObject fillObject;

    private Transform shipGameObject;

    private ShipCategorizer_Level shipCategorizer_Level;
    private ShipHealthAmmoSystem shipHealthAmmoSystem;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "Fill")
            {
                fillObject = transform.GetChild(i).gameObject;
                fill = fillObject.GetComponent<Image>();
            }
        }
    }
    private void Start()
    {
        shipGameObject = FindHighestParent(transform);
        shipCategorizer_Level = shipGameObject.GetComponent<ShipCategorizer_Level>();
        shipHealthAmmoSystem = shipGameObject.GetComponent<ShipHealthAmmoSystem>();
    }
    private void Update()
    {
        int shipMaxHealth = shipCategorizer_Level.maxShipHealth;
        int currentShipHealth = shipHealthAmmoSystem.currentShipHealth;

        SetShipMaxHealth(shipMaxHealth);
        SetShipHealth(currentShipHealth);
    }
    private void SetShipMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }
    private void SetShipHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    private static Transform FindHighestParent(Transform childTransform)
    {
        if (childTransform.parent == null)
        {
            return childTransform;
        }
        else
        {
            return FindHighestParent(childTransform.parent);
        }
    }
}
