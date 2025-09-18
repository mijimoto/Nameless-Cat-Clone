using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI msgText;
    [SerializeField] private Animator animator;
    
    public Color TextColor
    {
        get
        {
            if (msgText != null)
                return msgText.color;
            return Color.white;
        }
        set
        {
            if (msgText != null)
                msgText.color = value;
        }
    }

    public void Init(TextBoxSetting textBoxSetting)
    {
        if (textBoxSetting != null && msgText != null)
        {
            textBoxSetting.InitTextMeshProUGUI(msgText, true);
        }
    }

    public void UpdateText(string msg)
    {
        if (msgText != null)
        {
            msgText.text = msg;
        }
    }

    public void playAnimation(bool inverse)
    {
        if (animator != null)
        {
            if (inverse)
            {
                animator.SetTrigger("PlayReverse");
            }
            else
            {
                animator.SetTrigger("Play");
            }
        }
    }
}
