using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDisplay : MonoBehaviour
{
    [SerializeField] private InventoryItem itemDetails;

    [SerializeField] Text itemName;
    [SerializeField] Image itemIcon;
    [SerializeField] Text itemCost;

    private static readonly int totalLevels = 4;

    [SerializeField] private Sprite[] levelSprites = new Sprite[2 * totalLevels];//lower index = active, higher index = inactive sprite

    private GameObject levelsGameObject;
    private GameObject[] level = new GameObject[totalLevels];
    private bool[] levelIsActive = new bool[totalLevels];

    private int currentlyActiveLevel;

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
    }
    private void Start()
    {
        itemName.text = itemDetails.shipSize;
        itemIcon.sprite = itemDetails.shipSprite;
        itemCost.text = itemDetails.shipCost.ToString();

        currentlyActiveLevel = 1;
        ActivateDeactivateLevelSprite();
    }
    public void ClickOn1()
    {
        if (currentlyActiveLevel != 1)
        {
            currentlyActiveLevel = 1;
            ActivateDeactivateLevelSprite();
        }
    }
    public void ClickOn2()
    {
        if (currentlyActiveLevel != 2)
        {
            currentlyActiveLevel = 2;
            ActivateDeactivateLevelSprite();
        }
    }
    public void ClickOn3()
    {
        if (currentlyActiveLevel != 3)
        {
            currentlyActiveLevel = 3;
            ActivateDeactivateLevelSprite();
        }
    }
    public void ClickOn4()
    {
        if (currentlyActiveLevel != 4)
        {
            currentlyActiveLevel = 4;
            ActivateDeactivateLevelSprite();
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
}
