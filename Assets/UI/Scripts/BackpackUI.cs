using UnityEngine;

public class BackpackUI : MonoBehaviour
{
    public GameObject itemUIPrefab; // The prefab that represents an item in UI
    public Transform contentPanel; // The parent UI container for items in the backpack
    private Backpack backpack; // The actual backpack script

    private void Start()
    {
        backpack = GetComponent<Backpack>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0)) // Check if Left Mouse Button is held down
        {
            DisplayContents();
        }
    }

    private void DisplayContents()
    {
        // Clear existing UI
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Populate UI with backpack items
        foreach (GameObject item in backpack.items)
        {
            GameObject itemUI = Instantiate(itemUIPrefab, contentPanel);
        }
    }
}