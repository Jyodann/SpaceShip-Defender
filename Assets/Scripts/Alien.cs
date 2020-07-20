using UnityEngine;

public class Alien : MonoBehaviour
{
    private Player playerCharacter;
    public float speed = 10f;
    public bool isFrozen = false;

    // Start is called before the first frame update
    private void Start()
    {
        playerCharacter = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isFrozen)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerCharacter.transform.position, speed * Time.deltaTime);
        }
    }
}