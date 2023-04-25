using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Item : MonoBehaviour
{
    private string name;
    private string description;
    private Image icon;
    private int amount;

    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        return description;
    }

    public Image GetIcon()
    {
        return icon;
    }

    public int GetAmount()
    {
        return amount;
    }

    public void SetName(string _name)
    {
        name = _name;
    }

    public void SetDescription(string _descr)
    {
        description = _descr;
    }

    public void SetIcon(Image _icon)
    {
        icon = _icon;
    }

    public void SetAmount(int amnt)
    {
        amount = amnt;
    }

    public Item CreateNullItem()
    {
        Item nullItem = new Item();
        nullItem.SetAmount(0);
        nullItem.SetDescription("");
        nullItem.SetIcon(null);
        nullItem.SetName("");
        return nullItem;
    }
}
