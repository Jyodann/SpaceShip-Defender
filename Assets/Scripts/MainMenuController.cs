using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionInformation;
    [SerializeField] private Toggle musicEffectToggle;
    [SerializeField] private Toggle swapJoysticksToggle;
    [SerializeField] private Toggle batterySaverToggle;
    
    [SerializeField] private GameObject donationScreen;
    [SerializeField] private GameObject mainScreen;


    /// <summary>
    ///     Code Referenced from How to Make a Main Menu by Brackeys:
    ///     https://www.youtube.com/watch?v=zc8ac_qUXQY
    /// </summary>
    private void Start()
    {
        SettingsHelper.LoadSettings();
        
        versionInformation.text = $"Version {Application.version} ({Application.platform})";

        musicEffectToggle.onValueChanged.AddListener(delegate(bool changed) { SettingsHelper.IsMusicOn = changed; });
        swapJoysticksToggle.onValueChanged.AddListener(delegate(bool changed)
        {
            SettingsHelper.IsSwappedJoysticks = changed;
        });
        batterySaverToggle.onValueChanged.AddListener(delegate(bool changed)
        {
            SettingsHelper.IsBatterySaver = changed;
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

    public void OpenDonationScreen()
    {
        #if UNITY_STANDALONE
            Application.OpenURL("https://ko-fi.com/jordynwinnie");
        #endif
        
        #if UNITY_WEBGL
            openWindow("https://ko-fi.com/jordynwinnie");
        #endif
        
        #if UNITY_IOS || UNITY_ANDROID
            mainScreen.SetActive(false);
            donationScreen.SetActive(true);
        #endif
    }

    #if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void openWindow(string url);
    #endif
}