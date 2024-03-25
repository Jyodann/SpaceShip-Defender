using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public enum ItemType
    {
        Dollar1, Dollar5, Dollar10
    }

    public ItemType itemType;

    private TextMeshProUGUI priceText;

    private string defaultText;

    private void Start()
    {
        priceText = GetComponentInChildren<TextMeshProUGUI>();
        defaultText = priceText.text;

        StartCoroutine(LoadPrice());
    }

    public void ClickBuy()
    {
        throw new NotImplementedException();
        /*
        switch (itemType)
        {
            case ItemType.Dollar1:
                IAPManager.Instance.Buy1Dollar();
                break;
            case ItemType.Dollar5:
                IAPManager.Instance.Buy5Dollar();
                break;
            case ItemType.Dollar10:
                IAPManager.Instance.Buy10Dollar();
                break;
            
        }
        */
    }

    IEnumerator LoadPrice()
    {
        throw new NotImplementedException();
        /*
        while (!IAPManager.Instance.IsInitialized())
        {
            yield return null;
        }
        
        var loadedPrice = String.Empty;
        
        switch (itemType)
        {
            case ItemType.Dollar1:
                loadedPrice = IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.ConsumableProducts[0]);
                break;
            case ItemType.Dollar5:
                loadedPrice = IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.ConsumableProducts[1]);
                break;
            case ItemType.Dollar10:
                loadedPrice = IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.ConsumableProducts[2]);
                break;
            
        }

        priceText.text += " " + loadedPrice;
*/
    }
}
