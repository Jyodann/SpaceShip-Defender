using UnityEngine;

public class BoundingBoxScript : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        //This script is here to do Optimisation: It destorys anything out of the player's current
        //vision
        //https://www.youtube.com/watch?v=GIatyq9KT28
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}