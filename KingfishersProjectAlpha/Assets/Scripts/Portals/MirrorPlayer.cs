using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPlayer : MonoBehaviour
{
    [SerializeField] Rigidbody m_Player;
    [SerializeField] Transform currentPortal;
    [SerializeField] GameObject nextPortal;

    public Vector3 position;
    public Boolean teleporting;
    private Vector3 rotationRelation;

    private void OnTriggerEnter(Collider other)
    {
        if (teleporting == false)
            StartCoroutine(Teleport());
    }


    IEnumerator Teleport()
    {
        teleporting = true;
        gameManager.Instance.playerController.PlayerBody.isKinematic = true;
        //Movement
        position = m_Player.transform.position - currentPortal.transform.position;
        position.x = Mathf.Clamp(position.x, -10, 10);
        position.y = Mathf.Clamp(position.y, -5, 5);
        m_Player.transform.position = position + nextPortal.transform.position;
        //Rotation
        rotationRelation = m_Player.transform.forward - currentPortal.transform.forward;
        m_Player.transform.rotation = Quaternion.Euler(rotationRelation + nextPortal.transform.forward);
        m_Player.transform.position += m_Player.transform.forward * 1f;
        yield return new WaitForSeconds(0.1f);
        gameManager.Instance.playerController.PlayerBody.isKinematic = false;
        teleporting = false;
    }
}