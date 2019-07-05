using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    
    private ItemDatabase itemDatabase;
    private List<GameObject> itemButtons;
    private Transform currentItemUI;
    private GameController gameController;
    private int currentItem;
    public int[][] inventories;
    public int currentPlayer = 0;

    // Use this for initialization
    void Start()
    {
        itemDatabase = GameObject.Find("_GameManager").transform.Find("ItemDatabase").GetComponent<ItemDatabase>();
        gameController = GameObject.Find("_GameManager").GetComponent<GameController>();
        currentItemUI = transform.parent.Find("CurrentItem");
        itemButtons = new List<GameObject>();
        inventories = new int[8][]; //initialises the inventories - constrains max number of inventories to 12;

        loadUIItems();
        gameObject.SetActive(false);
    }

    //method must be called before use of script
    public void initialiseInventories(int numPlayers)
    {
        Start(); //ensure the script is initialised
        for (int i = 0; i < numPlayers; i++)
        {
            //itemDatabase.getStartingInventory().CopyTo(inventories[i], 12);// sets inventories to start with default inventory defined in ItemDatabase
            inventories[i] = (int[]) itemDatabase.getStartingInventory().Clone();
        }
    }

    /// <summary> Initialises the UI elements of the display inventory </summary>
    private void loadUIItems()
    {
        GameObject buttonPrefab = (GameObject)Resources.Load("ItemButton");
        //for each item in the item database
        for (int i = 0; i < itemDatabase.getNumItems(); i++)
        {
            GameObject itemButton = Instantiate(buttonPrefab);
            //create an ItemButton prefab and add to itemButtons list
            int rowNum = ((i / 4) - ((i / 4) % 1));
            Vector3 pos = new Vector3((-90 + ((i % 4) * 60)), 65 + (rowNum * -80), 0); //-90 is x starting point , then add 60 * the current collumn number

            itemButton.transform.SetParent(transform);
            itemButton.transform.localPosition = pos;
            itemButton.transform.localScale = new Vector3(1,1,1);
            itemButton.GetComponent<ItemButton>().setItem(itemDatabase.getItem(i));
            itemButton.GetComponent<ItemButton>().setItemID(i);

            itemButtons.Add(itemButton);
        }
    }

    /// <summary> Returns the contents of the display inventory as a string </summary>
    public string invToString(int[] inv)
    {
        string contents = "empty";
        if (inv != null && inv.Length != 0)
        {
            contents = "";
            for (int i = 0; i < inv.Length; i++)
            {
                contents += "(" + inv[i] + " " + itemDatabase.getItem(i).getName() + ")";
            }
        }
        return contents;
    }

    /// <summary> Add 1 item of id itemID to current inventory array </summary>
    public void addItem(int itemId)
    {
        inventories[currentPlayer][itemId] += 1;
    }

    /// <summary> Remove 1 item of id itemID from displayInventory array </summary>
    public Item removeItem(int itemId)
    {
        if (inventories[currentPlayer][itemId] > 0) //if the player has >0 of item
        {
            inventories[currentPlayer][itemId]--;
            return itemDatabase.getItem(itemId);
        }
        else if(itemDatabase.getItem(itemId).getIsConsumable() == false) //if the item is not consumable (unlimited)
        {
            return itemDatabase.getItem(itemId);
        }
        else //if the player has none of the item
        {
            print("no items of type " + itemId + " to remove. TODO: implement this msg as tooltip");
            return itemDatabase.getItem(0); //return rock (default)
        }
    }

    /// <summary> Checks if inventory has > 0 of item, if so: sets currentItem to newCurrentItem and changes current item UI element </summary>
    public void setCurrentItem(int newCurrentItemID)
    {
        //if the player has at least one of item or if item is not consumable (unlimited)
        if(inventories[currentPlayer][newCurrentItemID] > 0 || itemDatabase.getItem(newCurrentItemID).getIsConsumable() == false)
        {
            currentItem = newCurrentItemID;

            if(itemDatabase.getItem(newCurrentItemID).getIsPlaceable() == true) //if the new item is a place-able item
            {
                //load item into world
                GameObject placeableItem = (GameObject) Resources.Load("Items/" + itemDatabase.getItem(newCurrentItemID).getName());
                GameObject newObject = Instantiate(placeableItem) as GameObject;
                newObject.SendMessage("setPlayerOwner", gameController.getCurrentPlayer().transform);
            }

            //set image and text of current item UI element
            currentItemUI.Find("ItemImage").GetComponent<Image>().sprite = itemDatabase.getItem(currentItem).getImage();
            currentItemUI.Find("ItemName").GetComponent<Text>().text = itemDatabase.getItem(currentItem).getName();
        }
        else
        {
            print("can't set current item to item of itemId " + newCurrentItemID + " TODO: implement this msg as tooltip");
        }
    }

    /// <summary> Get the current quantity of a specific item </summary>
    public int getItemQuantity(int itemID)
    {
        return inventories[currentPlayer][itemID];
    }

    /// <summary> Get the displayed current in-use item </summary>
    public int getCurrentItem()
    {
        return currentItem;
    }

    /// <summary> Set the current player for the script to reference when displaying the current inventory </summary>
    public void setCurrentPlayer(int playerNum)
    {
        currentPlayer = playerNum;
    }
}
