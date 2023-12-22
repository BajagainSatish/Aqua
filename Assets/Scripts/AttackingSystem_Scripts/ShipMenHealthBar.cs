using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipMenHealthBar : MonoBehaviour
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
        int shipMenMaxHealth = shipCategorizer_Level.maxShipMenHealth;//needed in update as level selection occurs in update, later make check when button click
        int currentShipMenHealth = shipHealthAmmoSystem.currentShipMenHealth;

        SetShipMenMaxHealth(shipMenMaxHealth);
        SetShipMenHealth(currentShipMenHealth);
    }
    private void SetShipMenMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }
    private void SetShipMenHealth(int health)
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
