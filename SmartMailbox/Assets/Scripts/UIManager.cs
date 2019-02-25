using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {

    [SerializeField] private GameObject HomeContent;
    [SerializeField] private GameObject HistoryContent;
    [SerializeField] private GameObject SettingsContent;

    [SerializeField] private Color UIButtonBar_active;
    [SerializeField] private Color UIButtonBar_inactive;

    [SerializeField] private TextMeshProUGUI HomeIcon;
    [SerializeField] private TextMeshProUGUI HomeText;
    [SerializeField] private GameObject NewMailObj;
    [SerializeField] private GameObject NoMailObj;

    [SerializeField] private TextMeshProUGUI HistoryIcon;
    [SerializeField] private TextMeshProUGUI HistoryText;

    [SerializeField] private TextMeshProUGUI SettingsIcon;
    [SerializeField] private TextMeshProUGUI SettingsText;
    [SerializeField] private GameObject SettingsSavedText;

    [SerializeField] private TextMeshProUGUI Settings_InputField_ArduinoID;
    [SerializeField] private TextMeshProUGUI Settings_InputField_ServerURL;
    [SerializeField] private TextMeshProUGUI Settings_InputField_ServerPHP;

    [SerializeField] NetworkManager networkManager;

    public void Start()
    {
        // Initialie home screen at the start
        initHomescreen();
    }

    public void initHomescreen()
    {
        // Set all other UI Colors to inactive
        HistoryIcon.color   = UIButtonBar_inactive;
        HistoryText.color   = UIButtonBar_inactive;
        SettingsIcon.color  = UIButtonBar_inactive;
        SettingsText.color  = UIButtonBar_inactive;

        // Set the home screen button color to active
        HomeIcon.color = UIButtonBar_active;
        HomeText.color = UIButtonBar_active;

        // Set the other contents as inactive
        HistoryContent.SetActive(false);
        SettingsContent.SetActive(false);
        NoMailObj.SetActive(false);

        // Set the home content as active
        HomeContent.SetActive(true);

        // Get the newest mail
    }

    public void initHistoryscreen()
    {
        // Set all other UI Colors to inactive
        HomeIcon.color = UIButtonBar_inactive;
        HomeText.color = UIButtonBar_inactive;
        SettingsIcon.color = UIButtonBar_inactive;
        SettingsText.color = UIButtonBar_inactive;

        // Set the history screen button color to active
        HistoryIcon.color = UIButtonBar_active;
        HistoryText.color = UIButtonBar_active;

        // Set the other contents as inactive
        HomeContent.SetActive(false);
        SettingsContent.SetActive(false);

        // Set the home content as active
        HistoryContent.SetActive(true);
    }

    public void initSettingsscreen()
    {
        // Set all other UI Colors to inactive
        HistoryIcon.color = UIButtonBar_inactive;
        HistoryText.color = UIButtonBar_inactive;
        HomeIcon.color = UIButtonBar_inactive;
        HomeText.color = UIButtonBar_inactive;
        SettingsSavedText.SetActive(false);

        // Set the home screen button color to active
        SettingsIcon.color = UIButtonBar_active;
        SettingsText.color = UIButtonBar_active;

        // Set the other contents as inactive
        HistoryContent.SetActive(false);
        HomeContent.SetActive(false);

        // Set the settings content as active
        SettingsContent.SetActive(true);

        // Get the current networking settings and display them
        Settings_InputField_ArduinoID.text = networkManager.ArduinoID.ToString();
        Settings_InputField_ServerURL.text = networkManager.ServerIP;
        Settings_InputField_ServerPHP.text = networkManager.ServerPHP;
    }

    public void SaveSettings()
    {
        // Save the inputs to the variables from the networkmanager
        networkManager.ArduinoID    = int.Parse(Settings_InputField_ArduinoID.text);
        networkManager.ServerIP     = Settings_InputField_ServerURL.text;
        networkManager.ServerPHP    = Settings_InputField_ServerPHP.text;
        SettingsSavedText.SetActive(true);
    }
}
