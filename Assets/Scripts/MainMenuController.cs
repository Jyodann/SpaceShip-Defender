using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentSelectedControlOption;
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetMixedControlMode()
    {
        GameManager.playerControlMode = GameManager.ControlMode.MixedMouseKeyboard;
    }
    public void SetKeyboardOnlyControlMode()
    {
        GameManager.playerControlMode = GameManager.ControlMode.KeyboardOnly;
    }

    public void UpdateControlModeText()
    {
        switch (GameManager.playerControlMode)
        {
            case GameManager.ControlMode.KeyboardOnly:
                currentSelectedControlOption.text = "Current Control Mode: Classic";
                break;
            case GameManager.ControlMode.MixedMouseKeyboard:
                currentSelectedControlOption.text = "Current Control Mode: Keyboard + Mouse";
                break;
            default:
                break;
        }
    }
}
