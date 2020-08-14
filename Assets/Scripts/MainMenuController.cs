using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// Code Referenced from How to Make a Main Menu by Brackeys:
    /// https://www.youtube.com/watch?v=zc8ac_qUXQY
    /// </summary>

    //SerialisedField to reference the controlOption so that the text can be updated based on current setting
    [SerializeField] private TextMeshProUGUI currentSelectedControlOption;

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
        GameManager.playerControlMode = GameManager.ControlMode.MixedMouseKeyboard;
    }

    //Helper method to set ControlMode to Keyboard when option is selected
    public void SetKeyboardOnlyControlMode()
    {
        GameManager.playerControlMode = GameManager.ControlMode.KeyboardOnly;
    }

    //Helper method that gets called when options button is selected so that the text will show current control mode
    public void UpdateControlModeText()
    {
        switch (GameManager.playerControlMode)
        {
            case GameManager.ControlMode.KeyboardOnly:
                currentSelectedControlOption.text = "Control Style: Classic";
                break;

            case GameManager.ControlMode.MixedMouseKeyboard:
                currentSelectedControlOption.text = "Control Style: Keyboard + Mouse";
                break;

            default:
                break;
        }
    }
}