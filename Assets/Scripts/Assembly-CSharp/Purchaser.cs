using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour, IStoreListener
{
	private const string LOG_KEY = "IAP";

	public static Purchaser _instance;

	private static IStoreController m_StoreController;

	private static IExtensionProvider m_StoreExtensionProvider;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	public void InitializePurchasing()
	{
	}

	public void InitializePurchased()
	{
	}

	public void ReloadPurchasedItem()
	{
	}

	public string getProductPrice(int productId)
	{
		return null;
	}

	public void updateItem(int id, bool has = true)
	{
	}

	public void updateItemUI(int id, bool has = true)
	{
	}

	public bool IsInitialized()
	{
		return false;
	}

	public void BuyNonConsumableSkin(int skinIndex)
	{
	}

	public void removeAd()
	{
	}

	private void BuyProductID(string productId)
	{
	}

	public void RestorePurchases(Action successCallback = null, Action failCallback = null)
	{
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		return default(PurchaseProcessingResult);
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
	}
}
