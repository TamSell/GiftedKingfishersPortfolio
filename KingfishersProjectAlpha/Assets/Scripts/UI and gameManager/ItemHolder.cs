using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
    public Item itemObject;
    [SerializeField] public Image currentImage = null;

    private void Awake()
    {
        currentImage.sprite = itemObject.icon;
    }
}
