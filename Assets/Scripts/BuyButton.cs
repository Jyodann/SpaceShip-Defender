using System.Collections;
using TMPro;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public enum ItemType
    {
        Dollar1,
        Dollar5,
        Dollar10
    }

    public ItemType itemType;

    private string defaultText;

    private TextMeshProUGUI priceText;

    private void Start()
    {
        priceText = GetComponentInChildren<TextMeshProUGUI>();
        defaultText = priceText.text;

        StartCoroutine(LoadPrice());
    }

    public void ClickBuy()
    {
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
    }

    private IEnumerator LoadPrice()
    {
        while (!IAPManager.Instance.IsInitialized()) yield return null;

        var loadedPrice = string.Empty;

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
    }
}