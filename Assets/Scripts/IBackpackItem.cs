using UnityEngine;

public interface IBackpackItem
{
    void PickUp();
    void Drop(Vector3 position);
    void AttachToBackpack(Transform backpack);

    void SetItemType(ItemConfig itemConfig);
}