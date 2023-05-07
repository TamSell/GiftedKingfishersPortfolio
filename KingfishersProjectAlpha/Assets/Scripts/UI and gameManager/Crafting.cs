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

    public GunHolder result;

    private Item trueAttachment;
    private float dist;

    private void Awake()
    {
        if (gameManager.Instance.currentGunAspects)
        {
            result.gunHeld = gameManager.Instance.currentGunAspects;
        }
        else
            result.gunHeld = new GunStats2();
    }
    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(currentAttach != null)
            {
                trueAttachment = currentAttach.itemObject;
                grabbedItem.gameObject.SetActive(false);
                AttachmentSlots nearest = null;
                for(int x = 0; x < equipped.Length;x++)
                {
                    dist = Vector2.Distance(Input.mousePosition, equipped[x].transform.position);
                    if(dist < 100 && currentAttach != null && currentAttach.itemObject.position == x)
                    {
                        nearest = equipped[x];
                        result.gunHeld.Attachments[x] = currentAttach.itemObject;
                    }
                }
                //foreach(AttachmentSlots slot in equipped) 
                //{
                //    float dist = Vector2.Distance(Input.mousePosition, slot.transform.position);
                //    if(closestDist == float.MaxValue || (dist-closestDist <= 100))
                //    {
                //        closestDist= dist;
                //        nearest = slot;
                //    }
                //}
                if(nearest != null)
                {
                    nearest.gameObject.SetActive(true);
                    nearest.GetComponent<Image>().sprite = grabbedItem.gameObject.GetComponent<Image>().sprite;
                    nearest.GetComponent<AttachmentSlots>().description.text = trueAttachment.description;
                    nearest.item = currentAttach;
                }
                currentAttach = null;
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

    public void NextGun()
    {
        result.gunHeld = gameManager.Instance.currentGunAspects;
        GunStats2 newGun = result.gunHeld;
        if(newGun != null)
        {
            for (int x = 0; x < 4; x++)
            {
                if (newGun.Attachments[x] != null)
                {
                    equipped[x].GetComponent<Image>().sprite = newGun.Attachments[x].icon;
                    equipped[x].GetComponent<AttachmentSlots>().description.text = newGun.Attachments[x].description;
                }
            }
        }
    }
}
