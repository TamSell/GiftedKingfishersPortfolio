using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> items;
    [SerializeField] int invSize;
    // Start is called before the first frame update
    void Start()
    {
        items = new List<Item>();
    }

    void PickUp(Item _item)
    {
        items.Add(_item);
    }
}
