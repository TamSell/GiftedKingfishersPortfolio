using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadshotTurret : MonoBehaviour, Damage
{
    public void TakeDamage(int amountDamage)
    {
        amountDamage += 5;
        this.GetComponentInParent<EnemyTurret>().TakeDamage(amountDamage);
        StartCoroutine(flashColor());
    }
    IEnumerator flashColor()
    {
        this.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<Renderer>().material.color = Color.white;
    }
}
