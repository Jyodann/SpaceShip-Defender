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

    private Renderer[] renderers;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        verticalMovement = Input.GetButton("Vertical");
        horizontalMovement = Input.GetButton("Horizontal");
    }

    private void FixedUpdate()
    {
        if (verticalMovement)
        {
            float v = Input.GetAxisRaw("Vertical");

            rb2d.AddRelativeForce(new Vector2(0, v).normalized * speed);
        }

        if (horizontalMovement)
        {
            float v = Input.GetAxisRaw("Horizontal");
            transform.Rotate(new Vector3(0, 0, -v) * rotateSpeed);
        }

        ScreenWrap();
    }

    private void ScreenWrap()
    {
        if (CheckRenderers())
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if (isWrappingX && isWrappingY)
        {
            return;
        }

        Vector3 newPosition = transform.position;

        if (newPosition.x > 1 || newPosition.x < 0)
        {
            newPosition.x = -newPosition.x;
            isWrappingX = true;
        }

        if (newPosition.y > 1 || newPosition.y < 0)
        {
            newPosition.y = -newPosition.y;
            isWrappingY = true;
        }

        transform.position = newPosition;
    }

    private bool CheckRenderers()
    {
        foreach (var renderer in renderers)
        {
            if (renderer.isVisible)
            {
                return true;
            }
        }

        return false;
    }

    public void ChangeSpeed(float speedToChange)
    {
        speed = speedToChange;
    }
}