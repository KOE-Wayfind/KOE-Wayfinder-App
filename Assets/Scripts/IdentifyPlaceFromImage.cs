using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class IdentifyPlaceFromImage : MonoBehaviour
{
    private static string _url = "https://iqfareez-special-bassoon-vrj7v597x5rcw7j6-5000.preview.app.github.dev/image";

    public static void Localize(string imageData, Action<LocalizeResult> callback)
    {
        // Create a JSON object with the desired data
        string json = "{\n\t\"image_data\": \" " + imageData + " \"\n}";

        // Create a UnityWebRequest object for the POST request
        // https://answers.unity.com/questions/1163204/prevent-unitywebrequestpost-from-url-encoding-the.html
        UnityWebRequest request = UnityWebRequest.Post(_url, json);
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
            // JObject responseJson = JObject.Parse(responseText);
            Debug.Log("Response: " + responseText);
            // string placeName = responseText["result"].ToString();
            callback(LocalizeResult.CreateFromJson(responseText));
        };
    }
}