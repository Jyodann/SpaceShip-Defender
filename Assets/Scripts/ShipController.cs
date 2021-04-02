using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    /// <summary>
    ///     Screen Wrapping code Provided by BlackMole Studio on YouTube: https://www.youtube.com/watch?v=3uI8qXDCmzU
    /// </summary>
    [SerializeField] private float speed = 10f;

    [SerializeField] private float rotateSpeed = 10f;
    public float verticalMovement;
    public float horizontalMovement;
    public float verticalRotation;
    public float horizontalRotation;
    public Rigidbody2D rb2d;
    
    private bool isWrappingX;
    private bool isWrappingY;

    //Checks all 4 screen bounds:
    private Renderer[] renderers;

    private InputMaster controls;
    private InputDevice lastDevice;
    private string lastDeviceInterface = string.Empty;
    
    private void Awake()
    {
        controls = new InputMaster();
        controls.Enable();
        
        controls.Player.Movement.performed += MovementOnperformed;
        controls.Player.Movement.canceled += MovementOncanceled;
        
        controls.Player.Rotation.performed += RotationOnperformed;
        controls.Player.Rotation.canceled += RotationOncanceled;
    }

    private void RotationOncanceled(InputAction.CallbackContext obj)
    {
        verticalRotation = 0;
        horizontalRotation = 0;
    }

    private void RotationOnperformed(InputAction.CallbackContext obj)
    {
        var vectorToRead = obj.ReadValue<Vector2>();
        verticalRotation = vectorToRead.y;
        horizontalRotation = vectorToRead.x;

        if (obj.control.device.native)
        {
            print("RotVector: " + vectorToRead + "Controller Detected");
        }
        else
        {
            print("Mobile");
        }
    }
    
    private void MovementOncanceled(InputAction.CallbackContext obj)
    {
        verticalMovement = 0;
        horizontalMovement = 0;
    }

    private void MovementOnperformed(InputAction.CallbackContext obj)
    {
        var vectorToRead = obj.ReadValue<Vector2>();
        verticalMovement = vectorToRead.y;
        horizontalMovement = vectorToRead.x;
    }

    private void Start()
    {
        //Sets controlMode to the one in GameManager:
        //currentControlMode = GameManager.playerControlMode;
        rb2d = GetComponent<Rigidbody2D>();
        //Gets 4 of the screenbounds:
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        //var currentBindingGroup = controls.controlSchemes.First().bindingGroup;
        #if !ENABLE_INPUT_SYSTEM
        //Gets Input from A/D/W/S:
        verticalMovement = Input.GetButton("Vertical");
        horizontalMovement = Input.GetButton("Horizontal");
        #endif


        //If controlMode is Mixed, detect for mousePointer:
    
        //Code Referenced from Danndx on YouTube:
        //https://www.youtube.com/watch?v=_XdqA3xbP2A

        //Get CurrentMousePosition
        var mousePosition = controls.Player.PointerLocation.ReadValue<Vector2>();
        
        //Gets mouse position, but converted from ScreenPoint, to a WorldPosition:
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //Gets the directionToFace RELATIVE to mouse position, based on player's current location:
        var directionToFace = new Vector2(mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y);

        //Sets the ship to face the direction:
        transform.up = directionToFace;
        
        
        var rotation = Mathf.Atan2(horizontalRotation, verticalRotation) * 180 / Mathf.PI;
        if (rotation != 0) transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -rotation));
    }

    private void FixedUpdate()
    {
        
        rb2d.AddForce(new Vector2(horizontalMovement, verticalMovement) * speed);

#if !ENABLE_INPUT_SYSTEM
        

        //Changes movement based on controlMode Selected from MainMenu:
        switch (SettingsHelper.CurrentControlMode)
        {
            /** KeyboardOnly Controls (DEPRECATED)
                //RelativeForces are used as they steer the ship based on WHERE the ship is facing
                case SettingsHelper.ControlMode.KeyboardOnly:
                    //If keyboard only, the "vertical" (W/S) buttons will add relativeForce in the Y-axis:
                    if (verticalMovement)
                    {
                        var v = Input.GetAxisRaw("Vertical");
                        rb2d.AddRelativeForce(new Vector2(0, v) * speed);
                    }

                    //If keyboard only, the "horizontal" (A/D) buttons will rotate the ship left/right:
                    if (horizontalMovement)
                    {
                        var v = Input.GetAxisRaw("Horizontal");
                        transform.Rotate(new Vector3(0, 0, -v) * rotateSpeed);
                    }

                    break;
            **/

            case SettingsHelper.ControlMode.MixedMouseKeyboard:
                //If mixed, "vertical", "horizontal" will add Relativeforce on the y-axis and x-axis respectively
                

                break;

            case SettingsHelper.ControlMode.MobileInput:

                if (!SettingsHelper.IsSwappedJoysticks)
                {
                    if (leftJoystick.Vertical >= 0.2f)
                        rb2d.AddForce(new Vector2(0, 1) * speed);
                    else if (leftJoystick.Vertical <= -0.2f) rb2d.AddForce(new Vector2(0, -1) * speed);

                    if (leftJoystick.Horizontal >= 0.2f)
                        rb2d.AddForce(new Vector2(1, 0) * speed);
                    else if (leftJoystick.Horizontal <= -0.2f) rb2d.AddForce(new Vector2(-1, 0) * speed);

                    var rotation = Mathf.Atan2(rightJoystick.Horizontal, rightJoystick.Vertical) * 180 / Mathf.PI;
                    if (rotation != 0) transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -rotation));
                }
                else
                {
                    if (rightJoystick.Vertical >= 0.2f)
                        rb2d.AddForce(new Vector2(0, 1) * speed);
                    else if (rightJoystick.Vertical <= -0.2f) rb2d.AddForce(new Vector2(0, -1) * speed);

                    if (rightJoystick.Horizontal >= 0.2f)
                        rb2d.AddForce(new Vector2(1, 0) * speed);
                    else if (rightJoystick.Horizontal <= -0.2f) rb2d.AddForce(new Vector2(-1, 0) * speed);

                    var rotation = Mathf.Atan2(leftJoystick.Horizontal, leftJoystick.Vertical) * 180 / Mathf.PI;
                    if (rotation != 0) transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -rotation));
                }

                break;
        }
#endif
        ScreenWrap();
    }

    private void ScreenWrap()
    {
        //If space ship is on screen, no wrapping is happening:
        if (CheckRenderers())
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        //If wrapping is occuring on both sides, no more wrapping is required:
        if (isWrappingX && isWrappingY) return;
        //Get ship's current position:
        var newPosition = transform.position;

        //if newPosition x is more than one or less than zero, meaning it is OUT of screen
        // AND is in a valid position (i.e. Not in the screen border), then wrap by the x-axis by
        // mirroring the position:
        if (newPosition.x > 1 || newPosition.x < 0)
        {
            newPosition.x = -newPosition.x;
            isWrappingX = true;
        }

        //Same logic as x, but applied for y:
        if (newPosition.y > 1 || newPosition.y < 0)
        {
            newPosition.y = -newPosition.y;
            isWrappingY = true;
        }

        //sets the position of the ship to be the new, wrapped position:
        transform.position = newPosition;
    }

    private bool CheckRenderers()
    {
        //Goes through each renderer to check if spaceShip is currently on screen:
        foreach (var renderer in renderers)
            if (renderer.isVisible)
                return true;
        //Returns false if spaceship is not in screen:
        return false;
    }

    //Helper method to change speed, used by the PowerupScripot:
    public void ChangeSpeed(float speedToChange)
    {
        speed = speedToChange;
    }
}