using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CardAPI : MonoBehaviour
{
    // The URL of the API endpoint to send the data to
    public string apiUrl;

    // Structure that needs to be sent to API
    [Serializable]
    public class CardRequest
    {
        public string Id;
    }

    // Structure that is received from API - Mimics API. Need to update string and int types.
    [Serializable]
    public class CardResponse
    {
        public string Id;
        public string CardID;
        public string CardName;
        public string SubType;
        public int Attack;
        public int HP;
        public int Range;
        public int Width;
        public string Ability;
        public int PromotionLevel;
        public int ActionPoints;
    }

    // Instance of CardResponse to hold API response data
    private CardResponse cardResponse;

    // Driver for in-game testing - Called when the button is clicked
    public void OnButtonClick()
    {
        Debug.Log("Button Clicked");

        // Start a coroutine to send the data to the API
        CardRequest cardReq = new CardRequest { Id = "Removed" };
        StartCoroutine(SendDataToAPI(cardReq));
    }

    // Sends data to API
    private IEnumerator SendDataToAPI(CardRequest data)
    {
        // Transform CardRequest data into JSON
        string jsonData = JsonUtility.ToJson(data);
        Debug.Log(jsonData);

        // Constructing Unity API request
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Waiting for API response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log(json);

                // Parsing JSON response into a CardResponse class instance
                cardResponse = JsonUtility.FromJson<CardResponse>(json);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }

            // Dispose handlers to prevent memory leaks
            request.uploadHandler.Dispose();
            request.downloadHandler.Dispose();
        }
    }
}
