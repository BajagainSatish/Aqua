using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActiveShipTypeSelector : MonoBehaviour
{
    private string activeShipType;
    private int totalShipTypes = 4;

    private GameObject[] background = new GameObject[4];
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
    public void ClickOnArcher()
    {
        if (activeShipType != "Archer")
        {
            activeShipType = "Archer";
            SetBackgroundColor();
        }
    }
    public void ClickOnCannon()
    {
        if (activeShipType != "Cannon")
        {
            activeShipType = "Cannon";
            SetBackgroundColor();
        }
    }
    public void ClickOnGun()
    {
        if (activeShipType != "Gun")
        {
            activeShipType = "Gun";
            SetBackgroundColor();
        }
    }
    public void ClickOnMortar()
    {
        if (activeShipType != "Mortar")
        {
            activeShipType = "Mortar";
            SetBackgroundColor();
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
}
