using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string name;
    public string description;
    public Image icon;
    public int amount;
}
