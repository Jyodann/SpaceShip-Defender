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
        currentControlMode = GameManager.playerControlMode;
        rb2d = GetComponent<Rigidbody2D>();
        //Gets 4 of the screenbounds:
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        verticalMovement = Input.GetButton("Vertical");
        horizontalMovement = Input.GetButton("Horizontal");

        if (currentControlMode == GameManager.ControlMode.MixedMouseKeyboard)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector2 directionToFace = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            transform.up = directionToFace;
        }
    }

    private void FixedUpdate()
    {
        switch (currentControlMode)
        {
            case GameManager.ControlMode.KeyboardOnly:
                if (verticalMovement)
                {
                    float v = Input.GetAxisRaw("Vertical");
                    rb2d.AddRelativeForce(new Vector2(0, v) * speed);
                }

                if (horizontalMovement)
                {
                    float v = Input.GetAxisRaw("Horizontal");
                    transform.Rotate(new Vector3(0, 0, -v) * rotateSpeed);
                }
                break;
            case GameManager.ControlMode.MixedMouseKeyboard:
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