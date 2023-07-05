using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private List<string> navigationTargets;
    [SerializeField] private GameObject listItem;
    [SerializeField] private GameObject scrollViewContent;
    
    [SerializeField] private GameObject infoPanel;
    
    private string _currentSelectedLocation;

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < navigationTargets.Count; i++)
        {
            var destinationTarget = navigationTargets[i];
            var button = Instantiate(listItem, scrollViewContent.transform);
            
            // offset below one another
            button.transform.localPosition = new Vector3(0, (-i * 30) - 25, 0);
            button.gameObject.SetActive(true);

            button.GetComponentInChildren<TMP_Text>().text = destinationTarget;
            button.GetComponent<Button>().onClick.AddListener(() => OnLocationSelected(destinationTarget));
        }
    }
    
    public void StartButtonClicked()
    {
        MySceneManager.DestinationLocationName = _currentSelectedLocation;
        Debug.Log("destinationTarget is: " + MySceneManager.DestinationLocationName);

        MySceneManager.LoadScene("CameraCapture");
    }

    public void OnLocationSelected(string location)
    {
        Debug.Log($"On Location Selected {location}");
        startButton.GetComponent<Button>().interactable = true;
        _currentSelectedLocation = location;
    }

    public void OpenInfoPanel()
    {
        infoPanel.SetActive(true);
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }

    public void OpenLink(string url)
    {
        // open URL
        Application.OpenURL(url);
    }
}
