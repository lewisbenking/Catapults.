using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {

    private List<Item> items;
    private int[] startingInventory = { 1, 3, 1, 1, 1, 3, 1, 2, 2, 0, 0, 0 };

	// Use this for initialization
	void Start () {
        items = new List<Item>();

        //Add predefined Items with Objects from Resources folder
        items.Add(new Item(0,"Rock", false, true, false,Resources.Load("Items/Rock")));
        items.Add(new Item(1,"FlamingRock", true, true, false,Resources.Load("Items/FlamingRock")));
        items.Add(new Item(2, "Barrier", true, false, true, Resources.Load("Items/Barrier")));
        items.Add(new Item(3,"Fragmentation Bomb", true, true, false,Resources.Load("Items/FragBomb")));
        items.Add(new Item(4, "Posion Vial", true, true, false, Resources.Load("Items/PoisonVial")));
        items.Add(new Item(5, "Spike Scatter", true, true, false, Resources.Load("Items/SpikeBallClump")));
        items.Add(new Item(6, "Repair", true, false, false, Resources.Load("Items/Pickups/Repair")));
        items.Add(new Item(7, "FusedBomb", true, true, false, Resources.Load("Items/FusedBomb")));
        items.Add(new Item(8, "Lightning Strike", true, false, false, Resources.Load("Items/Lightning")));

        //Placeholder items - not yet implemented
        items.Add(new Item(9, "placeholder", true, false, false, Resources.Load("Items/Placeholder")));
        items.Add(new Item(10, "placeholder", true, false, false, Resources.Load("Items/Placeholder")));
        items.Add(new Item(11, "placeholder", true, false, false, Resources.Load("Items/Placeholder")));
    }

    public Item getItem(int index)
    {
        return items[index]; //casts Item to GameObject & resturns
    }

    public int getNumItems()
    {
        if (items!= null)
        {
            return items.Count;
        }
        return 0;
    }

    public int[] getStartingInventory()
    {
        return startingInventory;
    }
}
