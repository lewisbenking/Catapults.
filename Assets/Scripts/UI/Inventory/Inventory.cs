using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    private GameObject gameManager;
    private ItemDatabase itemDatabase;
    public int[] ownedItems;

    private int currentItem;

    // Use this for initialization
    void Start()
    {
        itemDatabase = GameObject.Find("_GameManager").transform.Find("ItemDatabase").GetComponent<ItemDatabase>();
        ownedItems = new int[12]; //inventory of size 12

        initialiseOwnedItems();
        currentItem = 0; //start with rock
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void addItem(int itemId)
    {
        ownedItems[itemId] += 1;
    }

    public void removeItem(int itemId)
    {
        if (ownedItems[itemId] > 0)
        {
            ownedItems[itemId] -= 1;
        }
    }

    public void setCurrentItem(int itemId)
    {
        if(ownedItems[itemId] > 0)
        {
            currentItem = itemId;
        }
        else
        {
            print("Inventory is: " + ownedItems[0] + ownedItems[1] + ownedItems[2] + ownedItems[3]);
            print("no items of itemId " + itemId + " TODO: implement this msg as tooltip");
        }
    }

    public int getCurrentItem()
    {
        return currentItem;
    }

    public int[] getInventory()
    {
        print("getting inventory of " + transform.name + " as " + ownedItems[0] + ownedItems[1] + ownedItems[2] + ownedItems[3]);
        if (ownedItems == null)
        {
            initialiseOwnedItems();
        }
        return ownedItems;
    }

    public void setInventory(int[] newOwnedItems)
    {
        ownedItems = newOwnedItems;
        print("setting inventory of " + transform.name + " as " + ownedItems[0] + ownedItems[1] + ownedItems[2] + ownedItems[3]);
    }

    private void initialiseOwnedItems()
    {
        ownedItems = itemDatabase.getStartingInventory();
        //print(ownedItems[0].ToString() + ownedItems[1].ToString() + ownedItems[2].ToString() + ownedItems[3].ToString());
        /*
        //Initialise owned items
        ownedItems[0] = 1; //Rock
        ownedItems[1] = 0; //FlamingRock
        ownedItems[2] = 0; //Repair
        ownedItems[3] = 0; //Armour
        */
    }
}
