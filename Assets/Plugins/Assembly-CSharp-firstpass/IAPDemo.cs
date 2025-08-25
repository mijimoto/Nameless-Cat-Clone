using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPDemo : MonoBehaviour, IStoreListener
{
	private IStoreController m_Controller;

	private IAppleExtensions m_AppleExtensions;

	private IMoolahExtension m_MoolahExtensions;

	private ISamsungAppsExtensions m_SamsungExtensions;

	private IMicrosoftExtensions m_MicrosoftExtensions;

	private ITransactionHistoryExtensions m_TransactionHistoryExtensions;

	private IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;

	private bool m_IsGooglePlayStoreSelected;

	private bool m_IsSamsungAppsStoreSelected;

	private bool m_IsCloudMoolahStoreSelected;

	private bool m_PurchaseInProgress;

	private Dictionary<string, IAPDemoProductUI> m_ProductUIs;

	public GameObject productUITemplate;

	public RectTransform contentRect;

	public Button restoreButton;

	public Text versionText;

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
	{
		return default(PurchaseProcessingResult);
	}

	public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
	{
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
	}

	public void Awake()
	{
	}

	private void OnTransactionsRestored(bool success)
	{
	}

	private void OnDeferred(Product item)
	{
	}

	private void InitUI(IEnumerable<Product> items)
	{
	}

	public void PurchaseButtonClick(string productID)
	{
	}

	public void RestoreButtonClick()
	{
	}

	private void ClearProductUIs()
	{
	}

	private void AddProductUIs(Product[] products)
	{
	}

	private void UpdateProductUI(Product p)
	{
	}

	private void UpdateProductPendingUI(Product p, int secondsRemaining)
	{
	}

	private bool NeedRestoreButton()
	{
		return false;
	}

	private void LogProductDefinitions()
	{
	}
}
