using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPlayer : MonoBehaviour
{
    [SerializeField] FinalPlayerController m_Player;
    [SerializeField] Transform currentPortal;
    [SerializeField] Transform nextPortal;

    private Vector3 currentVelocity;
    public Vector3 position;
    private Vector3 rotationRelation;

    private void OnTriggerEnter(Collider other)
    {
        if (!m_Player.isTeleporting)
            StartCoroutine(Teleport());
        else
            StartCoroutine(TeleportCD());
    }


    IEnumerator Teleport()
    {
        m_Player.isTeleporting = true;
        currentVelocity = m_Player.PlayerBody.velocity;
        gameManager.Instance.playerController.PlayerBody.isKinematic = true;
        //Movement
        position = m_Player.transform.position - currentPortal.position;
        m_Player.transform.position = nextPortal.position + position;
        //Rotation
        rotationRelation = currentPortal.forward - m_Player.transform.forward;
        Quaternion relation = Quaternion.Euler(rotationRelation.x - nextPortal.forward.x, rotationRelation.y - nextPortal.forward.y, rotationRelation.z - nextPortal.forward.z);
        m_Player.PlayerBody.MoveRotation(relation);
        m_Player.transform.position += nextPortal.transform.forward * 2.5f;
        yield return new WaitForSeconds(0.1f);
        gameManager.Instance.playerController.PlayerBody.isKinematic = false;
        m_Player.PlayerBody.velocity = currentVelocity;
    }
    IEnumerator TeleportCD()
    {
        yield return new WaitForSeconds(0.2f);
        m_Player.isTeleporting = false;
    }
}