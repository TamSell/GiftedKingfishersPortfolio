using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goDie : MonoBehaviour
{
    public void GoDie()
    {
        gameManager.Instance.death();
    }
}
