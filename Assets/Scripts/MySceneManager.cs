using UnityEngine.SceneManagement;

/// <summary>
/// Hold data to pass between scenes
/// </summary>
public static class MySceneManager {
    public static void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
    
    public static void LoadScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }

    public static string DestinationLocationName { get; set; }
    
    public static string OriginLocationName { get; set; }
}