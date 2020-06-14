using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxScript : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}