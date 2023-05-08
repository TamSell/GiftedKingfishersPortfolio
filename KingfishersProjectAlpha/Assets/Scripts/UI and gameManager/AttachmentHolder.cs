using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttachmentHolder : MonoBehaviour
{
    public Attachment attachObject;
    [SerializeField] public Image currentImage = null;

    private void Awake()
    {
        if (currentImage)
            currentImage.sprite = attachObject.icon;
    }
}
