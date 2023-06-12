using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class LocalizeResult
{
    public string result;

    public static LocalizeResult CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<LocalizeResult>(jsonString);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}