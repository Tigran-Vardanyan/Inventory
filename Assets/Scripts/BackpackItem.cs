using System;
using Unity.VisualScripting;
using UnityEngine;

public class BackpackItem : MonoBehaviour, IBackpackItem
{
    private Rigidbody rb;

    public ItemConfig itemConfig;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SetItemType(itemConfig);
    }

    public void PickUp()
    {
        rb.isKinematic = true;
        transform.SetParent(null);
    }

    public void Drop(Vector3 position)
    {
        rb.isKinematic = false;
        transform.position = position;
    }

    public void AttachToBackpack(Transform backpack)
    {
        transform.SetParent(backpack);
        transform.localPosition = Vector3.zero;
        rb.isKinematic = true;
    }

    public void SetItemType(ItemConfig config)
    {
        this.itemConfig = config;
        rb.mass = config.itemWeight;
    }
}

