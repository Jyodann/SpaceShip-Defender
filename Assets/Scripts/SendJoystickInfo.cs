using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class SendJoystickInfo : OnScreenControl
{
    [SerializeField] private Joystick joystick;
    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string m_ControlPath;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        joystick = GetComponent<Joystick>();
    }

    private void Update()
    {
        SendValueToControl(joystick.Direction);
    }
}
