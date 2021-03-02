using UnityEngine;
using UnityEngine.UI;

public class UpgradeOptionScript : MonoBehaviour
{
    /// <summary>
    ///     Text following object script provided by: https://www.youtube.com/watch?v=0bvDmqqMXcA
    /// </summary>

    //The "Upgrade" text can be set in the inspector:
    [SerializeField] private Text upgradeLabel;

    //Caches the transform of the location of the Ship:
    private Transform shipLocation;

    private void Start()
    {
        shipLocation = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    private void Update()
    {
        //Changes the position of the label to ship location:
        transform.position = new Vector3(shipLocation.position.x, shipLocation.position.y + 10);
        //Converts position of label from WorldSpace, to screenSpace:
        var namePose = Camera.main.WorldToScreenPoint(transform.position);
        //Sets the position of the label to be the ScreenSpace Location:
        upgradeLabel.transform.position = namePose;
    }
}