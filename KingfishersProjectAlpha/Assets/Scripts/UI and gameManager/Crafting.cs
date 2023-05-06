using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    private Item currentAttach;
    public Image grabbedItem;

    public void OuMouseDownItem(Item _item)
    {
        if(_item == null)
        {
            currentAttach = _item;
            grabbedItem.gameObject.SetActive(true);
            grabbedItem.sprite = currentAttach.GetComponent<Image>().sprite;
        }
    }
}
