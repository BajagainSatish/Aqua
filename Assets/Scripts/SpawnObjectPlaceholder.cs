using UnityEngine;

public class SpawnObjectPlaceholder : MonoBehaviour
{
    public string SpawnRegionTag = "SpawnRegion";

    public Material spawnableMaterial;
    public Material notSpawnableMaterial;

    private bool canSpawn;
    private bool isInSpawnableRegion;
    private bool isTouchingOtherObject;

    private bool prevFrameSpawnValue; //to reduce call to material change procedure

    private MeshRenderer[] meshRenderers;
    private DragShipClassifier dragShipClassifier;

    private bool isInValidSpawnableRegionWrtPlayer;
    private bool _dragShipBelongsToP1;
    private bool spawnableRegionBelongsToP1;

    private void Start()
    {
        dragShipClassifier = GetComponent<DragShipClassifier>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        canSpawn = false;
        prevFrameSpawnValue = false;
        SetMaterials();
    }

    private void Update()
    {
        CheckSpawnableRegionValidityWrtPlayer();
        CheckIfSpawnable();
        //only if previous frame was different try to change material
        if(canSpawn != prevFrameSpawnValue)
        {
            SetMaterials();
        }
        prevFrameSpawnValue = canSpawn;
    }
    private void SetMaterials()
    {
        if (canSpawn)
        {
            foreach (MeshRenderer mr in meshRenderers)
            {
                mr.material = spawnableMaterial;
            }
        }
        else
        {
            foreach (MeshRenderer mr in meshRenderers)
            {
                mr.material = notSpawnableMaterial;
            }
        }
    }
    public bool CanSpawn()
    {
        return canSpawn;
    }
    private void CheckIfSpawnable()
    {
        if(isInSpawnableRegion && !isTouchingOtherObject && isInValidSpawnableRegionWrtPlayer)
        {
            canSpawn = true;
        }
        else
        {
            canSpawn = false;
        }
    }
    private void CheckSpawnableRegionValidityWrtPlayer()
    {
        _dragShipBelongsToP1 = dragShipClassifier.DragShipBelongsToP1;

        if ((_dragShipBelongsToP1 && spawnableRegionBelongsToP1) || (!_dragShipBelongsToP1 && !spawnableRegionBelongsToP1))
        {
            isInValidSpawnableRegionWrtPlayer = true;
        }
        else
        {
            isInValidSpawnableRegionWrtPlayer = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(SpawnRegionTag))
        {
            isInSpawnableRegion = true;
        }
        else
        {
            isTouchingOtherObject = true;
        }

        if (other.TryGetComponent<InitializationPointClassifier>(out _))
        {
            InitializationPointClassifier initializationPointClassifier = other.GetComponent<InitializationPointClassifier>();
            spawnableRegionBelongsToP1 = initializationPointClassifier.InitializationPointBelongsToP1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(SpawnRegionTag))
        {
            isInSpawnableRegion = false;
        }
        else
        {
            isTouchingOtherObject = false;
        } 
    }

}
