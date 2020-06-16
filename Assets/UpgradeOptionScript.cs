using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeOptionScript : MonoBehaviour
{
    public Text UpgradeLabel;
    public Transform shipLocation;

    // Update is called once per frame
    private void Update()
    {
        transform.position = new Vector3(shipLocation.position.x, shipLocation.position.y + 10);
        Vector3 namePose = Camera.main.WorldToScreenPoint(transform.position);
        UpgradeLabel.transform.position = namePose;
    }
}