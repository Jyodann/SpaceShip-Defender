using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    [SerializeField] private GameObject tutorialUI;
    public void LoadLevel(int sceneIndex)
    {
        if (SettingsHelper.IsFirstTimePlaying)
        {
            SettingsHelper.IsFirstTimePlaying = false;
            tutorialUI.SetActive(true);
        }
        else
        {
            StartCoroutine(LoadAsync(sceneIndex));
        }
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        var operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            yield return null;
        }
    }
}
