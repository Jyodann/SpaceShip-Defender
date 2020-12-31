using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionInformation;
    [SerializeField] private Toggle musicEffectToggle;
    [SerializeField] private Toggle swapJoysticksToggle;
    [SerializeField] private Toggle batterySaverToggle;


    /// <summary>
    /// Code Referenced from How to Make a Main Menu by Brackeys:
    /// https://www.youtube.com/watch?v=zc8ac_qUXQY
    /// </summary>
    private void Start()
    {
        SettingsHelper.LoadSettings();
        versionInformation.text = $"Version {Application.version} ({Application.platform})";

        musicEffectToggle.onValueChanged.AddListener(delegate(bool changed) { SettingsHelper.IsMusicOn = changed; Debug.Log(SettingsHelper.IsMusicOn);});
        swapJoysticksToggle.onValueChanged.AddListener(delegate(bool changed) { SettingsHelper.IsSwappedJoysticks = changed;
            Debug.Log(SettingsHelper.IsSwappedJoysticks); 
        });
        batterySaverToggle.onValueChanged.AddListener(delegate(bool changed) { SettingsHelper.IsBatterySaver = changed;
            Debug.Log(SettingsHelper.IsBatterySaver);
        });
    }
    
    //Helper method to quit the Game when quit button is Clicked
    public void QuitGame()
    {
        Application.Quit();
    }
    
    //Helper method to display the options menu
    public void ShowOptions()
    {
        musicEffectToggle.isOn = SettingsHelper.IsMusicOn;
        swapJoysticksToggle.isOn = SettingsHelper.IsSwappedJoysticks;
        batterySaverToggle.isOn = SettingsHelper.IsBatterySaver;
    }

    public void DonateLink()
    {
        Application.OpenURL("https://ko-fi.com/jordynwinnie");
    }
}