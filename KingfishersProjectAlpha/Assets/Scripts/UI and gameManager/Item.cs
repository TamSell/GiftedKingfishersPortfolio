using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string named;
    [TextArea(15,20)]
    public string description;
    public Sprite icon;
    List<int> modifiers;
    public int position;
}
