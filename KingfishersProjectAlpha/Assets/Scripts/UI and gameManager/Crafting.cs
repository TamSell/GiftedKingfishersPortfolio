using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    private AttachmentHolder currentAttach;
    public Image grabbedItem;

    public AttachmentSlots[] equipped;

    public GunHolder result;
    public TextMeshProUGUI gunStatsText;

    private Attachment trueAttachment;
    private float dist;
    private int AttachmentIndex;

    private void Awake()
    {
        if (gameManager.Instance.currentGunAspects)
        {
            result.gunHeld = gameManager.Instance.currentGunAspects;
        }
        else
            result.gunHeld = new GunStats2();
        displayStats();
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (currentAttach != null)
            {
                trueAttachment = currentAttach.attachObject;
                grabbedItem.gameObject.SetActive(false);
                AttachmentSlots nearest = null;
                for (int x = 0; x < equipped.Length; x++)
                {
                    dist = Vector2.Distance(Input.mousePosition, equipped[x].transform.position);
                    if (dist < 100 && currentAttach != null && currentAttach.attachObject.position == x)
                    {
                        AttachmentIndex = x;
                        nearest = equipped[x];
                        UpdateGun();
                        displayStats();
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
                if (nearest != null)
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
    public void OuMouseDownItem(AttachmentHolder _item)
    {
        if (currentAttach == null)
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
        if (newGun != null)
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

    public void UpdateGun()
    {
        if (currentAttach != null)
        {
                if (result.gunHeld.Attachments[AttachmentIndex] != null && result.gunHeld.Attachments[AttachmentIndex].equipped)
                {
                    result.gunHeld.damage -= result.gunHeld.Attachments[AttachmentIndex].modifiedStats[0];
                    result.gunHeld.shootRange -= result.gunHeld.Attachments[AttachmentIndex].modifiedStats[1];
                    result.gunHeld.realoadSpeed -= result.gunHeld.Attachments[AttachmentIndex].modifiedStats[2];
                    result.gunHeld.magSize -= result.gunHeld.Attachments[AttachmentIndex].modifiedStats[3];
                    result.gunHeld.recoil -= result.gunHeld.Attachments[AttachmentIndex].modifiedStats[4];
                    result.gunHeld.shootRate -= result.gunHeld.Attachments[AttachmentIndex].modifiedStats[5];
                    result.gunHeld.Attachments[AttachmentIndex].equipped = false;
                }
                Attachment current = currentAttach.attachObject;
                current.equipped = true;
                result.gunHeld.damage += current.modifiedStats[0];
                result.gunHeld.shootRange += current.modifiedStats[1];
                result.gunHeld.realoadSpeed += current.modifiedStats[2];
                result.gunHeld.magSize += current.modifiedStats[3];
                result.gunHeld.recoil += current.modifiedStats[4];
                result.gunHeld.shootRate += current.modifiedStats[5];

        }
    }

    public void ResetGun()
    {
        for (int x = 0; x < 4; x++)
        {
            if (result.gunHeld.Attachments[x].equipped)
            {
                result.gunHeld.damage -= result.gunHeld.Attachments[x].modifiedStats[0];
                result.gunHeld.shootRange -= result.gunHeld.Attachments[x].modifiedStats[1];
                result.gunHeld.realoadSpeed -= result.gunHeld.Attachments[x].modifiedStats[2];
                result.gunHeld.magSize -= result.gunHeld.Attachments[x].modifiedStats[3];
                result.gunHeld.recoil -= result.gunHeld.Attachments[x].modifiedStats[4];
                result.gunHeld.shootRate -= result.gunHeld.Attachments[x].modifiedStats[5];
                result.gunHeld.Attachments[x].equipped = false;
                result.gunHeld.Attachments[x] = null;
            }
        }
    }

    public void displayStats()
    {
        gunStatsText.text = "";
        if (result.gunHeld != null)
        {
            GunStats2 currentGun = result.gunHeld;
            gunStatsText.text += "Damage: " + currentGun.damage.ToString() + "\n";
            gunStatsText.text += "Range: " + currentGun.shootRange.ToString() + "\n";
            gunStatsText.text += "Reload: " + currentGun.realoadSpeed.ToString() + "\n";
            gunStatsText.text += "Mag Size: " + currentGun.magSize.ToString() + "\n";
            gunStatsText.text += "Recoil: " + currentGun.recoil.ToString() + "\n";
            gunStatsText.text += "Rate of Fire: " + currentGun.shootRate.ToString() + "\n";
        }
    }
}
