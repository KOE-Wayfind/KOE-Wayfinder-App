using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationSettings : MonoBehaviour
{
    [SerializeField] private TMP_InputField serverUrlInputField;
    [SerializeField] private GameObject settingsPanel;
    
    public static string ServerUrl;
    
    // Start is called before the first frame update
    void Start()
    {
        // load setting
        ServerUrl = PlayerPrefs.GetString("serverUrl", "https://iqfareez-improved-fishstick-q45gqxjgrjg24qqx-5000.preview.app.github.dev");
        serverUrlInputField.text = ServerUrl;
    }

    /// <summary>
    /// Save changes in setting panel
    /// </summary>
    public void SaveSetting()
    {
        if (serverUrlInputField.text != "")
        {
            // trim ending slash if any
            serverUrlInputField.text = serverUrlInputField.text.TrimEnd('/');

            PlayerPrefs.SetString("serverUrl", serverUrlInputField.text);
        }
    }

    /// <summary>
    /// Toggle the settings panel visibility on button press
    /// </summary>
    public void ToggleSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
    
    
}
