using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static List<Block> blockList;
    public static Block target;
    public static Block selectedObject;
    public Standable attachedPlatform;
    protected bool touchPlayer;
    protected bool blockEnabled;
    public bool canBeTrade;
    private List<GameObject> childList;
    private BoxCollider2D objCollider;
    private Rigidbody2D rb2d;
    private bool isBlock;

    public BoxCollider2D Collider => objCollider;
    public Rigidbody2D Rb2d => rb2d;

    public static void cleanUp()
    {
        if (blockList != null)
        {
            blockList.Clear();
        }
        target = null;
        selectedObject = null;
    }

    protected virtual void Awake()
    {
        if (blockList == null)
            blockList = new List<Block>();

        blockList.Add(this);

        objCollider = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        childList = new List<GameObject>();
        blockEnabled = true;
        isBlock = true;

        for (int i = 0; i < transform.childCount; i++)
        {
            childList.Add(transform.GetChild(i).gameObject);
        }
    }

    protected virtual void selectObject(bool b, Block lastObject = null)
    {
        if (b)
        {
            if (selectedObject != null && selectedObject != this)
            {
                selectedObject.selectObject(false, this);
            }
            selectedObject = this;
            target = this;
        }
        else
        {
            if (selectedObject == this)
            {
                selectedObject = null;
                target = null;
            }
        }
    }

    public virtual bool conditionCheck()
    {
        return blockEnabled && isBlock;
    }

    public void clickOn()
    {
        if (!blockEnabled)
            return;

        if (conditionCheck())
        {
            selectObject(true);
        }
    }

    public static void unSelect()
    {
        if (selectedObject != null)
        {
            selectedObject.selectObject(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            touchPlayer = true;
        }

        Standable platform = other.gameObject.GetComponent<Standable>();
        if (platform != null)
        {
            attachedPlatform = platform;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            touchPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            touchPlayer = false;
        }

        if (attachedPlatform != null && other.gameObject.GetComponent<Standable>() == attachedPlatform)
        {
            attachedPlatform = null;
        }
    }

    public void unattach()
    {
        if (attachedPlatform != null)
        {
            attachedPlatform = null;
        }

        if (rb2d != null)
        {
            rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
    }

   private void FixedUpdate()
{
    GameObject player = PlayerPlatformerController._instance != null ? PlayerPlatformerController._instance.gameObject : null;
    float dist = float.MaxValue;
    float range = 2f; // fallback

    if (PlayerPlatformerController._instance != null)
        range = PlayerPlatformerController._instance.attachRange;

    if (player != null)
        dist = Vector2.Distance(transform.position, player.transform.position);

    // ✅ Only mark as target and show button, do not auto-attach
    if ((dist <= range && conditionCheck()) || selectedObject == this)
    {
        target = this;
        AttachBlock attachBlock = this as AttachBlock;
        if (attachBlock != null && UIManager._instance != null)
        {
            UIManager._instance.showActiveButton(true, true);
        }
    }
    else if (target == this)
    {
        target = null;
        if (UIManager._instance != null)
        {
            UIManager._instance.showActiveButton(false);
        }
    }
}
}

