using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEngine.Purchasing
{
	public class IAPButton : MonoBehaviour
	{
		public enum ButtonType
		{
			Purchase = 0,
			Restore = 1
		}

		[Serializable]
		public class OnPurchaseCompletedEvent : UnityEvent<Product>
		{
		}

		[Serializable]
		public class OnPurchaseFailedEvent : UnityEvent<Product, PurchaseFailureReason>
		{
		}

		[HideInInspector]
		public string productId;

		public ButtonType buttonType;

		public bool consumePurchase;

		public OnPurchaseCompletedEvent onPurchaseComplete;

		public OnPurchaseFailedEvent onPurchaseFailed;

		public Text titleText;

		public Text descriptionText;

		public Text priceText;

		private void Start()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void PurchaseProduct()
		{
		}

		private void Restore()
		{
		}

		private void OnTransactionsRestored(bool success)
		{
		}

		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
		{
			return default(PurchaseProcessingResult);
		}

		public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
		{
		}

		internal void UpdateText()
		{
		}
	}
}
