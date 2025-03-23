using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;

public class ItemEventHandler : MonoBehaviour
{
    public UnityEvent<int, string> OnItemAdded;
    public UnityEvent<int, string> OnItemRemoved;

    private string serverUrl = "https://wadahub.manerai.com/api/inventory/status";
    private string bearerToken = "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";

    public void AddItemEvent(int itemId)
    {
        StartCoroutine(SendItemEvent(itemId, "Added"));
    }

    public void RemoveItemEvent(int itemId)
    {
        StartCoroutine(SendItemEvent(itemId, "Removed"));
    }

    private IEnumerator SendItemEvent(int itemId, string eventType)
    {
        var requestData = new { item_id = itemId, event_type = eventType };
        string jsonData = JsonUtility.ToJson(requestData);

        UnityWebRequest request = new UnityWebRequest(serverUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + bearerToken);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result!= UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Server response: " + request.downloadHandler.text);
        }
    }
}