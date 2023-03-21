/// <summary>
/// Hold data to pass between scenes
/// </summary>
public static class SceneManager {
    public static void LoadScene(string sceneName) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    
    public static void LoadScene(int sceneIndex) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
    
    public static string destinationTarget { get; set; }
}