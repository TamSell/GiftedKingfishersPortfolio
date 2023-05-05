using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Portals : MonoBehaviour
{
    [SerializeField] Transform exit;
    [SerializeField] Transform portal;
    [SerializeField] GameObject player;
    [SerializeField] UnityEngine.Vector3 offset;

    private UnityEngine.Vector3 playerPosition;
    private UnityEngine.Vector3 playerRotation;
    private float angleDiff;
    private UnityEngine.Vector3 seeThroughPos;
    private UnityEngine.Quaternion rotationRelation;
    private UnityEngine.Vector3 positionRelation;

    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !gameManager.Instance.teleporting)
        {
            StartCoroutine(Teleport());
        }
    }

    IEnumerator Teleport()
    {
        gameManager.Instance.playerController.enabled= false;
        gameManager.Instance.teleporting = true;
        //Get players position and rotation
        playerPosition = player.transform.position;
        playerRotation = player.transform.eulerAngles;
        //Obtain relational rotation and position to portal
        positionRelation.Set(transform.position.x - playerPosition.x, playerPosition.y, 0);
        //Move the player and set the new camera rotation
        exit.localPosition = exit.localPosition + offset;
        player.transform.localPosition = exit.position + offset;
        yield return new WaitForSeconds(0.2f);
        gameManager.Instance.teleporting = false;
        gameManager.Instance.playerController.enabled= true;

    }
}
