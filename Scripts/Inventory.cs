using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SpecialInteractable
{
    public int inventory_size = 20;
    public List<Item> item_inventory;
    public GameObject item_spawn;


    // Start is called before the first frame update
    void Start()
    {
        item_inventory.ForEach(delegate(Item i) 
        {
            Instantiate(i.item_prefab, item_spawn.transform.position, item_spawn.transform.rotation);
        }
        );
    }


    public List<Item> getItems() 
    {
        return item_inventory;
    }


    //Add item to inventory
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Item>() != null) 
        {
            if (item_inventory.Count <= inventory_size)
            {
                item_inventory.Add(other.gameObject.GetComponent<Item>());
            }else 
            {
                print("Inventory full");
            }
        }
    }


    //TODO fix issues
    //public void retrieveItem(Item item) 
    //{
    //    if (item_inventory.Contains(item))
    //    {
    //        GameObject new_item =  Instantiate(item.item_prefab, item_spawn.transform.position, item_spawn.transform.rotation);
    //        item_inventory.Remove(item);
    //        return;
    //    }
    //    else
    //    {
    //        Debug.Log("Item not found");
    //        return;
    //    }

    //}

    //public void AddItem(Item item) 
    //{
    //    if (item_inventory.Count <= inventory_size)
    //    {
    //        item_inventory.Add(item);
    //    }
    //    else 
    //    {
    //        print("Inventory full");
    //    }

    //}


    public override void action() 
    {
        base.action();
    }

 

}
