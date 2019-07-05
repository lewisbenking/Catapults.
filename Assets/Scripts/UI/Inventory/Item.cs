using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Item {

    private GameObject itemObject;
    private Sprite itemImage;

    private string itemName;
    private bool isConsumable;
    private bool isProjectile;
    private bool isPlaceable;
    private int itemId;

    public Item(int itemId, string itemName, bool isConsumable, bool isProjectile, bool isPlaceable, Object itemObject)
    {
        this.itemId = itemId;
        this.itemName = itemName;
        this.isConsumable = isConsumable;
        this.isProjectile = isProjectile;
        this.itemObject = (GameObject) itemObject;
        this.isPlaceable = isPlaceable;

        //get image component from GameObject as Item's low-res image value
        itemImage = this.itemObject.GetComponent<Image>().sprite;
        System.Console.Write(itemImage);
    }

    public int getId()
    {
        return itemId;
    }

    public Sprite getImage()
    {
        return itemImage;
    }

    public string getName()
    {
        return itemName;
    }

    public bool getIsConsumable()
    {
        return isConsumable;
    }

    public bool getIsProjectile()
    {
        return isProjectile;
    }

    public bool getIsPlaceable()
    {
        return isPlaceable;
    }

    public GameObject getGameObject()
    {
        return itemObject;
    }

}
