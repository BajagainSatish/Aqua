using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActiveShipTypeSelector : MonoBehaviour
{
    public string activeShipType;
    private int totalShipTypes = 4;
    private readonly static int totalSizeCount = 3;

    private GameObject[] background = new GameObject[4];
    [SerializeField]private InventoryItemDisplay[] inventoryItemDisplayScript = new InventoryItemDisplay[totalSizeCount];
    [SerializeField] private InventoryItemDragHandler[] inventoryItemDragHandlerScript = new InventoryItemDragHandler[totalSizeCount];

    //0 = archer, 1 = cannon, 2 = gunman, 3 = mortar
    private void Awake()
    {
        for (int i = 0; i < totalShipTypes; i++)
        {
            background[i] = transform.GetChild(i).gameObject;
        }
        activeShipType = "Archer";
        SetBackgroundColor();
    }
    private void Start()
    {
        SelectProperShipForDrag();
    }
    public void ClickOnArcher()
    {
        if (activeShipType != "Archer")
        {
            activeShipType = "Archer";
            SetBackgroundColor();
            ReEvaluateShipCost();
            SelectProperShipForDrag();
        }
    }
    public void ClickOnCannon()
    {
        if (activeShipType != "Cannon")
        {
            activeShipType = "Cannon";
            SetBackgroundColor();
            ReEvaluateShipCost();
            SelectProperShipForDrag();
        }
    }
    public void ClickOnGun()
    {
        if (activeShipType != "Gun")
        {
            activeShipType = "Gun";
            SetBackgroundColor();
            ReEvaluateShipCost();
            SelectProperShipForDrag();
        }
    }
    public void ClickOnMortar()
    {
        if (activeShipType != "Mortar")
        {
            activeShipType = "Mortar";
            SetBackgroundColor();
            ReEvaluateShipCost();
            SelectProperShipForDrag();
        }
    }
    private void SetBackgroundColor()
    {
        if (activeShipType == "Archer")
        {
            BackgroundChanger(0);
        }
        else if (activeShipType == "Cannon")
        {
            BackgroundChanger(1);
        }
        else if (activeShipType == "Gun")
        {
            BackgroundChanger(2);
        }
        else if (activeShipType == "Mortar")
        {
            BackgroundChanger(3);
        }
    }
    private void BackgroundChanger(int index)
    {
        for (int i = 0; i < totalShipTypes; i++)
        {
            Image backgroundImage = background[i].GetComponent<Image>();
            if (i == index)
            {
                SetColorFromHex(backgroundImage, "#8FFF93");
            }
            else
            {
                SetColorFromHex(backgroundImage, "#FFFFFF");
            }
        }
    }
    void SetColorFromHex(Image backgroundImage, string hexColor)
    {
        // Convert hex string to Color
        Color color;
        if (ColorUtility.TryParseHtmlString(hexColor, out color))
        {
            backgroundImage.color = color;
        }
        else
        {
            Debug.LogError("Invalid hex color code: " + hexColor);
        }
    }
    private void ReEvaluateShipCost()
    {
        for (int i = 0; i < totalSizeCount; i++)
        {
            inventoryItemDisplayScript[i].EvaluateShipCost();
        }
    }
    private void SelectProperShipForDrag()
    {
        if (activeShipType == "Archer")
        {
            for (int i = 0; i < totalSizeCount; i++)
            {
                inventoryItemDragHandlerScript[i].dragObjectPrefab = inventoryItemDragHandlerScript[i].dragObjectPrefabArcher;//large, medium, small
            }
        }
        else if (activeShipType == "Cannon")
        {
            for (int i = 0; i < totalSizeCount; i++)
            {
                inventoryItemDragHandlerScript[i].dragObjectPrefab = inventoryItemDragHandlerScript[i].dragObjectPrefabCannon;//large, medium, small
            }
        }
        else if (activeShipType == "Gun")
        {
            for (int i = 0; i < totalSizeCount; i++)
            {
                inventoryItemDragHandlerScript[i].dragObjectPrefab = inventoryItemDragHandlerScript[i].dragObjectPrefabGunman;//large, medium, small
            }
        }
        else if (activeShipType == "Mortar")
        {
            for (int i = 0; i < totalSizeCount; i++)
            {
                inventoryItemDragHandlerScript[i].dragObjectPrefab = inventoryItemDragHandlerScript[i].dragObjectPrefabMortar;//large, medium, small
            }
        }
    }
}
