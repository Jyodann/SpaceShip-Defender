using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    //SerialisedField to reference the controlOption so that the text can be updated based on current setting
    [SerializeField] private TextMeshProUGUI currentSelectedControlOption;

    [SerializeField] private TextMeshProUGUI versionInformation;

    /// <summary>
    /// Code Referenced from How to Make a Main Menu by Brackeys:
    /// https://www.youtube.com/watch?v=zc8ac_qUXQY
    /// </summary>
    private void Start()
    {
        SettingsHelper.LoadSettings();
        versionInformation.text = $"Version {Application.version} ({Application.platform})";
        
    }

    //Helper method to load the Game when play button is Clicked
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Helper method to quit the Game when quit button is Clicked
    public void QuitGame()
    {
        Application.Quit();
    }
}