using UnityEngine;

public class ShipController : MonoBehaviour
{
    /// <summary>
    /// Screen Wrapping code Provided by BlackMole Studio on YouTube: https://www.youtube.com/watch?v=3uI8qXDCmzU
    /// </summary>
    [SerializeField] private float speed = 10f;

    [SerializeField] private float rotateSpeed = 10f;
    public bool verticalMovement = false;
    public bool horizontalMovement = false;
    public Rigidbody2D rb2d;
    private bool isWrappingX = false;
    private bool isWrappingY = false;

    public GameManager.ControlMode currentControlMode;

    //Checks all 4 screen bounds:
    private Renderer[] renderers;

    private void Start()
    {
        //Sets controlMode to the one in GameManager:
        currentControlMode = GameManager.playerControlMode;
        rb2d = GetComponent<Rigidbody2D>();
        //Gets 4 of the screenbounds:
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Gets Input from A/D/W/S:
        verticalMovement = Input.GetButton("Vertical");
        horizontalMovement = Input.GetButton("Horizontal");

        //If controlMode is Mixed, detect for mousePointer:
        if (currentControlMode == GameManager.ControlMode.MixedMouseKeyboard)
        {
            //Code Referenced from Danndx on YouTube:
            //https://www.youtube.com/watch?v=_XdqA3xbP2A

            //Get CurrentMousePosition
            Vector3 mousePosition = Input.mousePosition;
            //Gets mouse position, but converted from ScreenPoint, to a WorldPosition:
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            //Gets the directionToFace RELATIVE to mouse position, based on player's current location:
            Vector2 directionToFace = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            //Sets the ship to face the direction:
            transform.up = directionToFace;
        }
    }

    private void FixedUpdate()
    {
        //Changes movement based on controlMode Selected from MainMenu:
        switch (currentControlMode)
        {
            //RelativeForces are used as they steer the ship based on WHERE the ship is facing
            case GameManager.ControlMode.KeyboardOnly:
                //If keyboard only, the "vertical" (W/S) buttons will add relativeForce in the Y-axis:
                if (verticalMovement)
                {
                    float v = Input.GetAxisRaw("Vertical");
                    rb2d.AddRelativeForce(new Vector2(0, v) * speed);
                }
                //If keyboard only, the "horizontal" (A/D) buttons will rotate the ship left/right:
                if (horizontalMovement)
                {
                    float v = Input.GetAxisRaw("Horizontal");
                    transform.Rotate(new Vector3(0, 0, -v) * rotateSpeed);
                }
                break;

            case GameManager.ControlMode.MixedMouseKeyboard:
                //If mixed, "vertical", "horizontal" will add Relativeforce on the y-axis and x-axis respectively
                if (verticalMovement)
                {
                    float v = Input.GetAxisRaw("Vertical");
                    rb2d.AddRelativeForce(new Vector2(0, v) * speed);
                }

                if (horizontalMovement)
                {
                    float v = Input.GetAxisRaw("Horizontal");
                    rb2d.AddRelativeForce(new Vector2(v, 0) * speed);
                }
                break;

            default:
                break;
        }

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
        if (isWrappingX && isWrappingY)
        {
            return;
        }
        //Get ship's current position:
        Vector3 newPosition = transform.position;

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
        {
            if (renderer.isVisible)
            {
                return true;
            }
        }
        //Returns false if spaceship is not in screen:
        return false;
    }

    public void ChangeSpeed(float speedToChange)
    {
        speed = speedToChange;
    }
}