using UnityEngine;

public class ItemDragHandler : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody rb;
    private Collider _collider;
    private bool isDragging = false;
    private Vector3 dragOffset;
    private Vector3 itemDistance = new Vector3(0,0.4f,0) ; 
    private int originalLayer;
    private int IgnoreRaycastLayer;

    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        IgnoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void Update()
    {
        if (isDragging)
        {
            FollowMouse();
        }
    }

    private void OnMouseDown()
    {
        StartDragging();
    }

    private void OnMouseUp()
    {
        StopDragging();
    }

    private void StartDragging()
    {
        isDragging = true;
        rb.isKinematic = true;
        _collider.isTrigger = true;

        // Store original layer and set the object to IgnoreRaycast
        originalLayer = gameObject.layer;
        gameObject.layer = IgnoreRaycastLayer;

        // Calculate offset between object position and mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            dragOffset = transform.position - hit.point + itemDistance;
        }
    }

    private void StopDragging()
    {
        isDragging = false;
        rb.isKinematic = false;
        _collider.isTrigger = false;
        // Restore the object's original layer
        gameObject.layer = originalLayer;

        // Check if dropped on a backpack
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = new RaycastHit[5]; // Use an array to prevent memory allocations
        int hitCount = Physics.RaycastNonAlloc(ray, hits, 100f);

        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider.CompareTag("Backpack"))
            {
                Backpack backpack = hits[i].collider.GetComponent<Backpack>();
                if (backpack != null)
                {
                    backpack.AddItem(this.gameObject);
                    return;
                }
            }
        }
    }

    private void FollowMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point + dragOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.2f);
        }
    }
}