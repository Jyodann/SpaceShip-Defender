using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, 20f);
    }
}