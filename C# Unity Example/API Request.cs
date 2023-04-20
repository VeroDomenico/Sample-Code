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

    //Stucture that needs to be sent to API
    public class CardRequest{
        public string Id;
    }
    //Structure that is recieved from API Mimics API Need to Update String and int
    public class CardResponse{
        string Id;
        string CardID;
        string CardName;
        string SubType;
        int Attack;
        int HP;
        int Range;
        int Width;
        string Ability;
        int PromotionLevel;
        int ActionPoints;
    }

    //card response
    CardResponse cardResponse;

    //Driver for In-Game Testing
    // Called when the button is clicked
    public void OnButtonClick()
    {
   
        Debug.Log("Button Clicked");

        // Start a coroutine to send the data to the API
        CardRequest cardReq = new CardRequest();
        cardReq.Id = "Removed";
        StartCoroutine(SendDataToAPI(cardReq));
    }

    //Sends data to API
    public IEnumerator SendDataToAPI(CardRequest data){

        //transform cardrequest Data into JSON
        string jsonData = JsonUtility.ToJson(data);
        Debug.Log(jsonData);

        // Constructing Unity API request
        UnityWebRequest request = new UnityWebRequest(apiUrl, "GET");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        //waiting for api response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success){
            string json = request.downloadHandler.text;
            int jsonStart = json.LastIndexOf("{");
            int jsonEnd = json.LastIndexOf("}");
            json = json.Substring(jsonStart, jsonEnd - jsonStart + 1);
            Debug.Log(json);
            //place data into a card request class
            cardResponse = JsonUtility.FromJson<CardResponse>(json);
        } else {
            Debug.Log("Error: " + request.error);
        }
        //Need to end the download and upload handlers to prevent memory leaks
        request.uploadHandler.Dispose();
        request.downloadHandler.Dispose();
        yield return null;
    }
}

    

