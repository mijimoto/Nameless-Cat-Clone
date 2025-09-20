using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour, IStoreListener
{
    private const string LOG_KEY = "IAP";
    public static Purchaser _instance;
    
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
    
    // Product IDs - MUST match exactly what you set in Google Play Console
    private const string REMOVE_ADS = "remove_ads";
    private const string SKIN_PREFIX = "skin_"; // e.g., skin_3, skin_6, skin_9
    
    // Events for purchase callbacks
    public static System.Action<string> OnPurchaseSuccess;
    public static System.Action<string> OnPurchaseFail;
    public static System.Action OnInitializationComplete;
    
    // Dictionary to track owned items and prices
    private Dictionary<int, bool> ownedItems = new Dictionary<int, bool>();
    private Dictionary<string, string> productPrices = new Dictionary<string, string>();
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }
        
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        
        // Add your products here
        builder.AddProduct(REMOVE_ADS, ProductType.NonConsumable);
        
        // Add IAP skin products - only the ones that are actually IAP
        // Based on your logic: skin IDs 3, 6, 9, 12, 15, 18, 21, 24, 27, 30
        int[] iapSkinIds = GetIAPSkinIds();
        foreach (int skinId in iapSkinIds)
        {
            builder.AddProduct(SKIN_PREFIX + skinId, ProductType.NonConsumable);
        }
        
        UnityPurchasing.Initialize(this, builder);
    }
    
    private int[] GetIAPSkinIds()
    {
        // Generate IAP skin IDs based on your logic: every 3rd skin starting from 3
        List<int> iapIds = new List<int>();
        for (int i = 3; i <= 30; i += 3) // Adjust max based on your total skins
        {
            iapIds.Add(i);
        }
        return iapIds.ToArray();
    }
    
    public bool IsIAPSkin(int skinId)
    {
        // Check if skin should use IAP based on your existing logic
        return skinId % 3 == 0 && skinId > 0;
    }
    
    public void InitializePurchased()
    {
        if (!IsInitialized())
        {
            Debug.Log(LOG_KEY + ": Store not initialized");
            return;
        }
        
        // Check all owned products and cache prices
        foreach (Product product in m_StoreController.products.all)
        {
            // Cache the price
            if (product.metadata != null)
            {
                productPrices[product.definition.id] = product.metadata.localizedPriceString;
            }
            
            if (product.hasReceipt)
            {
                Debug.Log(LOG_KEY + ": Owned product: " + product.metadata.localizedTitle);
                
                if (product.definition.id == REMOVE_ADS)
                {
                    PlayerPrefs.SetInt("RemoveAds", 1);
                }
                else if (product.definition.id.StartsWith(SKIN_PREFIX))
                {
                    string skinNumberStr = product.definition.id.Replace(SKIN_PREFIX, "");
                    if (int.TryParse(skinNumberStr, out int skinId))
                    {
                        updateItem(skinId, true);
                    }
                }
            }
        }
        
        // Notify that initialization is complete
        OnInitializationComplete?.Invoke();
    }
    
    public void ReloadPurchasedItem()
    {
        InitializePurchased();
    }
    
    public string getProductPrice(int skinId)
    {
        if (!IsInitialized())
        {
            return "$0.99"; // Default price
        }
        
        string productKey = SKIN_PREFIX + skinId;
        
        // Return cached price if available
        if (productPrices.ContainsKey(productKey))
        {
            return productPrices[productKey];
        }
        
        // Try to get price from store controller
        Product product = m_StoreController.products.WithID(productKey);
        if (product != null && product.metadata != null)
        {
            string price = product.metadata.localizedPriceString;
            productPrices[productKey] = price; // Cache it
            return price;
        }
        
        return "$0.99"; // Default price
    }
    
    public void updateItem(int id, bool has = true)
    {
        ownedItems[id] = has;
        
        // Save to PlayerPrefs using the same key format as your Skin class
        PlayerPrefs.SetInt($"SkinOwned_{id}", has ? 1 : 0);
        PlayerPrefs.Save();
        
        updateItemUI(id, has);
    }
    
    public void updateItemUI(int id, bool has = true)
    {
        Debug.Log(LOG_KEY + ": Updating UI for item " + id + " to " + (has ? "owned" : "not owned"));
        
        // Find and update SkinMenu if it exists
        SkinMenu skinMenu = FindObjectOfType<SkinMenu>();
        if (skinMenu != null)
        {
            skinMenu.updateAllSkinUI();
        }
    }
    
    public bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }
    
    public void BuyNonConsumableSkin(int skinIndex)
    {
        string productId = SKIN_PREFIX + skinIndex;
        BuyProductID(productId);
    }
    
    public void removeAd()
    {
        BuyProductID(REMOVE_ADS);
    }
    
    private void BuyProductID(string productId)
    {
        if (!IsInitialized())
        {
            Debug.LogError(LOG_KEY + ": BuyProductID FAIL. Not initialized.");
            OnPurchaseFail?.Invoke(productId);
            return;
        }
        
        Product product = m_StoreController.products.WithID(productId);
        
        if (product != null && product.availableToPurchase)
        {
            Debug.Log(LOG_KEY + ": Purchasing product asynchronously: " + product.definition.id);
            m_StoreController.InitiatePurchase(product);
        }
        else
        {
            Debug.LogError(LOG_KEY + ": BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            OnPurchaseFail?.Invoke(productId);
        }
    }

    public void RestorePurchases(Action successCallback = null, Action failCallback = null)
    {
        if (!IsInitialized())
        {
            Debug.LogError(LOG_KEY + ": RestorePurchases FAIL. Not initialized.");
            failCallback?.Invoke();
            return;
        }
        
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log(LOG_KEY + ": RestorePurchases started ...");
            
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) =>
            {
                Debug.Log(LOG_KEY + ": RestorePurchases continuing: " + result + 
                         ". If no further messages, no purchases available to restore.");
                
                if (result)
                {
                    InitializePurchased();
                    successCallback?.Invoke();
                }
                else
                {
                    failCallback?.Invoke();
                }
            });
        }
        else
        {
            Debug.Log(LOG_KEY + ": RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            // On Android, just reinitialize purchased items
            InitializePurchased();
            successCallback?.Invoke();
        }
    }
    
    // IStoreListener Implementation
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log(LOG_KEY + ": OnInitialized: PASS");
        
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
        
        InitializePurchased();
    }
    
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError(LOG_KEY + ": OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError(LOG_KEY + ": OnInitializeFailed InitializationFailureReason:" + error + ", message: " + message);
    }
    
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string productId = args.purchasedProduct.definition.id;
        Debug.Log(LOG_KEY + ": ProcessPurchase: " + productId);
        
        if (productId == REMOVE_ADS)
        {
            Debug.Log(LOG_KEY + ": Remove Ads purchased");
            PlayerPrefs.SetInt("RemoveAds", 1);
            PlayerPrefs.Save();
            
            OnPurchaseSuccess?.Invoke(productId);
        }
        else if (productId.StartsWith(SKIN_PREFIX))
        {
            string skinNumberStr = productId.Replace(SKIN_PREFIX, "");
            if (int.TryParse(skinNumberStr, out int skinId))
            {
                Debug.Log(LOG_KEY + ": Skin " + skinId + " purchased");
                updateItem(skinId, true);
                OnPurchaseSuccess?.Invoke(productId);
            }
        }
        else
        {
            Debug.LogWarning(LOG_KEY + ": Unknown product purchased: " + productId);
        }
        
        return PurchaseProcessingResult.Complete;
    }
    
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError(LOG_KEY + ": OnPurchaseFailed: FAIL. Product: " + product.definition.storeSpecificId + 
                      ", PurchaseFailureReason: " + failureReason);
        
        OnPurchaseFail?.Invoke(product.definition.id);
    }
    
    // Helper methods
    public bool HasSkin(int skinId)
    {
        if (ownedItems.ContainsKey(skinId))
        {
            return ownedItems[skinId];
        }
        
        return PlayerPrefs.GetInt($"SkinOwned_{skinId}", 0) == 1;
    }
    
    public bool HasRemovedAds()
    {
        return PlayerPrefs.GetInt("RemoveAds", 0) == 1;
    }
}