using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDragHandler : DraggingTriggerer, IBeginDragHandler ,IDragHandler, IEndDragHandler
{
    private readonly static int shipSizes = 3;
    public string shipSize;

    //Array to be initialized in the inspector in order of Small, medium, large
    public GameObject dragObjectPrefabArcher;
    public GameObject dragObjectPrefabGunman;
    public GameObject dragObjectPrefabCannon;
    public GameObject dragObjectPrefabMortar;

    public GameObject[] instantiateObjectPrefabArcher = new GameObject[shipSizes];
    public GameObject[] instantiateObjectPrefabGunman = new GameObject[shipSizes];
    public GameObject[] instantiateObjectPrefabCannon = new GameObject[shipSizes];
    public GameObject[] instantiateObjectPrefabMortar = new GameObject[shipSizes];

    public GameObject dragObjectPrefab;
    public GameObject instantiateObjectPrefab;

    private GameObject dragObject;

    private bool isDragging = false;
    private void Awake()
    {
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
        if (dragObject.GetComponent<SpawnObjectPlaceholder>().CanSpawn())
            Instantiate(instantiateObjectPrefab, dragObject.transform.position, dragObject.transform.rotation);

        isDragging = false;
        Destroy(dragObject);
    }

    public override bool IsDragging()
    {
        return isDragging;
    }
}
