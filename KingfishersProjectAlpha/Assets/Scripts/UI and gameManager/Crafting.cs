using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    private ItemHolder currentAttach;
    public Image grabbedItem;

    public AttachmentSlots[] equipped;

    private Item trueAttachment;

    private void Awake()
    {
        if(currentAttach != null)
        trueAttachment = currentAttach.itemObject;
    }
    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(currentAttach != null)
            {
                grabbedItem.gameObject.SetActive(false);
                AttachmentSlots nearest = null;
                float closestDist = float.MaxValue;

                foreach(AttachmentSlots slot in equipped) 
                { 
                float dist = Vector2.Distance(Input.mousePosition, slot.transform.position);
                    if(dist < closestDist)
                    {
                        closestDist= dist;
                        nearest = slot;
                    }
                }
                nearest.gameObject.SetActive(true);
                nearest.GetComponent<Image>().sprite = grabbedItem.gameObject.GetComponent<Image>().sprite;
                nearest.item = currentAttach;
                currentAttach= null;
            }
        }
    }
    public void OuMouseDownItem(ItemHolder _item)
    {
        if(currentAttach == null)
        {
            currentAttach = _item;
            grabbedItem.gameObject.SetActive(true);
            grabbedItem.sprite = currentAttach.GetComponent<Image>().sprite;
        }
    }
}
