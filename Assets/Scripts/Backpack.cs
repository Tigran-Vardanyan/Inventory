using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Backpack : MonoBehaviour
{
    private ItemEventHandler itemEventHandler;
    
    public List<GameObject> items = new List<GameObject>();
    public UnityEvent<GameObject> OnItemAdded;
    public UnityEvent<GameObject> OnItemRemoved;
    public Transform itemDropPosition;

    public GameObject backpackUIPanel;
    public List<Transform> itemsContainers;
    public GameObject itemImagePrefab;


    public Transform weponSlot;
    public Transform potionSlot;
    public Transform sheldSlot;

    private bool isBackpackOpen = false;
    private GameObject selectedItem = null;

    private void Start()
    {
        itemEventHandler = GetComponent<ItemEventHandler>();
        backpackUIPanel.SetActive(false);
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ClickedOnBackpack())
            {
                OpenBackpackUI();
            }
        }
        
        if (Input.GetMouseButtonUp(0) && isBackpackOpen)
        {
            GameObject hoveredImage = GetHoveredItemImage();
            if (hoveredImage != null)
            {
                selectedItem = hoveredImage.GetComponent<ItemImageElement>().linkedItem;
                DropItem(selectedItem);
            }
            CloseBackpackUI();
        }
    }
    private bool ClickedOnBackpack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.CompareTag("Backpack");
        }
        return false;
    }
    private void OpenBackpackUI()
    {
        isBackpackOpen = true;
        backpackUIPanel.SetActive(true);
    }
    private void CloseBackpackUI()
    {
        isBackpackOpen = false;
        backpackUIPanel.SetActive(false);
        selectedItem = null;
    }
    private void PopulateBackpackUI()
    {
        foreach (Transform container in itemsContainers)
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (GameObject item in items)
        {
            BackpackItem backpackItem = item.GetComponent<BackpackItem>();
            Transform correctContainer = GetContainerForItem(backpackItem.itemConfig.itemType);

            GameObject itemImage = Instantiate(itemImagePrefab, correctContainer);
            ItemImageElement imageElement = itemImage.GetComponent<ItemImageElement>();
            imageElement.SetItem(item, backpackItem.itemConfig);
        }
    }
    private Transform GetContainerForItem(ItemType itemType)
    {
        return itemsContainers[(int)itemType];
    }

    private GameObject GetHoveredItemImage()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("ItemImage"))
            {
                return result.gameObject;
            }
        }
        return null;
    }
    public void AddItem(GameObject item)
    {
        var itemConfig = item.GetComponent<BackpackItem>().itemConfig;
        var itemRB = item.GetComponent<Rigidbody>();
        // Add item to the list
        items.Add(item);
        item.transform.SetParent(transform);
        
        switch (itemConfig.itemType)
        {
            case ItemType.Armor :
                item.transform.position = sheldSlot.position;
                item.transform.rotation = sheldSlot.rotation;
                item.layer = LayerMask.NameToLayer("Ignore Raycast");
                itemRB.isKinematic = true;
                break;
            case ItemType.Potion:
                item.transform.position = potionSlot.position;
                item.transform.rotation = potionSlot.rotation;
                item.layer = LayerMask.NameToLayer("Ignore Raycast");
                itemRB.isKinematic = true;
                break;
            case  ItemType.Weapon :
                item.transform.position = weponSlot.position;
                item.transform.rotation = weponSlot.rotation;
                item.layer = LayerMask.NameToLayer("Ignore Raycast");
                itemRB.isKinematic = true;
                break;
            default:
                item.transform.localPosition = Vector3.zero;
                item.SetActive(false);
                break;        
        }
       
        // Trigger the OnItemAdded UnityEvent
        OnItemAdded.Invoke(item);

        PopulateBackpackUI();
        
        if (itemEventHandler != null)
        {
            if (itemConfig != null)
            {
                // Call AddItemEvent with the item's ID
                itemEventHandler.AddItemEvent(itemConfig.itemID);
            }
            else
            {
                Debug.LogError("ItemConfig not found on the item. Cannot send the item event.");
            }
        }
    }
    public void DropItem(GameObject item)
    {
        items.Remove(item);
    
        item.transform.parent = null;
        item.SetActive(true);
    
        var itemRB = item.GetComponent<Rigidbody>();
        var itemConfigs = item.GetComponent<BackpackItem>().itemConfig;
    
        if (itemRB != null)
        {
            itemRB.isKinematic = false;
            itemRB.velocity = Vector3.zero;
            itemRB.angularVelocity = Vector3.zero;
        }

        switch (itemConfigs.itemType)
        {
            case ItemType.Armor:
            case ItemType.Potion:
            case ItemType.Weapon:
                item.layer = 0;
                break;
        }

        StartCoroutine(DelayedDrop(item));

        
        // Trigger the OnItemAdded UnityEvent
        OnItemRemoved.Invoke(item);
        PopulateBackpackUI();
        
        if (itemEventHandler != null)
        {
            var itemConfig = item.GetComponent<BackpackItem>().itemConfig; 
            if (itemConfig != null)
            {
                // Call RemoveItemEvent with the item's ID
                itemEventHandler.RemoveItemEvent(itemConfig.itemID);
            }
            else
            {
                Debug.LogError("ItemConfig not found on the item. Cannot send the item event.");
            }
        }
    }
    private IEnumerator DelayedDrop(GameObject item)
    {
        yield return new WaitForFixedUpdate(); // Ensure physics updates
        item.transform.position = itemDropPosition.position;
        item.transform.rotation = itemDropPosition.rotation;
    }
    
    
}