using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPDemoProductUI : MonoBehaviour
{
	public Button purchaseButton;

	public Button receiptButton;

	public Text titleText;

	public Text descriptionText;

	public Text priceText;

	public Text statusText;

	private string m_ProductID;

	private Action<string> m_PurchaseCallback;

	private string m_Receipt;

	public void SetProduct(Product p, Action<string> purchaseCallback)
	{
	}

	public void SetPendingTime(int secondsRemaining)
	{
	}

	public void PurchaseButtonClick()
	{
	}

	public void ReceiptButtonClick()
	{
	}
}
