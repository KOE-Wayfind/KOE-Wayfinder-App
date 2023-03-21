using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private List<string> navigationTargets;
    [SerializeField] private GameObject listItem;
    [SerializeField] private GameObject scrollViewContent;
    
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
        SceneManager.destinationTarget = _currentSelectedLocation;
        SceneManager.LoadScene("MainARNavigation");
    }

    public void OnLocationSelected(string location)
    {
        startButton.GetComponent<Button>().interactable = true;
        _currentSelectedLocation = location;
    }
}
