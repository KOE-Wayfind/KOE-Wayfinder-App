using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Send data to server to get the resulting place name
/// </summary>
public class IdentifyPlaceFromImage : MonoBehaviour
{
    public static void Localize(string imageData, Action<LocalizeResult> callback)
    {
        // obtain server url from setting
        var serverUrl = LocalizationSettings.ServerUrl;

        // Create a JSON object with the desired data
        string json = "{\n\t\"image_data\": \" " + imageData + " \"\n}";

        // Create a UnityWebRequest object for the POST request
        // https://answers.unity.com/questions/1163204/prevent-unitywebrequestpost-from-url-encoding-the.html
        var imageEndpoint = serverUrl + "/image";
        UnityWebRequest request = UnityWebRequest.Post(imageEndpoint, json);
        request.uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.method = UnityWebRequest.kHttpVerbPOST;

        // Set the content type to application/json
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request asynchronously
        var operation = request.SendWebRequest();
        operation.completed += delegate(AsyncOperation asyncOperation)
        {
            UnityWebRequest request = ((UnityWebRequestAsyncOperation)asyncOperation).webRequest;
            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                callback(null);
            }

            Debug.Log("Request completed successfully!");
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);
            callback(LocalizeResult.CreateFromJson(responseText));
        };
    }
}