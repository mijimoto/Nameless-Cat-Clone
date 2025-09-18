using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SkinBlock : MonoBehaviour
{
    [SerializeField] private Image catImage;
    [SerializeField] private Image bgImage;
    [SerializeField] private GameObject priceCanObj;
    [SerializeField] private GameObject priceIAPObj;
    [SerializeField] private TextMeshProUGUI priceCanText;
    [SerializeField] private TextMeshProUGUI priceIAPText;

    // cached skin object assigned via LoadSkin
    private Skin skinData;

    public void LoadSkin(Skin skinData)
    {
        this.skinData = skinData;

        if (skinData == null)
        {
            // nothing to load
            return;
        }

        // UI visibility
        if (priceCanObj != null) priceCanObj.SetActive(!skinData.useIAP);
        if (priceIAPObj != null) priceIAPObj.SetActive(skinData.useIAP);
        if (priceCanText != null) priceCanText.text = skinData.price.ToString();
        if (priceIAPText != null) priceIAPText.text = skinData.useIAP ? "IAP" : "";

        // Try to map the current catImage sprite (base) to the skin variant using your naming rules.
        // base example: "player_jump(white)_0" -> skin example: "player_jump_skin_3_0"
        if (catImage != null)
        {
            Sprite baseSprite = catImage.sprite;
            Sprite newSprite = null;

            if (baseSprite != null)
            {
                string baseName = baseSprite.name; // e.g., player_run(white)_1
                // parse frame suffix (underscore + frame)
                int lastUnderscore = baseName.LastIndexOf('_');
                string frame = "0";
                string prefix = baseName;
                if (lastUnderscore >= 0 && lastUnderscore < baseName.Length - 1)
                {
                    frame = baseName.Substring(lastUnderscore + 1);
                    prefix = baseName.Substring(0, lastUnderscore);
                }

                // remove "(white)" or other parentheses if present
                prefix = Regex.Replace(prefix, @"\([^\)]*\)", "");

                // build candidate name
                string candidateName = $"{prefix}_skin_{skinData.id}_{frame}";

                // Try runtime Resources first
#if UNITY_EDITOR
                // In editor, try AssetDatabase lookup in Assets/Sprite for convenience
                string[] guids = AssetDatabase.FindAssets(candidateName + " t:Sprite", new[] { "Assets/Sprite" });
                if (guids != null && guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    newSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                }
#endif
                if (newSprite == null)
                {
                    // Try Resources
                    Sprite[] loaded = Resources.LoadAll<Sprite>("Sprite");
                    if (loaded != null)
                    {
                        for (int i = 0; i < loaded.Length; i++)
                        {
                            if (loaded[i] != null && loaded[i].name == candidateName)
                            {
                                newSprite = loaded[i];
                                break;
                            }
                        }
                    }
                }
            }

            // If found, assign; otherwise fall back to a sprite stored in skinData.sprite or keep base
            if (newSprite != null)
            {
                catImage.sprite = newSprite;
            }
            else if (skinData.sprite != null)
            {
                catImage.sprite = skinData.sprite;
            }
            // else keep whatever is set in the scene (base)
        }
    }
}
