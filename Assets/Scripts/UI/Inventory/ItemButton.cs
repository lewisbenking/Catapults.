using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {

    private Image itemImage;
    private Text quantityText;
    private Item item;
    private Transform toolTip;
    private InventoryManager inventoryManager;

    private bool consumable;
    private int itemID;
    

	// Use this for initialization
	void Start () {
        toolTip = transform.parent.Find("Tooltip");
        inventoryManager = transform.parent.GetComponent<InventoryManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (consumable)
        {
            quantityText.text = inventoryManager.getItemQuantity(itemID).ToString();
        }
	}

    public void setItem(Item item)
    {
        quantityText = transform.Find("quantityText").GetComponent<Text>();
        itemImage = transform.Find("ItemImage").GetComponent<Image>();

        quantityText.text = "∞";

        this.item = item;
        consumable = item.getIsConsumable();
        itemImage.sprite = item.getImage();
    }

    public void showToolTip()
    {
        toolTip.GetComponent<Tooltip>().fadeIn();
        toolTip.GetComponent<Tooltip>().setToolTipText(item.getName());

    }

    public void hideToolTip()
    {
        toolTip.GetComponent<Tooltip>().fadeOut();
    }

    public void useItem()
    {
        if (consumable)
        {
            if (inventoryManager.getItemQuantity(itemID) > 0)
            {
                inventoryManager.setCurrentItem(item.getId());
            }
            else
            {
                //TODO:
                print("Implement 'no items left' for a consumable item type, then delete this print message");
            }
        }
        else //If not a consumable item
        {
            //switch to item
        }
    }

    public int getQuantity()
    {
        return inventoryManager.getItemQuantity(itemID);
    }

    public void setItemID(int itemID)
    {
        this.itemID = itemID;
    }
}
