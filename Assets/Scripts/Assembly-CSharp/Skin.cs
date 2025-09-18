using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Nameless Cat/Skin")]
[System.Serializable]
public class Skin : ScriptableObject
{
    [SerializeField] public int id;
    [SerializeField] public bool useIAP;
    [SerializeField] public bool canBuy;
    [SerializeField] public bool canUse;
    [SerializeField] public RuntimeAnimatorController controller;
    [SerializeField] public Sprite sprite;
    [SerializeField] public int price;

    public void setCanUse(bool able)
    {
        canUse = able;
        // persist ownership by PlayerPrefs (simple approach)
        PlayerPrefs.SetInt($"SkinOwned_{id}", able ? 1 : 0);
        PlayerPrefs.Save();
    }
}
