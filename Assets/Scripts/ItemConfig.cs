using UnityEngine;

public enum ItemType { Weapon, Potion, Armor }

[CreateAssetMenu(fileName = "NewItemConfig", menuName = "Inventory/ItemConfig")]
public class ItemConfig : ScriptableObject
{
    public string itemName;
    public int itemID;
    public ItemType itemType;
    public float itemWeight;
    public GameObject itemPrefab;
    public Sprite itemSprite;
}