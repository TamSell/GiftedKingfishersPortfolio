using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> items;
    [SerializeField] int invSize;
    private int place;
    // Start is called before the first frame update
    void Start()
    {
        items = new List<Item>();
    }

    void InvenAdd(Item _item = null)
    {
        if(items.Contains(_item))
        {
           for(int x = 0; x < items.Count; x++)
           {
                if (items[x].id == _item.id)
                {
                    place = x;
                }
           }
        }
        else
        {
            items.Add(_item);
        }
    }

    public Item InvenSelect(int place)
    {
        return items[place];
    }
}
