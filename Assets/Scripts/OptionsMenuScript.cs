using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject batterySaverOption;
    [SerializeField] private GameObject swapSticksToggle;
    
    // Start is called before the first frame update
    void Start()
    {
        
        #if UNITY_IOS || UNITY_ANDROID
            batterySaverOption.SetActive(true);
            swapSticksToggle.SetActive(true);
        #endif
        
        #if UNITY_STANDALONE || UNITY_WEBGL
            batterySaverOption.SetActive(false);
            swapSticksToggle.SetActive(false);
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
