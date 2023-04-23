using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Item[] items;
    [SerializeField] int invSize;
    // Start is called before the first frame update
    void Start()
    {
        items = new Item[invSize];
    }

    void PickUp()
    {
    }
}
