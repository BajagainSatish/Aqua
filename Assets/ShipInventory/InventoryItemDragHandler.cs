using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDragHandler : DraggingTriggerer, IBeginDragHandler ,IDragHandler, IEndDragHandler
{
    private static int shipSizes = 3;

    //Array to be initialized in the inspector in order of Small, medium, large
    [SerializeField] private GameObject[] dragObjectPrefabArcher = new GameObject[shipSizes];
    [SerializeField] private GameObject[] dragObjectPrefabGunman = new GameObject[shipSizes];
    [SerializeField] private GameObject[] dragObjectPrefabCannon = new GameObject[shipSizes];
    [SerializeField] private GameObject[] dragObjectPrefabMortar = new GameObject[shipSizes];

    [SerializeField] private GameObject[] instantiateObjectPrefabArcher = new GameObject[shipSizes];
    [SerializeField] private GameObject[] instantiateObjectPrefabGunman = new GameObject[shipSizes];
    [SerializeField] private GameObject[] instantiateObjectPrefabCannon = new GameObject[shipSizes];
    [SerializeField] private GameObject[] instantiateObjectPrefabMortar = new GameObject[shipSizes];

    public GameObject dragObjectPrefab;
    public GameObject instantiateObjectPrefab;

    private GameObject dragObject;

    private bool isDragging = false;
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
