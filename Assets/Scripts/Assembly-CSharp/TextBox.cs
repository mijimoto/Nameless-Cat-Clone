using TMPro;
using UnityEngine;
public class TextBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI msgText;
    [SerializeField]
    private Animator animator;
    public Color TextColor
    {
        get
        {
            return default(Color);
        }
        set
        {
        }
    }
    public void Init(TextBoxSetting textBoxSetting)
    {
    }
    public void UpdateText(string msg)
    {
    }
    public void playAnimation(bool inverse)
    {
    }
}