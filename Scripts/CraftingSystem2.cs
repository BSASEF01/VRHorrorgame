using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem2 : MonoBehaviour
{
    public Item[] craftable_items;
    public Item [,] crafting_recipes;
    public GameObject item_spawn;

    public Item item_1;
    public Item item_2;
    bool ejected = false;

    private void Start()
    {
        crafting_recipes = new Item[craftable_items.Length, 2];
        for(int i = 0; i < craftable_items.Length; i++) 
        {
            crafting_recipes[i, 0] = craftable_items[i].GetComponent<CraftableItem>().crafting_materials[0];
            crafting_recipes[i, 1] = craftable_items[i].GetComponent<CraftableItem>().crafting_materials[1];
        }
    }

    void OnTriggerEnter(Collider col) 
    {
        if(col.tag == "Item") 
        {
            if (item_1 == null) item_1 = col.GetComponent<Item>();
            else if (item_2 == null) item_2 = col.GetComponent<Item>();
            else StartCoroutine(eject_item(col));

        }
        else eject_item(col);
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Item" && !ejected)
        {
            if (item_1 == col.GetComponent<Item>()) item_1 = null;
            else if (item_2 == col.GetComponent<Item>()) item_2 = null;
            else Debug.Log("Item is not in the table");
        }
        else Debug.Log("object is not and item");
    }

    IEnumerator eject_item(Collider col) 
    {
        ejected = true;
        col.GetComponent<Rigidbody>().AddForce(new Vector3(200, 200, 0));
        yield return new WaitForSeconds(2);
        ejected = false;
    }

    public void craft()
    {
        if(item_1 == null || item_2 == null) 
        {
            Debug.Log("not enough items");
            return;
        }

        for (int i = 0; i < craftable_items.Length; i++)
        {
            List<int> ids = new List<int>();
            ids.Add(crafting_recipes[i, 0].id);
            ids.Add(crafting_recipes[i, 1].id);

            if (ids.Contains(item_1.id) && ids.Contains(item_2.id)) 
            {
                Instantiate(craftable_items[i].item_prefab, item_spawn.transform.position, item_spawn.transform.rotation);
                //despawn other objects 
                item_1.GetComponentInParent<Transform>().gameObject.SetActive(false);
                item_2.GetComponentInParent<Transform>().gameObject.SetActive(false);
                item_1 = null;
                item_2 = null;
                return;
            }
        }

    }
}
