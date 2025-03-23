using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemImageElement : MonoBehaviour
{
    [SerializeField]private Image itemImage;
    public GameObject linkedItem;

    public void SetItem(GameObject item, ItemConfig config)
    {
        linkedItem = item;
        itemImage.sprite = config.itemSprite;
        
        Color color = new Color(0,0,0,0);
        switch (config.itemType)
        {
            case ItemType.Armor :
                color = Color.blue;
                break;
            case ItemType.Potion:
                color = Color.blue;
                break;
            case  ItemType.Weapon :
                color = Color.red;
                break;
            default:
                color = new Color(0, 0, 0, 0);
                break;        
        }
        color.a = 0.5f;
        transform.GetComponent<Image>().color = color;
    }
}
