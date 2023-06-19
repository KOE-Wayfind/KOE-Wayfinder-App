using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationSceneManager : MonoBehaviour
{
    /// <summary>
    /// Navigate to previous scene from the build setings
    /// </summary>
    public void GoBack()
    {
        // just checking to prevent errors, but a wise man wouldn't put a back button on the first scene 🤓
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0) return;
        
        MySceneManager.LoadScene(currentSceneIndex - 1);
    }

    /// <summary>
    /// Well, exit the app
    /// </summary>
    public void ExitApplication()
    {
        Application.Quit();
    }
}
