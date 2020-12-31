using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingsHelper
{
    public enum ControlMode { KeyboardOnly, MixedMouseKeyboard, MobileInput }

    private static bool isBatterySaver = false;
    private static bool isSwappedJoysticks = false;
    private static bool isMusicOn = true;
    private static ControlMode _controlMode;
    private static bool isFirstTimePlaying = true;
    public static bool IsBatterySaver
    {
        get => isBatterySaver;
        set
        {
            isBatterySaver = value;
            PlayerPrefs.SetInt("batterySaver", isBatterySaver ? 1 : 0);
            Application.targetFrameRate = isBatterySaver ? 30 : 60;
            QualitySettings.vSyncCount = isBatterySaver ? 0 : 1;
        }
    }

    public static bool IsFirstTimePlaying
    {
        get => isFirstTimePlaying;
        set
        {
            isFirstTimePlaying = value;
            PlayerPrefs.SetInt("firstTime", isFirstTimePlaying ? 1 : 0);
        }
    }

    public static bool IsSwappedJoysticks
    {
        //If swapped = true, right side move, left side aim
        get => isSwappedJoysticks;
        set
        {
            isSwappedJoysticks = value;
            PlayerPrefs.SetInt("swappedSticks", isSwappedJoysticks ? 1 : 0);
        }
    }

    public static bool IsMusicOn
    {
        get => isMusicOn;
        set
        {
            isMusicOn = value;
            PlayerPrefs.SetInt("musicOn", isMusicOn ? 1 : 0);
        }
    }

    public static ControlMode CurrentControlMode
    {
        get => _controlMode;
        set
        {
            _controlMode = value;
            PlayerPrefs.SetInt("controlMode", (int) _controlMode);
        }
    }

    public static void LoadSettings()
    {
        QualitySettings.vSyncCount = 1;
        IsMusicOn = PlayerPrefs.GetInt("musicOn", 1) == 1;
        IsSwappedJoysticks = PlayerPrefs.GetInt("swappedSticks", 0) == 1;
        IsBatterySaver = PlayerPrefs.GetInt("batterySaver", 0) == 1;
        CurrentControlMode = (ControlMode)PlayerPrefs.GetInt("controlMode", 1);
        IsFirstTimePlaying = PlayerPrefs.GetInt("firstTime", 1) == 1;
        
        if (Application.isMobilePlatform)
        {
            Application.targetFrameRate = isBatterySaver ? 30 : 60;
            QualitySettings.vSyncCount = isBatterySaver ? 0 : 1;
            CurrentControlMode = ControlMode.MobileInput;
        }
        else
        {
            CurrentControlMode = ControlMode.MixedMouseKeyboard;
        }
    }
}
