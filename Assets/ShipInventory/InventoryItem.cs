using UnityEngine;

[CreateAssetMenu(menuName = "InventoryItemShip", fileName = "NewShip")]
public class InventoryItem : ScriptableObject
{
    public string shipSize;
    public Sprite shipSprite;
    public int shipLevel;
    public int shipCost;
}
