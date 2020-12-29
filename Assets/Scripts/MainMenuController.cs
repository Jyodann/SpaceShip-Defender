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
        versionInformation.text = $"Version {Application.version} ({Application.platform})";
        if (Application.isMobilePlatform)
        {
            Application.targetFrameRate = 60;
            GameManager.instance.playerControlMode = GameManager.ControlMode.MobileInput;
        }
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

    //Helper method to set ControlMode to Mixed when option is selected
    public void SetMixedControlMode()
    {
        //Changes playerControlMode in GameManager:
        GameManager.instance.playerControlMode = GameManager.ControlMode.MixedMouseKeyboard;
        //Saves current ControlMode to PlayerPrefs:
        PlayerPrefs.SetInt("controlMode", (int)GameManager.ControlMode.MixedMouseKeyboard);
    }

    //Helper method to set ControlMode to Keyboard when option is selected
    public void SetKeyboardOnlyControlMode()
    {
        //Changes playerControlMode in GameManager:
        GameManager.instance.playerControlMode = GameManager.ControlMode.KeyboardOnly;
        //Saves current ControlMode to PlayerPrefs:
        PlayerPrefs.SetInt("controlMode", (int)GameManager.ControlMode.KeyboardOnly);
    }

    //Helper method that gets called when options button is selected so that the text will show current control mode
    public void UpdateControlModeText()
    {
        //Gets the current ControlMode from PlayerPrefs, and sets the Text to the current ControlStyle:
        switch ((GameManager.ControlMode)PlayerPrefs.GetInt("controlMode", 1))
        {
            case GameManager.ControlMode.KeyboardOnly:
                currentSelectedControlOption.text = "Control Style: Classic";
                break;

            case GameManager.ControlMode.MixedMouseKeyboard:
                currentSelectedControlOption.text = "Control Style: Mixed";
                break;

            case GameManager.ControlMode.MobileInput:
                currentSelectedControlOption.text = "Control Style: Mobile";
                break;

            default:
                break;
        }
    }
}