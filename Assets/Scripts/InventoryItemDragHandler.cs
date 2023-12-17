using UnityEngine;
using UnityEngine.EventSystems;
using static PlayerTurnSystem;
using static ShipCategorizer_Level;
public class InventoryItemDragHandler : DraggingTriggerer, IBeginDragHandler ,IDragHandler, IEndDragHandler
{
    public string shipSize;

    //Array to be initialized in the inspector in order of Small, medium, large
    public GameObject dragObjectPrefabArcher;
    public GameObject dragObjectPrefabGunman;
    public GameObject dragObjectPrefabCannon;
    public GameObject dragObjectPrefabMortar;

    public GameObject instantiateObjectPrefabArcher;
    public GameObject instantiateObjectPrefabGunman;
    public GameObject instantiateObjectPrefabCannon;
    public GameObject instantiateObjectPrefabMortar;

    public GameObject dragObjectPrefab;
    public GameObject instantiateObjectPrefab;

    private GameObject dragObject;

    [SerializeField] private PointSystem pointSystemScript;
    private InventoryItemDisplay inventoryItemDisplayScript;
    [SerializeField] private PlayerTurnSystem playerTurnSystemScript;

    private bool isDragging = false;
    private void Awake()
    {
        inventoryItemDisplayScript = GetComponent<InventoryItemDisplay>();
        if (name == "Large")
        {
            shipSize = "Large";
        }
        else if (name == "Medium")
        {
            shipSize = "Medium";
        }
        if (name == "Small")
        {
            shipSize = "Small";
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragObject = Instantiate(dragObjectPrefab);
        dragObject.GetComponent<DraggableObject>().followPointerPosition = true;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData) 
    {
        int shipCost = inventoryItemDisplayScript.GetShipCost;
        int shipLevel = inventoryItemDisplayScript.GetShipLevel;

        PlayerTurn currentPlayerTurn = playerTurnSystemScript.playerTurn;
        if (currentPlayerTurn == PlayerTurn.Player1Turn)
        {
            int currentPlayerPoint = pointSystemScript.Player1Point;
            if (dragObject.GetComponent<SpawnObjectPlaceholder>().CanSpawn() && currentPlayerPoint >= shipCost)
            {
                GameObject instantiatedShip = Instantiate(instantiateObjectPrefab, dragObject.transform.position, dragObject.transform.rotation);
                print("Price: " + inventoryItemDisplayScript.GetShipCost);
                pointSystemScript.ReducePlayer1Point(shipCost);

                ShipCategorizer_Player shipCategorizer_Player = instantiatedShip.GetComponent<ShipCategorizer_Player>();
                shipCategorizer_Player.isP1Ship = true;

                ShipCategorizer_Level shipCategorizer_Level = instantiatedShip.GetComponent<ShipCategorizer_Level>();
                ShipLevels shipLevelConverted = EvaluateShipLevel(shipLevel);
                shipCategorizer_Level.shipLevel = shipLevelConverted;
            }
            else if (currentPlayerPoint < shipCost)
            {
                print("Not enough money.");
            }
        }
        else if (currentPlayerTurn == PlayerTurn.Player2Turn)
        {
            int currentPlayerPoint = pointSystemScript.Player2Point;
            if (dragObject.GetComponent<SpawnObjectPlaceholder>().CanSpawn() && currentPlayerPoint >= shipCost)
            {
                GameObject instantiatedShip = Instantiate(instantiateObjectPrefab, dragObject.transform.position, dragObject.transform.rotation);
                print("Price: " + inventoryItemDisplayScript.GetShipCost);
                pointSystemScript.ReducePlayer2Point(shipCost);

                ShipCategorizer_Player shipCategorizer_Player = instantiatedShip.GetComponent<ShipCategorizer_Player>();
                shipCategorizer_Player.isP1Ship = false;

                ShipCategorizer_Level shipCategorizer_Level = instantiatedShip.GetComponent<ShipCategorizer_Level>();
                ShipLevels shipLevelConverted = EvaluateShipLevel(shipLevel);
                shipCategorizer_Level.shipLevel = shipLevelConverted;
            }
            else if (currentPlayerPoint < shipCost)
            {
                print("Not enough money.");
            }
        }

        isDragging = false;
        Destroy(dragObject);
    }

    public override bool IsDragging()
    {
        return isDragging;
    }
    private ShipLevels EvaluateShipLevel(int level)
    {
        if (level == 1)
        {
            return ShipLevels.Level1;
        }
        else if (level == 2)
        {
            return ShipLevels.Level2;
        }
        else if (level == 3)
        {
            return ShipLevels.Level3;
        }
        else
        {
            return ShipLevels.Level4;
        }
    }
}
