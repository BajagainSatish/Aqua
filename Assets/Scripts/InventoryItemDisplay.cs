using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDisplay : MonoBehaviour
{
    [SerializeField] Text shipSizeName;
    [SerializeField] Text shipCostText;
    [SerializeField] Text shipMenCountText;

    private static readonly int totalLevels = 4;

    [SerializeField] private Sprite[] levelSprites = new Sprite[2 * totalLevels];//lower index = active, higher index = inactive sprite
    [SerializeField] private ActiveShipTypeSelector activeShipTypeSelectorScript;

    private GameObject levelsGameObject;
    private GameObject[] level = new GameObject[totalLevels];
    private bool[] levelIsActive = new bool[totalLevels];

    private int currentlyActiveLevel;

    private string sizeOfShip_Item;
    private string typeOfShip_Item;

    private int weaponBasedShipCost;
    private int sizeBasedShipCost;
    private int totalShipCost;
    private int totalShipMenCount;

    public int GetShipCost
    {
        get
        {
            return totalShipCost;
        }
    }
    public int GetShipLevel
    {
        get
        {
            return currentlyActiveLevel;
        }
    }

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject childObject = transform.GetChild(i).gameObject;
            if (childObject.name == "Levels")
            {
                levelsGameObject = childObject;
            }
        }
        for (int i = 0; i < levelsGameObject.transform.childCount; i++)
        {
            level[i] = levelsGameObject.transform.GetChild(i).gameObject;
        }

        currentlyActiveLevel = 1;
        levelIsActive[0] = true;
        for (int i = 1; i < totalLevels; i++)
        {
            levelIsActive[i] = false;
        }
        DetermineShipSize();      
    }
    private void Start()
    {
        EvaluateShipMenCount();
        EvaluateShipCost();

        shipSizeName.text = sizeOfShip_Item;
        shipCostText.text = totalShipCost.ToString();
        shipMenCountText.text = totalShipMenCount.ToString();

        currentlyActiveLevel = 1;
        ActivateDeactivateLevelSprite();
    }
    public void ClickOn1()
    {
        if (currentlyActiveLevel != 1)
        {
            currentlyActiveLevel = 1;
            ActivateDeactivateLevelSprite();
            EvaluateShipCost();
        }
    }
    public void ClickOn2()
    {
        if (currentlyActiveLevel != 2)
        {
            currentlyActiveLevel = 2;
            ActivateDeactivateLevelSprite();
            EvaluateShipCost();
        }
    }
    public void ClickOn3()
    {
        if (currentlyActiveLevel != 3)
        {
            currentlyActiveLevel = 3;
            ActivateDeactivateLevelSprite();
            EvaluateShipCost();
        }
    }
    public void ClickOn4()
    {
        if (currentlyActiveLevel != 4)
        {
            currentlyActiveLevel = 4;
            ActivateDeactivateLevelSprite();
            EvaluateShipCost();
        }
    }
    private void ActivateDeactivateLevelSprite()
    {
        if (currentlyActiveLevel == 1)
        {
            for (int i = 0; i < totalLevels; i++)
            {
                Image levelImage = level[i].GetComponent<Image>();
                if (i == 0)
                {
                    levelImage.sprite = levelSprites[0];//active
                }
                else if (i == 1)
                {
                    levelImage.sprite = levelSprites[3];
                }
                else if (i == 2)
                {
                    levelImage.sprite = levelSprites[5];
                }
                else if (i == 3)
                {
                    levelImage.sprite = levelSprites[7];
                }
            }
        }
        else if (currentlyActiveLevel == 2)
        {
            for (int i = 0; i < totalLevels; i++)
            {
                Image levelImage = level[i].GetComponent<Image>();
                if (i == 0)
                {
                    levelImage.sprite = levelSprites[1];
                }
                else if (i == 1)
                {
                    levelImage.sprite = levelSprites[2];//active
                }
                else if (i == 2)
                {
                    levelImage.sprite = levelSprites[5];
                }
                else if (i == 3)
                {
                    levelImage.sprite = levelSprites[7];
                }
            }
        }
        else if (currentlyActiveLevel == 3)
        {
            for (int i = 0; i < totalLevels; i++)
            {
                Image levelImage = level[i].GetComponent<Image>();
                if (i == 0)
                {
                    levelImage.sprite = levelSprites[1];
                }
                else if (i == 1)
                {
                    levelImage.sprite = levelSprites[3];
                }
                else if (i == 2)
                {
                    levelImage.sprite = levelSprites[4];//active
                }
                else if (i == 3)
                {
                    levelImage.sprite = levelSprites[7];
                }
            }
        }
        else if (currentlyActiveLevel == 4)
        {
            for (int i = 0; i < totalLevels; i++)
            {
                Image levelImage = level[i].GetComponent<Image>();
                if (i == 0)
                {
                    levelImage.sprite = levelSprites[1];
                }
                else if (i == 1)
                {
                    levelImage.sprite = levelSprites[3];
                }
                else if (i == 2)
                {
                    levelImage.sprite = levelSprites[5];
                }
                else if (i == 3)
                {
                    levelImage.sprite = levelSprites[6];//active
                }
            }
        }
    }
    private void DetermineShipSize()
    {
        if (name == "Large")
        {
            sizeOfShip_Item = "Large";
        }
        else if (name == "Medium")
        {
            sizeOfShip_Item = "Medium";
        }
        else if (name == "Small")
        {
            sizeOfShip_Item = "Small";
        }
        else
        {
            print("Invalid size!!!");
        }

        if (sizeOfShip_Item == "Large")
        {
            sizeBasedShipCost = SetParameters.LargeShipCost;
        }
        else if (sizeOfShip_Item == "Medium")
        {
            sizeBasedShipCost = SetParameters.MediumShipCost;
        }
        else if (sizeOfShip_Item == "Small")
        {
            sizeBasedShipCost = SetParameters.SmallShipCost;
        }
    }
    public void EvaluateShipCost()
    {
        //Need 3 things: Ship type, Ship size, Ship Level
        typeOfShip_Item = activeShipTypeSelectorScript.activeShipType;

        if (typeOfShip_Item == "Archer")
        {
            weaponBasedShipCost = SetParameters.ArcherShipCost[currentlyActiveLevel - 1];
        }
        else if (typeOfShip_Item == "Cannon")
        {
            weaponBasedShipCost = SetParameters.CannonShipCost[currentlyActiveLevel - 1];
        }
        else if (typeOfShip_Item == "Gun")
        {
            weaponBasedShipCost = SetParameters.GunmanShipCost[currentlyActiveLevel - 1];
        }
        else if (typeOfShip_Item == "Mortar")
        {
            weaponBasedShipCost = SetParameters.MortarShipCost[currentlyActiveLevel - 1];
        }

        totalShipCost = weaponBasedShipCost + sizeBasedShipCost;
        shipCostText.text = totalShipCost.ToString();
    }
    private void EvaluateShipMenCount()
    {
        if (name == "Large")
        {
            totalShipMenCount = SetParameters.LargeShipMenCount;
        }
        else if (name == "Medium")
        {
            totalShipMenCount = SetParameters.MediumShipMenCount;
        }
        else if (name == "Small")
        {
            totalShipMenCount = SetParameters.SmallShipMenCount;
        }
    }
}
